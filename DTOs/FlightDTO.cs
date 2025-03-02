using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FlightService.Models;

namespace FlightService.DTOs;

public class FlightDTO
{
    public required string airlineName { get; set; }
    
    public required string source { get; set; }
    
    public required string destination { get; set; }
    
    public int availableSeats { get; set; }

    public string departureTime { get; set; }
    
    public string arrivalTime { get; set; }


    public FlightDTO()
    {
        // default
    }

    [SetsRequiredMembers]
    public FlightDTO( FlightModel flight)
    {
        this.airlineName = flight.airlineName;
        this.source = flight.source;
        this.destination = flight.destination;
        this.source = flight.source;
        this.availableSeats = flight.availableSeats;
        this.departureTime = flight.departureTime.ToString("s");
        this.arrivalTime = flight.arrivalTime.ToString("s");
    }
}