using DoctorAppointment.DoctorManagement.Application.Ports.Input;
using DoctorAppointment.DoctorManagement.Application.Ports.Output;
using DoctorAppointment.Shared.Interfaces;
using DoctorAppointment.Shared.Models;

namespace DoctorAppointment.DoctorManagement.Application
{
    public class AppointmentManagementService : IAppointmentManagementUseCase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly INotificationService _notificationService;

        public AppointmentManagementService(
            IAppointmentRepository appointmentRepository,
            INotificationService notificationService)
        {
            _appointmentRepository = appointmentRepository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments()
        {
            var appointments = await _appointmentRepository.GetUpcomingAppointments();
            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Status = a.Status
                // Map other properties
            });
        }

        public async Task<AppointmentDto> MarkAppointmentAsCompleted(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetById(appointmentId);

            if(appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            appointment.MarkAsCompleted();
            var updated = await _appointmentRepository.Update(appointment);

            return new AppointmentDto
            {
                Id = updated.Id,
                Status = updated.Status
                // Map other properties
            };
        }

        public async Task<AppointmentDto> CancelAppointment(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetById(appointmentId);
            appointment.Cancel();
            var updated = await _appointmentRepository.Update(appointment);

            return new AppointmentDto
            {
                Id = updated.Id,
                Status = updated.Status
                // Map other properties
            };
        }
    }
}
