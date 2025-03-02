using FlightService.DTOs;
using FlightService.DTOs.Responses;
using FlightService.Services;
using FlightServiceProtoService;
using Grpc.Core;

namespace FlightService.Grpc.Servers;

public class FlightServiceGrpcServer: FlightServiceProtoService.FlightServiceProtoService.FlightServiceProtoServiceBase
{
    // define your server's logic here
    private readonly IFlightService flightService;


    public FlightServiceGrpcServer(IFlightService flightService)
    {
        this.flightService = flightService;
    }

    public override Task<GetFlightGrpcResponse> GetFlight(GetFlightRequest request, ServerCallContext context)
    {
        BaseResponse baseResponse = this.flightService.GetFlightById(request.Id);
        FlightDTO? flightDTO = baseResponse.data as FlightDTO;
        
        // define grpc response object
        GetFlightGrpcResponse grpcResponse = new GetFlightGrpcResponse();
        grpcResponse.StatusCode = baseResponse.statusCode;
        
        // update response
        if (flightDTO != null)
        {
            Flight flight = new Flight
            {
                AirlineName = flightDTO.airlineName,
                Source = flightDTO.source,
                Destination = flightDTO.destination,
                AvailableSeats = flightDTO.availableSeats,
                DepartureTime = flightDTO.departureTime,
                ArrivalTime = flightDTO.arrivalTime,
            };
            grpcResponse.Flight = flight;
        }

        // return data 
        return Task.FromResult(grpcResponse);
    }
}