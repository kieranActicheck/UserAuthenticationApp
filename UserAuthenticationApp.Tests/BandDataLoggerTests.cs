using System;
using System.Linq;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Services;
using Xunit;

namespace UserAuthenticationApp.Tests
{
    public class BandDataLoggerTests
    {
        /// <summary>
        /// Tests that the LogBandData method logs all properties of the BandData instance.
        /// </summary>
        [Fact]
        public void LogBandData_LogsAllProperties()
        {
            // Arrange
            var bandData = new BandData
            {
                DeviceId = "device123",
                DateOfPacket = DateTime.Now,
                Key1 = 1,
                Key2 = 2,
                BlueToothLink = 3,
                Temperature = 36.5,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                BatteryVoltage = 3.7,
                Link = 5,
                Link2 = 6,
                Count = 7,
                ISMRadioLink = 8,
                OnWrist = true,
                MovementTrigger = true
            };

            var testLogger = new TestLogger<BandDataLogger>();
            var bandDataLogger = new BandDataLogger(bandData, testLogger);

            // Act
            bandDataLogger.LogBandData();

            // Assert
            Assert.Contains("DeviceId: device123", testLogger.LoggedMessages);
            Assert.Contains("DateOfPacket:", testLogger.LoggedMessages.FirstOrDefault(msg => msg.Contains("DateOfPacket:")));
            Assert.Contains("Key1: 1", testLogger.LoggedMessages);
            Assert.Contains("Key2: 2", testLogger.LoggedMessages);
            Assert.Contains("BlueToothLink: 3", testLogger.LoggedMessages);
            Assert.Contains("Temperature: 36.5", testLogger.LoggedMessages);
            Assert.Contains("Presence: 1", testLogger.LoggedMessages);
            Assert.Contains("AccelX: 1", testLogger.LoggedMessages);
            Assert.Contains("AccelY: 2", testLogger.LoggedMessages);
            Assert.Contains("AccelZ: 3", testLogger.LoggedMessages);
            Assert.Contains("MovementData: 4", testLogger.LoggedMessages);
            Assert.Contains("BatteryVoltage: 3.7", testLogger.LoggedMessages);
            Assert.Contains("Link: 5", testLogger.LoggedMessages);
            Assert.Contains("Link2: 6", testLogger.LoggedMessages);
            Assert.Contains("Count: 7", testLogger.LoggedMessages);
            Assert.Contains("ISMRadioLink: 8", testLogger.LoggedMessages);
            Assert.Contains("OnWrist: True", testLogger.LoggedMessages);
            Assert.Contains("MovementTrigger: True", testLogger.LoggedMessages);
        }

        /// <summary>
        /// Tests that the LogBandData method handles null BandData gracefully.
        /// </summary>
        [Fact]
        public void LogBandData_NullBandData_DoesNotThrowException()
        {
            // Arrange
            BandData? bandData = null;
            var testLogger = new TestLogger<BandDataLogger>();
            var bandDataLogger = new BandDataLogger(bandData, testLogger);

            // Act & Assert
            var exception = Record.Exception(() => bandDataLogger.LogBandData());
            Assert.Null(exception);
        }

        /// <summary>
        /// Tests that the LogBandData method logs default values of BandData properties.
        /// </summary>
        [Fact]
        public void LogBandData_LogsDefaultValues()
        {
            // Arrange
            var bandData = new BandData();
            var testLogger = new TestLogger<BandDataLogger>();
            var bandDataLogger = new BandDataLogger(bandData, testLogger);

            // Act
            bandDataLogger.LogBandData();

            // Assert
            Assert.Contains("DeviceId: ", testLogger.LoggedMessages);
            Assert.Contains("DateOfPacket: 01/01/0001 00:00:00", testLogger.LoggedMessages); // Updated assertion
            Assert.Contains("Key1: 0", testLogger.LoggedMessages);
            Assert.Contains("Key2: 0", testLogger.LoggedMessages);
            Assert.Contains("BlueToothLink: 0", testLogger.LoggedMessages);
            Assert.Contains("Temperature: 0", testLogger.LoggedMessages);
            Assert.Contains("Presence: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelX: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelY: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelZ: 0", testLogger.LoggedMessages);
            Assert.Contains("MovementData: 0", testLogger.LoggedMessages);
            Assert.Contains("BatteryVoltage: 0", testLogger.LoggedMessages);
            Assert.Contains("Link: 0", testLogger.LoggedMessages);
            Assert.Contains("Link2: 0", testLogger.LoggedMessages);
            Assert.Contains("Count: 0", testLogger.LoggedMessages);
            Assert.Contains("ISMRadioLink: 0", testLogger.LoggedMessages);
            Assert.Contains("OnWrist: False", testLogger.LoggedMessages);
            Assert.Contains("MovementTrigger: False", testLogger.LoggedMessages);
        }


        /// <summary>
        /// Tests that the LogBandData method logs properties in the correct format.
        /// </summary>
        [Fact]
        public void LogBandData_LogsInCorrectFormat()
        {
            // Arrange
            var bandData = new BandData
            {
                DeviceId = "device123",
                DateOfPacket = DateTime.Now,
                Key1 = 1,
                Key2 = 2,
                BlueToothLink = 3,
                Temperature = 36.5,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                BatteryVoltage = 3.7,
                Link = 5,
                Link2 = 6,
                Count = 7,
                ISMRadioLink = 8,
                OnWrist = true,
                MovementTrigger = true
            };

            var testLogger = new TestLogger<BandDataLogger>();
            var bandDataLogger = new BandDataLogger(bandData, testLogger);

            // Act
            bandDataLogger.LogBandData();

            // Assert
            Assert.All(testLogger.LoggedMessages, msg => Assert.Matches(@"^(Logging BandData properties:|\w+: .+)$", msg));
        }

        /// <summary>
        /// Tests that the LogBandData method handles exceptions properly.
        /// </summary>
        [Fact]
        public void LogBandData_HandlesExceptions()
        {
            // Arrange
            var bandData = new BandData
            {
                DeviceId = "device123",
                DateOfPacket = DateTime.Now,
                Key1 = 1,
                Key2 = 2,
                BlueToothLink = 3,
                Temperature = 36.5,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                BatteryVoltage = 3.7,
                Link = 5,
                Link2 = 6,
                Count = 7,
                ISMRadioLink = 8,
                OnWrist = true,
                MovementTrigger = true
            };

            var testLogger = new TestLogger<BandDataLogger>();
            var bandDataLogger = new BandDataLogger(bandData, testLogger);

            // Simulate an exception in the logger
            testLogger.ThrowExceptionOnLog = true;

            // Act & Assert
            var exception = Record.Exception(() => bandDataLogger.LogBandData());
            Assert.Null(exception);
        }
    }
}
