using FlightService.DTOs;
using FlightService.DTOs.Requests;
using FlightService.DTOs.Responses;
using FlightService.Grpc.Clients;
using FlightService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace FlightService.Services;

public class FlightService : IFlightService
{
    private readonly FlightDbContext flightDbContext;
    private readonly BookingServiceClient bookingServiceClient;

    public FlightService(FlightDbContext flightDbContext, BookingServiceClient bookingServiceClient)
    {
        this.flightDbContext = flightDbContext;
        this.bookingServiceClient = bookingServiceClient;
    }


    public BaseResponse AddFlight(AddFlightRequest request)
    {
        try
        {
            this.flightDbContext.Flights.Add(new FlightModel(request));
            this.flightDbContext.SaveChanges();
            return new BaseResponse(StatusCodes.Status200OK, "Flight added successfully");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }
    
    public BaseResponse GetFlights()
    {
        try
        {
            List<FlightDTO> flights = flightDbContext.Flights.Select(f => new FlightDTO(f)).ToList();
            return new BaseResponse(StatusCodes.Status200OK, flights);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }
    
    public BaseResponse GetFlightById(long id)
    {
        try
        {
            FlightModel? flight = flightDbContext.Flights.FirstOrDefault(f => f.id == id);
            FlightDTO? flightDTO = flight != null ? new FlightDTO(flight) : null;
            return new BaseResponse(flightDTO == null ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK, flightDTO == null ? "No record found" : flightDTO);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }


    public BaseResponse GetFlightBookings(long id)
    {
        try
        {
            FlightModel? flight = flightDbContext.Flights.FirstOrDefault(f => f.id == id);
            BaseResponse response;

            if (flight == null)
            {
                response = new BaseResponse(StatusCodes.Status400BadRequest, "No flight found");
            }
            else
            {
                // get bookings
                BaseResponse filterBookingResponse = this.bookingServiceClient.FilterBookings(flight.id);

                if (filterBookingResponse.statusCode == StatusCodes.Status500InternalServerError)
                {
                    response = new BaseResponse(StatusCodes.Status412PreconditionFailed, "Precondition Failed");
                }
                else
                {
                    List<BookingDTO>? bookingDTOs = filterBookingResponse.data as List<BookingDTO>;

                    if (bookingDTOs == null)
                    {
                        response = new BaseResponse(StatusCodes.Status412PreconditionFailed,
                            "Unable to parse booking data");
                    }
                    else
                    {
                        if (bookingDTOs.Count == 0)
                        {
                            response = new BaseResponse(StatusCodes.Status202Accepted, "No booking data");
                        }
                        else
                        {
                            // setup response
                            FlightBookingDTO flightBookingDTO = new FlightBookingDTO(flight);
                            foreach (BookingDTO bookingDTO in bookingDTOs)
                            {
                                flightBookingDTO.AddBooking(bookingDTO);
                            }
                        
                            flightBookingDTO.SetCountOfSeats();
                        
                            response = new BaseResponse(StatusCodes.Status200OK, flightBookingDTO);
                        }
                    }
                }
            }
            
            return response;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }
}