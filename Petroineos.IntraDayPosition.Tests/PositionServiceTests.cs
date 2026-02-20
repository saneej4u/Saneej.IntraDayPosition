using Moq;
using Petroineos.IntraDayPosition.Core.Service;
using Services;

namespace Petroineos.IntraDayPosition.Tests
{
    public class PositionServiceTests
    {
        private Mock<IPowerService> _mockPowerService;
        private PositionService _sut; // System Under Test

        [SetUp]
        public void Setup()
        {
            _mockPowerService = new Mock<IPowerService>();
            _sut = new PositionService(_mockPowerService.Object, null, null);
        }

        [Test]
        public void Aggregate_ShouldSumVolumesCorrectly_For_Period_1()
        {
            // Arrange
            var trade1 = CreateTrade(1, 100);  // Trade 1: 100 volume for Period 1
            var trade2 = CreateTrade(1, 50); // Trade 2: 50 volume for Period 1

            var trades = new List<PowerTrade> { trade1, trade2 };

            // Act
            var result = _sut.Aggregate(trades).ToList();

            // Assert:
            var positionAt2300 = result.FirstOrDefault(p => p.LocalTime == "23:00");

            Assert.IsNotNull(positionAt2300);
            Assert.AreEqual(150, positionAt2300.Volume);
        }

        [Test]
        public void Aggregate_ShouldHandleNegativeVolumes()
        {
            // Arrange
            var trade1 = CreateTrade(12, 100);
            var trade2 = CreateTrade(12, -20);  // Trade 2: negative volume

            var trades = new List<PowerTrade> { trade1, trade2 };

            // Act
            var result = _sut.Aggregate(trades).ToList();

            // Assert: 100 + (-20) = 80 for period 10
            var positionAt1000 = result.FirstOrDefault(p => p.LocalTime == "10:00");

            Assert.IsNotNull(positionAt1000);
            Assert.AreEqual(80, positionAt1000.Volume);
        }

        // Helper to create mock trade data without needing the actual DLL logic
        private PowerTrade CreateTrade(int periodNum, double volume)
        {
            var trade = PowerTrade.Create(DateTime.Now, 24);
            trade.Periods[periodNum - 1].Volume = volume;
            return trade;
        }
    }
}