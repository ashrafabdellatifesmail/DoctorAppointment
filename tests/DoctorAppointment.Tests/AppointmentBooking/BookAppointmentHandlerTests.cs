
using DoctorAppointment.AppointmentBooking.Application.Commands;
using DoctorAppointment.AppointmentBooking.Application.Handlers;
using DoctorAppointment.AppointmentBooking.Domain.Entities;
using DoctorAppointment.DoctorAvailability.Models;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.DoctorManagement.Application.Ports.Output;
using DoctorAppointment.Shared.Constants;
using DoctorAppointment.Shared.Interfaces;
using Moq;

namespace DoctorAppointment.Tests.AppointmentBooking
{
    public class BookAppointmentHandlerTests
    {
        private readonly Mock<DoctorAppointment.AppointmentBooking.Domain.Interfaces.IAppointmentRepository> _mockAppointmentRepo;
        private readonly Mock<ISlotRepository> _mockSlotRepo;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly BookAppointmentHandler _handler;

        public BookAppointmentHandlerTests()
        {
            _mockAppointmentRepo = new Mock<DoctorAppointment.AppointmentBooking.Domain.Interfaces.IAppointmentRepository>();
            _mockSlotRepo = new Mock<ISlotRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _handler = new BookAppointmentHandler(
                _mockAppointmentRepo.Object,
                _mockSlotRepo.Object,
                _mockNotificationService.Object);
        }

        [Fact]
        public async Task Handle_WithValidSlot_CreatesAppointment()
        {
            // Arrange
            var command = new BookAppointmentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "John Doe"
            );

            var slot = new Slot
            {
                Id = command.SlotId,
                IsReserved = false
            };

            _mockSlotRepo.Setup(r => r.GetSlotById(command.SlotId))
                .ReturnsAsync(slot);

            _mockAppointmentRepo.Setup(r => r.Create(It.IsAny<Appointment>()))
                .ReturnsAsync((Appointment a) => a);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(command.PatientName, result.PatientName);
            Assert.Equal(SystemConstants.AppointmentStatus.Scheduled, result.Status);
        }

        [Fact]
        public async Task Handle_WithReservedSlot_ThrowsException()
        {
            // Arrange
            var command = new BookAppointmentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "John Doe"
            );

            var slot = new Slot
            {
                Id = command.SlotId,
                IsReserved = true
            };

            _mockSlotRepo.Setup(r => r.GetSlotById(command.SlotId))
                .ReturnsAsync(slot);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }
    }
}
