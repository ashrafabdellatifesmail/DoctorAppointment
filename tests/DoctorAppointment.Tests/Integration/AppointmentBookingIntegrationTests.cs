using DoctorAppointment.AppointmentBooking.Application.Commands;
using DoctorAppointment.Shared.Constants;
using DoctorAppointment.Shared.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace DoctorAppointment.Tests.Integration
{
    public class AppointmentBookingIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AppointmentBookingIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CompleteAppointmentBookingFlow()
        {
            // Step 1: Create a slot
            var createSlotResponse = await _client.PostAsJsonAsync("/api/slot", new SlotDto
            {
                Time = DateTime.Now.AddDays(1),
                Cost = 100m
            });

            var slot = await createSlotResponse.Content.ReadFromJsonAsync<SlotDto>();
            Assert.NotNull(slot);

            // Step 2: Book the appointment
            var bookingCommand = new BookAppointmentCommand(
                slot.Id,
                Guid.NewGuid(),
                "Test Patient"
            );

            var bookingResponse = await _client.PostAsJsonAsync("/api/booking/book", bookingCommand);
            var appointment = await bookingResponse.Content.ReadFromJsonAsync<AppointmentDto>();
            Assert.NotNull(appointment);

            // Step 3: Verify the slot is now reserved
            var availableSlotsResponse = await _client.GetFromJsonAsync<List<SlotDto>>("/api/booking/available-slots");
            Assert.DoesNotContain(availableSlotsResponse, s => s.Id == slot.Id);

            // Step 4: Complete the appointment
            var completeResponse = await _client.PutAsync(
                $"/api/doctor/appointments/{appointment.Id}/complete",
                new StringContent(""));

            var completedAppointment = await completeResponse.Content.ReadFromJsonAsync<AppointmentDto>();
            Assert.Equal(SystemConstants.AppointmentStatus.Completed, completedAppointment.Status);
        }
    }
}
