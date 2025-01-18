

using DoctorAppointment.DoctorManagement.Application.Ports.Output;
using DoctorAppointment.DoctorManagement.Domain;
using DoctorAppointment.Shared.Constants;

namespace DoctorAppointment.Infrastructure.Repositories
{
    public class InMemoryAppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new();

        public async Task<Appointment> Create(Appointment appointment)
        {
            _appointments.Add(appointment);
            return await Task.FromResult(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointments()
        {
            return await Task.FromResult(_appointments.Where(a =>
                a.Status == SystemConstants.AppointmentStatus.Scheduled));
        }

        public async Task<Appointment> GetById(Guid id)
        {
            return await Task.FromResult(_appointments.FirstOrDefault(a => a.Id == id));
        }

        public async Task<Appointment> Update(Appointment appointment)
        {
            var existingAppointment = _appointments.FirstOrDefault(a => a.Id == appointment.Id);
            if (existingAppointment != null)
            {
                _appointments.Remove(existingAppointment);
                _appointments.Add(appointment);
            }
            return await Task.FromResult(appointment);
        }
    }
}
