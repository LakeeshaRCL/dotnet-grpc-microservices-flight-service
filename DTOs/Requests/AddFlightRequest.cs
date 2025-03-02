using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightService.DTOs.Requests;

public class AddFlightRequest
{
    [Required]
    public required string airlineName { get; set; }
    
    [Required]
    public required string source { get; set; }
    
    public required string destination { get; set; }
    
    public int availableSeats { get; set; }
    
    public DateTime departureTime { get; set; }
    
    public DateTime arrivalTime { get; set; }
}