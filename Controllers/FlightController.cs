using FlightService.DTOs.Requests;
using FlightService.DTOs.Responses;
using FlightService.Grpc.Clients;
using FlightService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers
{
    [Route("api/flight")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService flightService;

        public FlightController(IFlightService flightService)
        {
            this.flightService = flightService;
        }


        [HttpPost]
        public BaseResponse AddFlight(AddFlightRequest request)
        {
            return this.flightService.AddFlight(request);
        }

        [HttpGet]
        public BaseResponse GetFlights()
        {
            return this.flightService.GetFlights();
        }

        [HttpGet("{flightId}")]
        public BaseResponse GetFlight([FromRoute]long flightId)
        {
            return this.flightService.GetFlightById(flightId);   
        }


        [HttpGet("booking/{id}")]
        public BaseResponse GetFlightBookings([FromRoute]long id)
        {
            return this.flightService.GetFlightBookings(id);
        }
    }
}
