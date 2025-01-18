using DoctorAppointment.DoctorAvailability.Models;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.DoctorAvailability.Services;
using DoctorAppointment.Shared.Constants;
using DoctorAppointment.Shared.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Tests.DoctorAvailability
{
    public class SlotServiceTests
    {
        private readonly Mock<ISlotRepository> _mockRepository;
        private readonly SlotService _service;

        public SlotServiceTests()
        {
            _mockRepository = new Mock<ISlotRepository>();
            _service = new SlotService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllSlots_ReturnsAllSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new() { Id = Guid.NewGuid(), Time = DateTime.Now, DoctorId = SystemConstants.DefaultDoctorId },
                new() { Id = Guid.NewGuid(), Time = DateTime.Now.AddHours(1), DoctorId = SystemConstants.DefaultDoctorId }
            };

            _mockRepository.Setup(r => r.GetAllSlots())
                .ReturnsAsync(slots);

            // Act
            var result = await _service.GetAllSlots();

            // Assert
            Assert.Equal(slots.Count, result.Count());
        }

        [Fact]
        public async Task AddSlot_CreatesNewSlot()
        {
            // Arrange
            var slotDto = new SlotDto
            {
                Time = DateTime.Now,
                Cost = 100m
            };

            _mockRepository.Setup(r => r.AddSlot(It.IsAny<Slot>()))
                .ReturnsAsync((Slot s) => s);

            // Act
            var result = await _service.AddSlot(slotDto);

            // Assert
            Assert.Equal(SystemConstants.DefaultDoctorId, result.DoctorId);
            Assert.Equal(SystemConstants.DefaultDoctorName, result.DoctorName);
            Assert.False(result.IsReserved);
        }
    }
}
