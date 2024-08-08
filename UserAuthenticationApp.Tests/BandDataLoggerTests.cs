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
                Keys = 1,
                Link = 3,
                Temperature = 36,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                Voltage = 3,
                Link2 = 6,
                Count = 7,
                Fallmode = 8,
                Passcode = 1234,
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
            Assert.Contains("Keys: 1", testLogger.LoggedMessages);
            Assert.Contains("Link: 3", testLogger.LoggedMessages);
            Assert.Contains("Temperature: 36", testLogger.LoggedMessages);
            Assert.Contains("Presence: 1", testLogger.LoggedMessages);
            Assert.Contains("AccelX: 1", testLogger.LoggedMessages);
            Assert.Contains("AccelY: 2", testLogger.LoggedMessages);
            Assert.Contains("AccelZ: 3", testLogger.LoggedMessages);
            Assert.Contains("MovementData: 4", testLogger.LoggedMessages);
            Assert.Contains("Voltage: 3", testLogger.LoggedMessages);
            Assert.Contains("Link2: 6", testLogger.LoggedMessages);
            Assert.Contains("Count: 7", testLogger.LoggedMessages);
            Assert.Contains("Fallmode: 8", testLogger.LoggedMessages);
            Assert.Contains("Passcode: 1234", testLogger.LoggedMessages);
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
            Assert.Contains("Keys: 0", testLogger.LoggedMessages);
            Assert.Contains("Link: 0", testLogger.LoggedMessages);
            Assert.Contains("Temperature: 0", testLogger.LoggedMessages);
            Assert.Contains("Presence: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelX: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelY: 0", testLogger.LoggedMessages);
            Assert.Contains("AccelZ: 0", testLogger.LoggedMessages);
            Assert.Contains("MovementData: 0", testLogger.LoggedMessages);
            Assert.Contains("Voltage: 0", testLogger.LoggedMessages);
            Assert.Contains("Link2: 0", testLogger.LoggedMessages);
            Assert.Contains("Count: 0", testLogger.LoggedMessages);
            Assert.Contains("Fallmode: 0", testLogger.LoggedMessages);
            Assert.Contains("Passcode: 0", testLogger.LoggedMessages);
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
                Keys = 1,
                Link = 3,
                Temperature = 36,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                Voltage = 3,
                Link2 = 6,
                Count = 7,
                Fallmode = 8,
                Passcode = 1234,
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
                Keys = 1,
                Link = 3,
                Temperature = 36,
                Presence = 1,
                AccelX = 1,
                AccelY = 2,
                AccelZ = 3,
                MovementData = 4,
                Voltage = 3,
                Link2 = 6,
                Count = 7,
                Fallmode = 8,
                Passcode = 1234,
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
