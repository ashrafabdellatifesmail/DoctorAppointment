using DoctorAppointment.Shared.Constants;

namespace DoctorAppointment.AppointmentBooking.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; private set; }
        public Guid SlotId { get; private set; }
        public Guid PatientId { get; private set; }
        public string PatientName { get; private set; }
        public DateTime ReservedAt { get; private set; }
        public string Status { get; private set; }

        private Appointment() { }

        public static Appointment Create(Guid slotId, Guid patientId, string patientName)
        {
            return new Appointment
            {
                Id = Guid.NewGuid(),
                SlotId = slotId,
                PatientId = patientId,
                PatientName = patientName,
                ReservedAt = DateTime.UtcNow,
                Status = SystemConstants.AppointmentStatus.Scheduled
            };
        }
    }
}
