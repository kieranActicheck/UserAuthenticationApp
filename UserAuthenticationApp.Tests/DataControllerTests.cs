using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserAuthenticationApp.Controllers;
using UserAuthenticationApp.Data;
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
        private readonly TestLogContext _logContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataControllerTests"/> class.
        /// </summary>
        public DataControllerTests()
        {
            _loggerMock = new Mock<ILogger<DataController>>();
            _logContext = new TestLogContext();
            _controller = new DataController(_loggerMock.Object, _logContext);
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
            var returnValue = okResult.Value;
            Assert.NotNull(returnValue);

            // Use reflection to access properties of the anonymous type
            var timestampProperty = returnValue.GetType().GetProperty("Timestamp");
            var minResponseTimeProperty = returnValue.GetType().GetProperty("MinResponseTime");
            var maxResponseTimeProperty = returnValue.GetType().GetProperty("MaxResponseTime");

            Assert.NotNull(timestampProperty);
            Assert.NotNull(minResponseTimeProperty);
            Assert.NotNull(maxResponseTimeProperty);

            Assert.Equal(logData.Timestamp, timestampProperty.GetValue(returnValue));
            Assert.Equal(1.23, minResponseTimeProperty.GetValue(returnValue));
            Assert.Equal(1.23, maxResponseTimeProperty.GetValue(returnValue));
        }

        /// <summary>
        /// Tests that the <see cref="DataController.Receive"/> method returns a <see cref="BadRequestObjectResult"/> when invalid log data is provided.
        /// </summary>
        [Theory]
        [InlineData(null, 1.23, "SamplePayload")] // Null timestamp
        [InlineData("0001-01-01T00:00:00Z", 0, "SamplePayload")] // Min timestamp and zero response time
        [InlineData("2023-01-01T00:00:00Z", 1.23, "")] // Empty payload
        public void Receive_InvalidLogData_ReturnsBadRequest(string timestamp, double responseTime, string payload)
        {
            // Arrange
            var logData = new LogData
            {
                Timestamp = timestamp == null ? DateTime.MinValue : DateTime.Parse(timestamp),
                ResponseTime = responseTime,
                Payload = payload
            };

            // Act
            var result = _controller.Receive(logData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
