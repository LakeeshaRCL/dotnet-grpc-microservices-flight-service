using BookingServiceProtoService;
using FlightService.DTOs;
using FlightService.DTOs.Responses;
using Grpc.Net.Client;

namespace FlightService.Grpc.Clients;

public class BookingServiceClient
{
    private readonly BookingServiceProtoService.BookingServiceProtoService.BookingServiceProtoServiceClient client;

    public BookingServiceClient(string gRpcHostUrl)
    {
        GrpcChannel channel = GrpcChannel.ForAddress(gRpcHostUrl);
        this.client = new BookingServiceProtoService.BookingServiceProtoService.BookingServiceProtoServiceClient(channel);
    }


    public BaseResponse FilterBookings(long flightId)
    {
        try
        {
            FilterBookingsGrpcResponse grpcResponse =
                this.client.FilterBookings(new FilterBookingsRequest { FlightId = flightId });

            List<BookingDTO> bookingDTOs = new List<BookingDTO>();

            if (grpcResponse.Bookings != null)
            {
                bookingDTOs = grpcResponse.Bookings.Select(b => new BookingDTO
                {
                    bookingDate = b.BookingDate,
                    flightId = b.FlightId,
                    ticketCount = b.TicketCount,
                    userId = b.UserId
                }).ToList();
            }
            
            return new BaseResponse(grpcResponse.StatusCode, bookingDTOs);
            
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return new BaseResponse(StatusCodes.Status500InternalServerError, "gRPC client error");
        }
    }
}