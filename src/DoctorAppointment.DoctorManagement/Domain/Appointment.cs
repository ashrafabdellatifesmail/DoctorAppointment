using DoctorAppointment.Shared.Constants;

namespace DoctorAppointment.DoctorManagement.Domain
{
    public class Appointment
    {
        public void Create(Guid id, string status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; private set; }
        public string Status { get; private set; }

        public void MarkAsCompleted()
        {
            Status = SystemConstants.AppointmentStatus.Completed;
        }

        public void Cancel()
        {
            Status = SystemConstants.AppointmentStatus.Cancelled;
        }
    }
}
