using DoctorAppointment.Shared.Interfaces;
using DoctorAppointment.Shared.Models;
using Microsoft.Extensions.Logging;

namespace DoctorAppointment.AppointmentConfirmation
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendAppointmentConfirmation(AppointmentDto appointment, SlotDto slot)
        {
            var message = $@"
                Appointment Confirmation
                -----------------------
                Patient: {appointment.PatientName}
                Doctor: {slot.DoctorName}
                Date: {slot.Time:dd/MM/yyyy hh:mm tt}
                Appointment ID: {appointment.Id}
                Status: {appointment.Status}
                Cost: ${slot.Cost}
                ";

            _logger.LogInformation(message);

            // In a real application, you would send emails/SMS here
            await Task.CompletedTask;
        }
    }
}
