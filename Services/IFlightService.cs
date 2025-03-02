using FlightService.DTOs;
using FlightService.DTOs.Requests;
using FlightService.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Services;

public interface IFlightService
{
    BaseResponse AddFlight(AddFlightRequest request);
    BaseResponse GetFlights();
    BaseResponse GetFlightById(long id);
    BaseResponse GetFlightBookings(long id);
}