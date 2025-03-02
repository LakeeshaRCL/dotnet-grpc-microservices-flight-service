using Microsoft.EntityFrameworkCore;

namespace FlightService.Helpers;

public class GlobalSingletonProperties
{
    public required MySqlConfiguration mySqlConfiguration;
}


public class MySqlConfiguration
{
    public required string mySqlConnection { get; set; }
    public required string mySqlVersion { get; set; }

    
    public ServerVersion GetMySqlVersion()
    {
        return ServerVersion.Parse(mySqlVersion);
    }
}