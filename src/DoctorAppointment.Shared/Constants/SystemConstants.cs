
namespace DoctorAppointment.Shared.Constants
{
    public static class SystemConstants
    {
        public static class AppointmentStatus
        {
            public const string Scheduled = "Scheduled";
            public const string Completed = "Completed";
            public const string Cancelled = "Cancelled";
        }

        public static readonly Guid DefaultDoctorId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public const string DefaultDoctorName = "Dr. John Doe";
    }
}
