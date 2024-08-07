using Moq;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.Services;
using Xunit;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// Unit tests for the MsgCoordinator class.
    /// </summary>
    public class MsgCoordinatorTests
    {
        /// <summary>
        /// Tests that a valid message populates BandData correctly.
        /// </summary>
        [Fact]
        public void ProcessRequest_ValidMessage_PopulatesBandData()
        {
            // Arrange
            var mockCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            string message = "|BDSTAT:B4994C3317DF,316B6A00005508E718804F6B555D8233||";

            // Act
            mockCoordinator.Object.ProcessRequest(message);

            // Assert
            mockCoordinator.Verify(m => m.SendBandData(It.Is<BandData>(b =>
                b.DeviceId == "B4994C3317DF" && // Adjusted to match the message
                b.Key1 == 316 && // Adjusted to match the message
                b.Key2 == 180 && // Adjusted to match the message
                b.BlueToothLink == 76 && // Adjusted to match the message
                Math.Abs(b.Temperature - 33.1) < 0.0001 // Adjusted for floating-point precision
            )), Times.Once);
        }

        /// <summary>
        /// Tests that an invalid message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = "|BDSTAT:INVALIDDATA,316B6A00005508E718804F6B555D8233|INVALID|";

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that an empty message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_EmptyMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = string.Empty;

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that a null message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_NullMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            string? message = null;

            // Act
            msgCoordinator.Object.ProcessRequest(message!);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that a message with an invalid key does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidKey_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = "|INVALIDKEY:B4994C3317DF,316B6A00005508E718804F6B555D8233||";

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that a message with an invalid value does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidValue_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = "|BDSTAT:INVALIDVALUE,316B6A00005508E718804F6B555D8233||";

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that a message with invalid sub-parts does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidSubParts_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = "|BDSTAT:INVALIDDATA,INVALIDSUBPARTS||";

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }

        /// <summary>
        /// Tests that an invalid message does not call SendBandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotCallSendBandData()
        {
            // Arrange
            var msgCoordinator = new Mock<MsgCoordinator> { CallBase = true };
            var message = "|INVALID:DATA||";

            // Act
            msgCoordinator.Object.ProcessRequest(message);

            // Assert
            msgCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);
        }
    }
}
