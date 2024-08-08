using Moq;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Services;
using Xunit;
using Xunit.Abstractions;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// Unit tests for the MsgCoordinator class.
    /// </summary>
    public class MsgCoordinatorTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<MsgCoordinator> _mockCoordinator;

        public MsgCoordinatorTests(ITestOutputHelper output)
        {
            _output = output;
            _mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
        }

        private void VerifyBandData(string message, BandData expectedBandData)
        {
            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.Is<BandData>(b =>
                b.DeviceId == expectedBandData.DeviceId &&
                b.Status == expectedBandData.Status &&
                b.Temperature == expectedBandData.Temperature &&
                b.Presence == expectedBandData.Presence &&
                b.Keys == expectedBandData.Keys &&
                b.MovementData == expectedBandData.MovementData &&
                b.AccelX == expectedBandData.AccelX &&
                b.AccelY == expectedBandData.AccelY &&
                b.AccelZ == expectedBandData.AccelZ &&
                b.Voltage == expectedBandData.Voltage &&
                b.Link == expectedBandData.Link &&
                b.Link2 == expectedBandData.Link2 &&
                b.Fallmode == expectedBandData.Fallmode &&
                b.Count == expectedBandData.Count &&
                b.Passcode == expectedBandData.Passcode
            )), Times.Once);
        }

        /// <summary>
        /// Tests that a valid message populates BandData correctly.
        /// </summary>
        [Fact]
        public void ProcessRequest_ValidMessage_PopulatesBandData()
        {
            // Arrange
            string message = "|BDSTAT:B4994C3317DF,316B6A00005508E718804F6B555D8233||";
            var expectedBandData = new BandData
            {
                DeviceId = "B4994C3317DF",
                Status = 49,
                Temperature = 107,
                Presence = 106,
                Keys = 0,
                MovementData = 85,
                AccelX = 8,
                AccelY = 231,
                AccelZ = 24,
                Voltage = 128,
                Link = 79,
                Link2 = 107,
                Fallmode = 85,
                Count = 93,
                Passcode = 33331
            };

            // Act & Assert
            VerifyBandData(message, expectedBandData);
            _output.WriteLine("Processed valid message and verified BandData population.");
        }

        /// <summary>
        /// Tests that another valid message populates BandData correctly.
        /// </summary>
        [Fact]
        public void ProcessRequest_AnotherValidMessage_PopulatesBandData()
        {
            // Arrange
            string message = "|BDSTAT:AABBCCDDEEFF,1234B6A00005508E718804F6B555D8233||";
            var expectedBandData = new BandData
            {
                DeviceId = "AABBCCDDEEFF",
                Status = 18,
                Temperature = 52,
                Presence = 182,
                Keys = 160,
                MovementData = 5,
                AccelX = 80,
                AccelY = 142,
                AccelZ = 113,
                Voltage = 136,
                Link = 4,
                Link2 = 246,
                Fallmode = 181,
                Count = 85,
                Passcode = 55331
            };

            // Act & Assert
            VerifyBandData(message, expectedBandData);
            _output.WriteLine("Processed another valid message and verified BandData population.");
        }

        /// <summary>
        /// Tests that an invalid message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotPopulateBandData()
        {
            // Arrange
            string message = "|BDSTAT:INVALIDDATA,316B6A00005508E718804F6B555D8233|INVALID|";

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed invalid message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that an empty message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_EmptyMessage_DoesNotPopulateBandData()
        {
            // Arrange
            string message = string.Empty;

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed empty message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a null message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_NullMessage_DoesNotPopulateBandData()
        {
            // Arrange
            string? message = null;

            // Act
            _mockCoordinator.Object.ProcessRequest(message!);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed null message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with an invalid key does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidKey_DoesNotPopulateBandData()
        {
            // Arrange
            string message = "|INVALIDKEY:B4994C3317DF,316B6A00005508E718804F6B555D8233||";

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed message with invalid key and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with an invalid value does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidValue_DoesNotPopulateBandData()
        {
            // Arrange
            string message = "|BDSTAT:INVALIDVALUE,316B6A00005508E718804F6B555D8233||";

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed message with invalid value and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with invalid sub-parts does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidSubParts_DoesNotPopulateBandData()
        {
            // Arrange
            string message = "|BDSTAT:INVALIDDATA,INVALIDSUBPARTS||";

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed message with invalid sub-parts and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that an invalid message does not call SendBandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotCallSendBandData()
        {
            // Arrange
            string message = "|INVALID:DATA||";

            // Act
            _mockCoordinator.Object.ProcessRequest(message);

            // Assert
            _mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
            _output.WriteLine("Processed invalid message and verified SendBandData was not called.");
        }
    }
}
