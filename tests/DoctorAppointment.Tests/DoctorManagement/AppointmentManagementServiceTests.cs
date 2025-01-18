using DoctorAppointment.DoctorManagement.Application;
using DoctorAppointment.DoctorManagement.Application.Ports.Output;
using DoctorAppointment.DoctorManagement.Domain;
using DoctorAppointment.Shared.Constants;
using DoctorAppointment.Shared.Interfaces;
using Moq;

namespace DoctorAppointment.Tests.DoctorManagement
{
    public class AppointmentManagementServiceTests
    {
        private readonly Mock<IAppointmentRepository> _mockRepository;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly AppointmentManagementService _service;

        public AppointmentManagementServiceTests()
        {
            _mockRepository = new Mock<IAppointmentRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _service = new AppointmentManagementService(
                _mockRepository.Object,
                _mockNotificationService.Object);
        }

        [Fact]
        public async Task MarkAppointmentAsCompleted_UpdatesStatus()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment ();

            _mockRepository.Setup(r => r.GetById(appointmentId))
                .ReturnsAsync(appointment);
            _mockRepository.Setup(r => r.Update(It.IsAny<Appointment>()))
                .ReturnsAsync((Appointment a) => a);

            // Act
            var result = await _service.MarkAppointmentAsCompleted(appointmentId);

            // Assert
            Assert.Equal(SystemConstants.AppointmentStatus.Completed, result.Status);
        }

        [Fact]
        public async Task CancelAppointment_UpdatesStatus()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment ();
            _mockRepository.Setup(r => r.GetById(appointmentId))
                .ReturnsAsync(appointment);
            _mockRepository.Setup(r => r.Update(It.IsAny<Appointment>()))
                .ReturnsAsync((Appointment a) => a);

            // Act
            var result = await _service.CancelAppointment(appointmentId);

            // Assert
            Assert.Equal(SystemConstants.AppointmentStatus.Cancelled, result.Status);
        }
    }
}
