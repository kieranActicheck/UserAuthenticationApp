using System;
using System.Globalization;
using System.Linq;
using UserAuthenticationApp.Data;

namespace UserAuthenticationApp.Services
{
    /// <summary>
    /// Coordinates the processing of incoming messages.
    /// </summary>
    public class MsgCoordinator
    {
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
            // Example message: |BDSTAT:B4994C3317DF,316B6A00005508E718804F6B555D8233||
            var keyValue = message.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length == 2 && keyValue[0] == "BDSTAT")
            {
                var values = keyValue[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2 && IsValidSubParts(values))
                {
                    var bandData = new BandData
                    {
                        DeviceId = "1234567890", // Fake DeviceId
                        DateOfPacket = DateTime.Now, // Current DateTime
                    };

                    // Ensure the values[0] is long enough to avoid ArgumentOutOfRangeException
                    if (values[0].Length >= 28)
                    {
                        bandData.Key1 = int.Parse(values[0].Substring(0, 2), NumberStyles.HexNumber);
                        bandData.Key2 = int.Parse(values[0].Substring(2, 2), NumberStyles.HexNumber);
                        bandData.BlueToothLink = int.Parse(values[0].Substring(4, 2), NumberStyles.HexNumber);
                        bandData.Temperature = int.Parse(values[0].Substring(6, 2), NumberStyles.HexNumber) / 10.0;
                        bandData.Presence = int.Parse(values[0].Substring(8, 2), NumberStyles.HexNumber);
                        bandData.AccelX = int.Parse(values[0].Substring(10, 2), NumberStyles.HexNumber);
                        bandData.AccelY = int.Parse(values[0].Substring(12, 2), NumberStyles.HexNumber);
                        bandData.AccelZ = int.Parse(values[0].Substring(14, 2), NumberStyles.HexNumber);
                        bandData.MovementData = int.Parse(values[0].Substring(16, 2), NumberStyles.HexNumber);
                        bandData.BatteryVoltage = int.Parse(values[0].Substring(18, 2), NumberStyles.HexNumber) / 10.0;
                        bandData.Link = int.Parse(values[0].Substring(20, 2), NumberStyles.HexNumber);
                        bandData.Link2 = int.Parse(values[0].Substring(22, 2), NumberStyles.HexNumber);
                        bandData.Count = int.Parse(values[0].Substring(24, 2), NumberStyles.HexNumber);
                        bandData.ISMRadioLink = int.Parse(values[0].Substring(26, 2), NumberStyles.HexNumber);
                    }

                    // Ensure the values[1] is long enough to avoid IndexOutOfRangeException
                    if (values[1].Length >= 2)
                    {
                        bandData.OnWrist = values[1][0] == '1';
                        bandData.MovementTrigger = values[1][1] == '1';
                    }

                    // Log the bandData for debugging
                    Console.WriteLine($"DeviceId: {bandData.DeviceId}, Key1: {bandData.Key1}, Key2: {bandData.Key2}, BlueToothLink: {bandData.BlueToothLink}, Temperature: {bandData.Temperature}");

                    // Send the BandData object to another application
                    SendBandData(bandData);
                }
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
