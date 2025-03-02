

using System.Net;
using FlightService;
using FlightService.Grpc.Clients;
using FlightService.Grpc.Servers;
using FlightService.Helpers;
using FlightService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// set global access values
GlobalSingletonProperties globalSingletonProperties = new GlobalSingletonProperties
{
    mySqlConfiguration = builder.Configuration.GetSection("MySqlConfiguration").Get<MySqlConfiguration>()?? throw new ArgumentNullException()
};

builder.Services.AddSingleton(globalSingletonProperties);
builder.Services.AddDbContext<FlightDbContext>();
builder.Services.AddScoped<IFlightService, FlightService.Services.FlightService>();
builder.Services.AddScoped(s => new BookingServiceClient("http://localhost:7073"));

builder.Services.AddGrpc(); // add grpc
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// setting up http2 for gRPC and http1 for REST communications
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(7071, listenOptions =>{listenOptions.Protocols = HttpProtocols.Http2;}); // for gRPC communication
    o.ListenAnyIP(7070, listenOptions =>{listenOptions.Protocols = HttpProtocols.Http1;}); // for REST communication
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.MapGrpcService<FlightServiceGrpcServer>();


// create database
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    FlightDbContext flightDbContext = serviceScope.ServiceProvider.GetRequiredService<FlightDbContext>();
    flightDbContext.Database.EnsureCreated();
}


app.Run();