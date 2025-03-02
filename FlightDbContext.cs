using FlightService.Helpers;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService;

public class FlightDbContext : DbContext
{
    private MySqlConfiguration mySqlConfiguration;

    public FlightDbContext(GlobalSingletonProperties globalSingletonProperties)
    {
        this.mySqlConfiguration = globalSingletonProperties.mySqlConfiguration;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                connectionString: this.mySqlConfiguration.mySqlConnection,
                serverVersion: this.mySqlConfiguration.GetMySqlVersion());
        }
    }

    public DbSet<FlightModel> Flights { get; set; }
    
}