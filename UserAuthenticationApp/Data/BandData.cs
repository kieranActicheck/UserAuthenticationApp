namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents the data model for a band.
    /// </summary>
    public partial class BandData
    {
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the device ID.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty; // Initialise with a default value

        public int Status { get; set; }
        public int Temperature { get; set; }
        public int Presence { get; set; }
        public int Keys { get; set; }
        public int MovementData { get; set; }
        public int AccelX { get; set; }
        public int AccelY { get; set; }
        public int AccelZ { get; set; }
        public int Voltage { get; set; }
        public int Link { get; set; }
        public int Link2 { get; set; }
        public int Fallmode { get; set; }
        public int Count { get; set; }
        public int Passcode { get; set; }
        public bool OnWrist { get; set; }
        public DateTime DateOfPacket { get; set; }
        public bool MovementTrigger { get; set; }
    }
}
