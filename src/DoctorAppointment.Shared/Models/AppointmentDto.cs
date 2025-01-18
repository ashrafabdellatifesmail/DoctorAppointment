
namespace DoctorAppointment.Shared.Models
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime ReservedAt { get; set; }
        public string Status { get; set; }
    }
}
