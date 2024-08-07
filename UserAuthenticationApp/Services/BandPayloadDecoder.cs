using System;
using System.Collections.Generic;
using System.IO;

namespace ActiCheckCore
{
    /// <summary>
    /// Decodes the payload string into its respective parts.
    /// </summary>
    public class BandPayloadDecoder
    {
        /// <summary>
        /// Band Status Information.
        /// </summary>
        public string BDSTAT { get; set; }

        /// <summary>
        /// Band Engineering Data.
        /// </summary>
        public string BDENGI { get; set; }

        /// <summary>
        /// Band Boot Information.
        /// </summary>
        public string BDBOOT { get; set; }

        /// <summary>
        /// Band Shock Information.
        /// </summary>
        public string BDSHCK { get; set; }

        /// <summary>
        /// Base Encryption Information.
        /// </summary>
        public string BSENCR { get; set; }

        /// <summary>
        /// Radio Status Link Information.
        /// </summary>
        public string BSLINK { get; set; }

        /// <summary>
        /// Base Boot Information.
        /// </summary>
        public string BSBOOT { get; set; }

        /// <summary>
        /// Base Status Information.
        /// </summary>
        public string BSSTAT { get; set; }

        /// <summary>
        /// Base Location Data.
        /// </summary>
        public string BSLOCA { get; set; }

        /// <summary>
        /// Base Engineering Data.
        /// </summary>
        public string BSENGI { get; set; }

        /// <summary>
        /// Base GSM Cell Modem Information.
        /// </summary>
        public string BSCELL { get; set; }

        /// <summary>
        /// Phone Information.
        /// </summary>
        public string PHONE { get; set; }

        /// <summary>
        /// Phone Number Check Information.
        /// </summary>
        public string PHONENUMBERCHECK { get; set; }

        /// <summary>
        /// Part 1 of the BDSTAT payload.
        /// </summary>
        public string Part1 { get; set; }

        /// <summary>
        /// Part 2 of the BDSTAT payload.
        /// </summary>
        public string Part2 { get; set; }

        /// <summary>
        /// Part 3 of the BDSTAT payload.
        /// </summary>
        public string Part3 { get; set; }

        /// <summary>
        /// Part 4 of the BDSTAT payload.
        /// </summary>
        public string Part4 { get; set; }

        /// <summary>
        /// Part 5 of the BDSTAT payload.
        /// </summary>
        public string Part5 { get; set; }

        /// <summary>
        /// Part 6 of the BDSTAT payload.
        /// </summary>
        public string Part6 { get; set; }

        /// <summary>
        /// Part 7 of the BDSTAT payload.
        /// </summary>
        public string Part7 { get; set; }

        /// <summary>
        /// Decodes the given payload string into its respective parts.
        /// </summary>
        /// <param name="payload">The payload string to decode.</param>
        /// <exception cref="ArgumentException">Thrown when the payload is null or empty.</exception>
        public void DecodePayload(string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                throw new ArgumentException("Payload cannot be null or empty", nameof(payload));
            }

            var parts = payload.Split('|', StringSplitOptions.RemoveEmptyEntries);
            string logDirectory = "Logs";
            string logFileName = $"log{DateTime.Now:yyyyMMdd}.txt";
            string logFilePath = Path.Combine(logDirectory, logFileName);

            // Ensure the Logs directory exists
            Directory.CreateDirectory(logDirectory);

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                foreach (var part in parts)
                {
                    var keyValue = part.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0];
                        var value = keyValue[1];

                        switch (key)
                        {
                            case "BDSTAT":
                                BDSTAT = value;
                                var subParts = value.Split('|', StringSplitOptions.RemoveEmptyEntries);
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
                                writer.WriteLine($"BDSTAT: {BDSTAT}");
                                writer.WriteLine($"Part1: {Part1}");
                                writer.WriteLine($"Part2: {Part2}");
                                writer.WriteLine($"Part3: {Part3}");
                                writer.WriteLine($"Part4: {Part4}");
                                writer.WriteLine($"Part5: {Part5}");
                                writer.WriteLine($"Part6: {Part6}");
                                writer.WriteLine($"Part7: {Part7}");
                                break;
                            case "BDENGI":
                                BDENGI = value;
                                writer.WriteLine($"BDENGI: {BDENGI}");
                                break;
                            case "BDBOOT":
                                BDBOOT = value;
                                writer.WriteLine($"BDBOOT: {BDBOOT}");
                                break;
                            case "BDSHCK":
                                BDSHCK = value;
                                writer.WriteLine($"BDSHCK: {BDSHCK}");
                                break;
                            case "BSENCR":
                                BSENCR = value;
                                writer.WriteLine($"BSENCR: {BSENCR}");
                                break;
                            case "BSLINK":
                                BSLINK = value;
                                writer.WriteLine($"BSLINK: {BSLINK}");
                                break;
                            case "BSBOOT":
                                BSBOOT = value;
                                writer.WriteLine($"BSBOOT: {BSBOOT}");
                                break;
                            case "BSSTAT":
                                BSSTAT = value;
                                writer.WriteLine($"BSSTAT: {BSSTAT}");
                                break;
                            case "BSLOCA":
                                BSLOCA = value;
                                writer.WriteLine($"BSLOCA: {BSLOCA}");
                                break;
                            case "BSENGI":
                                BSENGI = value;
                                writer.WriteLine($"BSENGI: {BSENGI}");
                                break;
                            case "BSCELL":
                                BSCELL = value;
                                writer.WriteLine($"BSCELL: {BSCELL}");
                                break;
                            case "PHONE":
                                PHONE = value;
                                writer.WriteLine($"PHONE: {PHONE}");
                                break;
                            case "SMSNUMBERCHECK":
                                PHONENUMBERCHECK = value;
                                writer.WriteLine($"PHONENUMBERCHECK: {PHONENUMBERCHECK}");
                                break;
                                // Add more cases as needed
                        }
                    }
                }
            }
        }
    }
}
