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

        public MsgCoordinatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Tests that a valid message populates BandData correctly.
        /// </summary>
        [Fact]
        public void ProcessRequest_ValidMessage_PopulatesBandData()
        {
            // Arrange
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            string message = "|BDSTAT:B4994C3317DF,316B6A00005508E718804F6B555D8233||";

            // Act
            mockCoordinator.Object.ProcessRequest(message);

            // Assert
            mockCoordinator.Verify(m => m.SendBandData(It.Is<BandData>(b =>
                b.DeviceId == "B4994C3317DF" &&
                b.Status == 49 &&
                b.Temperature == 107 &&
                b.Presence == 106 &&
                b.Keys == 0 &&
                b.MovementData == 85 &&
                b.AccelX == 8 &&
                b.AccelY == 231 &&
                b.AccelZ == 24 &&
                b.Voltage == 128 &&
                b.Link == 79 &&
                b.Link2 == 107 &&
                b.Fallmode == 85 &&
                b.Count == 93 &&
                b.Passcode == 33331
            )), Times.Once);

            _output.WriteLine("Processed valid message and verified BandData population.");
        }

        /// <summary>
        /// Tests that another valid message populates BandData correctly.
        /// </summary>
        [Fact]
        public void ProcessRequest_AnotherValidMessage_PopulatesBandData()
        {
            // Arrange
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            string message = "|BDSTAT:AABBCCDDEEFF,1234B6A00005508E718804F6B555D8233||";

            // Act
            mockCoordinator.Object.ProcessRequest(message);

            // Assert
            mockCoordinator.Verify(m => m.SendBandData(It.Is<BandData>(b =>
                b.DeviceId == "AABBCCDDEEFF" &&
                b.Status == 18 &&
                b.Temperature == 52 &&
                b.Presence == 182 &&
                b.Keys == 160 &&
                b.MovementData == 5 &&
                b.AccelX == 80 &&
                b.AccelY == 142 &&
                b.AccelZ == 113 &&
                b.Voltage == 136 &&
                b.Link == 4 &&
                b.Link2 == 246 &&
                b.Fallmode == 181 &&
                b.Count == 85 &&
                b.Passcode == 55331
            )), Times.Once);

            _output.WriteLine("Processed another valid message and verified BandData population.");
        }

        /// <summary>
        /// Tests that an invalid message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = "|BDSTAT:INVALIDDATA,316B6A00005508E718804F6B555D8233|INVALID|";

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed invalid message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that an empty message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_EmptyMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = string.Empty;

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed empty message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a null message does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_NullMessage_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            string? message = null;

            // Act
            msgCoordinator.ProcessRequest(message!);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message!);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed null message and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with an invalid key does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidKey_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = "|INVALIDKEY:B4994C3317DF,316B6A00005508E718804F6B555D8233||";

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed message with invalid key and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with an invalid value does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidValue_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = "|BDSTAT:INVALIDVALUE,316B6A00005508E718804F6B555D8233||";

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed message with invalid value and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that a message with invalid sub-parts does not populate BandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidSubParts_DoesNotPopulateBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = "|BDSTAT:INVALIDDATA,INVALIDSUBPARTS||";

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed message with invalid sub-parts and verified BandData was not populated.");
        }

        /// <summary>
        /// Tests that an invalid message does not call SendBandData.
        /// </summary>
        [Fact]
        public void ProcessRequest_InvalidMessage_DoesNotCallSendBandData()
        {
            // Arrange
            var msgCoordinator = new MsgCoordinator(_output);
            var message = "|INVALID:DATA||";

            // Act
            msgCoordinator.ProcessRequest(message);

            // Assert
            var mockCoordinator = new Mock<MsgCoordinator>(_output) { CallBase = true };
            mockCoordinator.Object.ProcessRequest(message);

            mockCoordinator.Verify(m => m.SendBandData(It.IsAny<BandData>()), Times.Never);

            _output.WriteLine("Processed invalid message and verified SendBandData was not called.");
        }
    }
}
