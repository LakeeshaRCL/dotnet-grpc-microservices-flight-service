using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FlightService.DTOs.Requests;

namespace FlightService.Models;

[Table("flight")]
public class FlightModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id { get; set; }
    
    [Column("airline_name", TypeName = "varchar(100)")]
    public required string airlineName {get; set;} 
    
    [Column(TypeName = "varchar(50)")]
    public required string source {get; set;} 
    
    [Column(TypeName = "varchar(50)")]
    public required string destination {get; set;} 
    
    [Column("departure_time")]
    public required DateTime departureTime {get; set;} 
    
    [Column("arrival_time")] 
    public required DateTime arrivalTime {get; set;} 
    
    [Column("available_seats")]
    public int availableSeats {get; set;}
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("created_at")]
    public DateTime createdAt { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("updated_at")]
    public DateTime updatedAt { get; set; }


    public FlightModel()
    {
        // default constructor
    }

    [SetsRequiredMembers]
    public FlightModel(AddFlightRequest request)
    {
        this.airlineName = request.airlineName;
        this.source = request.source;
        this.destination = request.destination;
        this.departureTime = request.departureTime;
        this.arrivalTime = request.arrivalTime;
        this.availableSeats = request.availableSeats;
    }
}