using DoctorAppointment.AppointmentBooking.Application.Handlers;
using DoctorAppointment.AppointmentConfirmation;
using DoctorAppointment.DoctorAvailability.Interfaces;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.DoctorAvailability.Services;
using DoctorAppointment.DoctorManagement.Application;
using DoctorAppointment.DoctorManagement.Application.Ports.Input;
using DoctorAppointment.DoctorManagement.Application.Ports.Output;
using DoctorAppointment.Infrastructure.Repositories;
using DoctorAppointment.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Doctor Appointment API",
        Version = "v1"
    });
});
// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Register modules
builder.Services.AddDoctorAvailabilityModule();
builder.Services.AddAppointmentBookingModule();
builder.Services.AddAppointmentConfirmationModule();
builder.Services.AddDoctorManagementModule();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Doctor Appointment API V1");
        c.RoutePrefix = "swagger";
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Global error handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 500,
                Message = ex.Message
            });
        }
    });
});

// Module specific endpoints registration
app.MapModuleEndpoints();

// Map controllers
app.MapControllers();

app.Run();

// Make the Program class public for testing purposes
public partial class Program { }

// Extension method for registering module endpoints
public static class ModuleEndpointsExtensions
{
    public static WebApplication MapModuleEndpoints(this WebApplication app)
    {
        // Health check endpoint
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
           .WithName("HealthCheck")
           .WithOpenApi();

        // Add other module-specific endpoints here
        return app;
    }
}

// Extension methods for registering module services
public static class ModuleServiceExtensions
{
    public static IServiceCollection AddDoctorAvailabilityModule(this IServiceCollection services)
    {
        services.AddSingleton<ISlotRepository, InMemorySlotRepository>();
        services.AddScoped<ISlotService, SlotService>();
        return services;
    }

    public static IServiceCollection AddAppointmentBookingModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BookAppointmentHandler).Assembly));
        services.AddScoped<IAppointmentRepository, InMemoryAppointmentRepository>();
        services.AddScoped<DoctorAppointment.AppointmentBooking.Domain.Interfaces.IAppointmentRepository, DoctorAppointment.AppointmentBooking.Infrastructure.Repositories.AppointmentRepository>();
        return services;
    }

    public static IServiceCollection AddAppointmentConfirmationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }

    public static IServiceCollection AddDoctorManagementModule(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentManagementUseCase, AppointmentManagementService>();
        // Using the same repository instance for management
        services.AddScoped<IAppointmentRepository,
            InMemoryAppointmentRepository>();
        return services;
    }
}