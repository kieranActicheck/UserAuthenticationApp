namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents the data model for a band.
    /// </summary>
    public partial class BandData
    {
        public int ID { get; set; }
        public string DeviceId { get; set; }
        public int Key1 { get; set; }
        public int Key2 { get; set; }
        public int BlueToothLink { get; set; }
        public double Temperature { get; set; }
        public int Presence { get; set; }
        public int AccelX { get; set; }
        public int AccelY { get; set; }
        public int AccelZ { get; set; }
        public int MovementData { get; set; }
        public double BatteryVoltage { get; set; }
        public int Link { get; set; }
        public int Link2 { get; set; }
        public int Count { get; set; }
        public int ISMRadioLink { get; set; }
        public bool OnWrist { get; set; }
        public DateTime DateOfPacket { get; set; }
        public bool MovementTrigger { get; set; }
    }
}
