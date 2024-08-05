using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserAuthenticationApp.Controllers;
using Xunit;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="DataController"/> class.
    /// </summary>
    public class DataControllerTests
    {
        private readonly DataController _controller;
        private readonly Mock<ILogger<DataController>> _loggerMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataControllerTests"/> class.
        /// </summary>
        public DataControllerTests()
        {
            _loggerMock = new Mock<ILogger<DataController>>();
            _controller = new DataController(_loggerMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="DataController.Receive"/> method returns an <see cref="OkObjectResult"/> when valid log data is provided.
        /// </summary>
        [Fact]
        public void Receive_ValidLogData_ReturnsOk()
        {
            // Arrange
            var logData = new LogData
            {
                Timestamp = DateTime.UtcNow,
                ResponseTime = 1.23,
                Payload = "SamplePayload"
            };

            // Act
            var result = _controller.Receive(logData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<dynamic>(okResult.Value);
            Assert.Equal(logData.Timestamp, returnValue.Timestamp);
            Assert.Equal(1.23, returnValue.MinResponseTime);
            Assert.Equal(1.23, returnValue.MaxResponseTime);
        }

        /// <summary>
        /// Tests that the <see cref="DataController.Receive"/> method returns a <see cref="BadRequestObjectResult"/> when invalid log data is provided.
        /// </summary>
        [Fact]
        public void Receive_InvalidLogData_ReturnsBadRequest()
        {
            // Arrange
            var logData = new LogData
            {
                Timestamp = DateTime.MinValue,
                ResponseTime = 0,
                Payload = null
            };

            // Act
            var result = _controller.Receive(logData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
