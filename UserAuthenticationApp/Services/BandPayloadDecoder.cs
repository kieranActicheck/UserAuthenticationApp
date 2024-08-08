using System;
using Xunit;
using Xunit.Abstractions;

namespace UserAuthenticationApp
{
    public class BandPayloadDecoder
    {
        private readonly ITestOutputHelper _output;

        public BandPayloadDecoder(ITestOutputHelper output)
        {
            _output = output;
        }

        // Properties
        public string BDSTAT { get; set; } = null;
        public string BDENGI { get; set; } = null;
        public string BDBOOT { get; set; } = null;
        public string BDSHCK { get; set; } = null;
        public string BSENCR { get; set; } = null;
        public string BSLINK { get; set; } = null;
        public string BSBOOT { get; set; } = null;
        public string BSSTAT { get; set; } = null;
        public string BSLOCA { get; set; } = null;
        public string BSENGI { get; set; } = null;
        public string BSCELL { get; set; } = null;
        public string PHONE { get; set; } = null;
        public string PHONENUMBERCHECK { get; set; } = null;
        public string Part1 { get; set; } = null;
        public string Part2 { get; set; } = null;
        public string Part3 { get; set; } = null;
        public string Part4 { get; set; } = null;
        public string Part5 { get; set; } = null;
        public string Part6 { get; set; } = null;
        public string Part7 { get; set; } = null;

        public void DecodePayload(string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                throw new ArgumentException("Payload cannot be null or empty", nameof(payload));
            }

            _output.WriteLine($"Decoding payload: {payload}");

            var parts = payload.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                _output.WriteLine($"Processing part: {part}");
                var keyValue = part.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (keyValue.Length == 2)
                {
                    var key = keyValue[0];
                    var value = keyValue[1];

                    _output.WriteLine($"Key: {key}, Value: {value}");

                    if (key == "BDSTAT")
                    {
                        // BDSTAT contains multiple parts; split the value
                        var subParts = value.Split('|', StringSplitOptions.None);
                        if (subParts.Length == 7)
                        {
                            Part1 = subParts[0];
                            Part2 = subParts[1];
                            Part3 = subParts[2];
                            Part4 = subParts[3];
                            Part5 = subParts[4];
                            Part6 = subParts[5];
                            Part7 = subParts[6];
                        }
                        else
                        {
                            _output.WriteLine("Invalid BDSTAT sub-parts count");
                            Part1 = Part2 = Part3 = Part4 = Part5 = Part6 = Part7 = null;
                        }
                        _output.WriteLine($"Part1: {Part1}");
                        _output.WriteLine($"Part2: {Part2}");
                        _output.WriteLine($"Part3: {Part3}");
                        _output.WriteLine($"Part4: {Part4}");
                        _output.WriteLine($"Part5: {Part5}");
                        _output.WriteLine($"Part6: {Part6}");
                        _output.WriteLine($"Part7: {Part7}");
                    }
                    else
                    {
                        // Handle other keys normally
                        switch (key)
                        {
                            case "BDENGI":
                                BDENGI = value;
                                _output.WriteLine($"BDENGI: {BDENGI}");
                                break;

                            case "BDBOOT":
                                BDBOOT = value;
                                _output.WriteLine($"BDBOOT: {BDBOOT}");
                                break;

                            // Add cases for other keys as necessary...
                            default:
                                _output.WriteLine($"Unknown key: {key}");
                                break;
                        }
                    }
                }
                else
                {
                    _output.WriteLine($"Invalid part: {part}");
                }
            }
        }
    }
}
