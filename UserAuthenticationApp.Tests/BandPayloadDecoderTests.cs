using System.Text;
using UserAuthenticationApp.Services;
using Xunit;
using Xunit.Abstractions;

namespace UserAuthenticationApp.Tests
{
    public class BandPayloadDecoderTests
    {
        private readonly ITestOutputHelper _output;
        private readonly BandPayloadDecoder _decoder;

        public BandPayloadDecoderTests(ITestOutputHelper output)
        {
            _output = output;
            _decoder = new BandPayloadDecoder(_output);
        }

        [Fact]
        public void DecodePayload_ValidPayload_SetsPropertiesCorrectly()
        {
            // Arrange
            string payload = "BDSTAT:Part1|Part2|Part3|Part4|Part5|Part6|Part7|BDENGI:SomeValue|BDBOOT:AnotherValue";

            // Act
            _decoder.DecodePayload(payload);

            // Assert
            Assert.Equal("Part1", _decoder.Part1);
            Assert.Equal("Part2", _decoder.Part2);
            Assert.Equal("Part3", _decoder.Part3);
            Assert.Equal("Part4", _decoder.Part4);
            Assert.Equal("Part5", _decoder.Part5);
            Assert.Equal("Part6", _decoder.Part6);
            Assert.Equal("Part7", _decoder.Part7);
            Assert.Equal("SomeValue", _decoder.BDENGI);
            Assert.Equal("AnotherValue", _decoder.BDBOOT);
        }

        [Fact]
        public void DecodePayload_MissingBDSTAT_DoesNotSetParts()
        {
            // Arrange
            string payload = "BDENGI:SomeValue|BDBOOT:AnotherValue";

            // Act
            _decoder.DecodePayload(payload);

            // Assert
            Assert.Null(_decoder.Part1);
            Assert.Null(_decoder.Part2);
            Assert.Null(_decoder.Part3);
            Assert.Null(_decoder.Part4);
            Assert.Null(_decoder.Part5);
            Assert.Null(_decoder.Part6);
            Assert.Null(_decoder.Part7);
            Assert.Equal("SomeValue", _decoder.BDENGI);
            Assert.Equal("AnotherValue", _decoder.BDBOOT);
        }

        [Fact]
        public void DecodePayload_IncompleteBDSTAT_SetsPartsToEmpty()
        {
            // Arrange
            string payload = "BDSTAT:Part1|Part2|Part3|Part4|Part5|Part6|BDENGI:SomeValue|BDBOOT:AnotherValue";

            // Act
            _decoder.DecodePayload(payload);

            // Assert
            Assert.Equal("Part1", _decoder.Part1);
            Assert.Equal("Part2", _decoder.Part2);
            Assert.Equal("Part3", _decoder.Part3);
            Assert.Equal("Part4", _decoder.Part4);
            Assert.Equal("Part5", _decoder.Part5);
            Assert.Equal("Part6", _decoder.Part6);
            Assert.Equal(string.Empty, _decoder.Part7);
            Assert.Equal("SomeValue", _decoder.BDENGI);
            Assert.Equal("AnotherValue", _decoder.BDBOOT);
        }

        [Fact]
        public void DecodePayload_EmptyPayload_ThrowsArgumentException()
        {
            // Arrange
            string payload = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _decoder.DecodePayload(payload));
        }

        [Fact]
        public void DecodePayload_NullPayload_ThrowsArgumentException()
        {
            // Arrange
            string payload = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _decoder.DecodePayload(payload));
        }

        [Fact]
        public void DecodePayload_UnknownKey_IgnoresUnknownKey()
        {
            // Arrange
            string payload = "UNKNOWN:Value|BDENGI:SomeValue|BDBOOT:AnotherValue";

            // Act
            _decoder.DecodePayload(payload);

            // Assert
            Assert.Null(_decoder.Part1);
            Assert.Null(_decoder.Part2);
            Assert.Null(_decoder.Part3);
            Assert.Null(_decoder.Part4);
            Assert.Null(_decoder.Part5);
            Assert.Null(_decoder.Part6);
            Assert.Null(_decoder.Part7);
            Assert.Equal("SomeValue", _decoder.BDENGI);
            Assert.Equal("AnotherValue", _decoder.BDBOOT);
        }
    }
}
