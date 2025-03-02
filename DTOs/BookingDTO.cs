namespace FlightService.DTOs;

public class BookingDTO
{
    public long flightId { get; set; }
    
    public long userId { get; set; }
    
    public int ticketCount { get; set; }
    
    public string bookingDate { get; set; }
}