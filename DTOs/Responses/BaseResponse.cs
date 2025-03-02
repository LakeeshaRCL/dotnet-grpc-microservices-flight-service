using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FlightService.DTOs.Responses;

public class BaseResponse
{
    public int statusCode { get; set; }
    
    [DefaultValue(null)]
    public object? data { get; set; }

    public BaseResponse(int statusCode, object data)
    {
        this.statusCode = statusCode;
        this.data = data;
    }
    
    
    public BaseResponse(int statusCode, string message)
    {
        this.statusCode = statusCode;
        this.data = new MessageDTO(message);
    }
}