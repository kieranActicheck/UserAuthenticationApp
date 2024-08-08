using System;
using System.Globalization;
using System.Linq;
using UserAuthenticationApp.Data;
using Xunit.Abstractions;

namespace UserAuthenticationApp.Services
{
    /// <summary>
    /// Coordinates the processing of incoming messages.
    /// </summary>
    public class MsgCoordinator
    {
        private readonly ITestOutputHelper _output;

        public MsgCoordinator(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        public void ProcessRequest(string message)
        {
            if (string.IsNullOrEmpty(message) || !message.Contains("||"))
            {
                return;
            }

            var parts = GetBaseBandMac(message);
            foreach (var part in parts)
            {
                Populate(part);
            }
        }

        /// <summary>
        /// Splits the message into parts based on the '|' delimiter.
        /// </summary>
        /// <param name="message">The message to split.</param>
        /// <returns>An array of message parts.</returns>
        private string[] GetBaseBandMac(string message)
        {
            // Split the message into parts
            return message.Split('|', StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Populates a BandData object from the message part.
        /// </summary>
        /// <param name="message">The message part to process.</param>
        private void Populate(string message)
        {
            _output.WriteLine($"Processing message part: {message}");
            var keyValue = message.Split(':', StringSplitOptions.RemoveEmptyEntries);

            if (keyValue.Length == 2 && keyValue[0] == "BDSTAT")
            {
                var values = keyValue[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 2 && IsValidSubParts(values))
                {
                    var bandData = new BandData
                    {
                        DeviceId = values[0].Substring(0, 12), // Extract DeviceId from the message
                        DateOfPacket = DateTime.Now,
                    };

                    try
                    {
                        // Check if the length of values[1] is at least 30 characters
                        if (values[1].Length >= 30)
                        {
                            bandData.Status = int.Parse(values[1].Substring(0, 2), NumberStyles.HexNumber);
                            bandData.Temperature = int.Parse(values[1].Substring(2, 2), NumberStyles.HexNumber);
                            bandData.Presence = int.Parse(values[1].Substring(4, 2), NumberStyles.HexNumber);
                            bandData.Keys = int.Parse(values[1].Substring(6, 2), NumberStyles.HexNumber);
                            bandData.MovementData = int.Parse(values[1].Substring(8, 4), NumberStyles.HexNumber);
                            bandData.AccelX = int.Parse(values[1].Substring(12, 2), NumberStyles.HexNumber);
                            bandData.AccelY = int.Parse(values[1].Substring(14, 2), NumberStyles.HexNumber);
                            bandData.AccelZ = int.Parse(values[1].Substring(16, 2), NumberStyles.HexNumber);
                            bandData.Voltage = int.Parse(values[1].Substring(18, 2), NumberStyles.HexNumber);
                            bandData.Link = int.Parse(values[1].Substring(20, 2), NumberStyles.HexNumber);
                            bandData.Link2 = int.Parse(values[1].Substring(22, 2), NumberStyles.HexNumber);
                            bandData.Fallmode = int.Parse(values[1].Substring(24, 2), NumberStyles.HexNumber);
                            bandData.Count = int.Parse(values[1].Substring(26, 2), NumberStyles.HexNumber);
                            bandData.Passcode = int.Parse(values[1].Substring(28, 4), NumberStyles.HexNumber);

                            // Log the parsed values
                            _output.WriteLine($"Status: {bandData.Status}, Temperature: {bandData.Temperature}, Presence: {bandData.Presence}, Keys: {bandData.Keys}, MovementData: {bandData.MovementData}, AccelX: {bandData.AccelX}, AccelY: {bandData.AccelY}, AccelZ: {bandData.AccelZ}, Voltage: {bandData.Voltage}, Link: {bandData.Link}, Link2: {bandData.Link2}, Fallmode: {bandData.Fallmode}, Count: {bandData.Count}, Passcode: {bandData.Passcode}");
                        }
                        else
                        {
                            // Log a message if the length is less than expected
                            _output.WriteLine($"Message part length is less than expected: {values[1].Length}");
                        }

                        _output.WriteLine($"Parsed BandData: {bandData.DeviceId}, {bandData.Status}, {bandData.Temperature}, {bandData.Presence}, {bandData.Keys}, {bandData.MovementData}, {bandData.AccelX}, {bandData.AccelY}, {bandData.AccelZ}, {bandData.Voltage}, {bandData.Link}, {bandData.Link2}, {bandData.Fallmode}, {bandData.Count}, {bandData.Passcode}");
                        SendBandData(bandData);
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine($"Error parsing message: {ex.Message}");
                    }
                }
                else
                {
                    _output.WriteLine("Invalid sub-parts in the message.");
                }
            }
            else
            {
                _output.WriteLine("Invalid message format.");
            }
        }

        /// <summary>
        /// Validates the sub-parts of the message.
        /// </summary>
        /// <param name="values">The sub-parts to validate.</param>
        /// <returns>True if the sub-parts are valid, otherwise false.</returns>
        private bool IsValidSubParts(string[] values)
        {
            // Add your validation logic here
            // For example, check if values contain "INVALIDDATA" or "INVALIDSUBPARTS"
            return !values.Any(v => v == "INVALIDDATA" || v == "INVALIDSUBPARTS" || v == "INVALIDVALUE");
        }

        /// <summary>
        /// Sends the BandData object to another application.
        /// </summary>
        /// <param name="bandData">The BandData object to send.</param>
        public virtual void SendBandData(BandData bandData)
        {
            // Implement the logic to send the BandData object to another application
            // HTTP request? A message queue? etc.
        }
    }
}
