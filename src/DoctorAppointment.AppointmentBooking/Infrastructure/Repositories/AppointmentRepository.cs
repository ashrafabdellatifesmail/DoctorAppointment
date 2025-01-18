using DoctorAppointment.AppointmentBooking.Domain.Entities;
using DoctorAppointment.AppointmentBooking.Domain.Interfaces;

namespace DoctorAppointment.AppointmentBooking.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new();
        public async Task<Appointment> Create(Appointment appointment)
        {
            _appointments.Add(appointment);
            return await Task.FromResult(appointment);
        }
    }
}
