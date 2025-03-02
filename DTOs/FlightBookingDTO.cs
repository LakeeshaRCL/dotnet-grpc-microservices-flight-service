using System.Diagnostics.CodeAnalysis;
using FlightService.Models;

namespace FlightService.DTOs;

public class FlightBookingDTO : FlightDTO
{
    public List<BookingDTO> bookings { get; set; }
    public int bookedSeatCount { get; set; }
    public int remainingSeatCount { get; set; }


    [SetsRequiredMembers]
    public FlightBookingDTO(FlightModel flight) : base(flight)
    {
        this.bookings = new List<BookingDTO>();
        this.remainingSeatCount = 0;
        this.bookedSeatCount = 0;
    }


    public void AddBooking(BookingDTO booking)
    {
        this.bookings.Add(booking);
    }


    public void SetCountOfSeats()
    {
        bookedSeatCount = bookings.Sum(b => b.ticketCount);
        remainingSeatCount = availableSeats - bookedSeatCount;
    }
}