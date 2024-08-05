namespace UserAuthenticationApp.Data
{
    /// <summary>
    /// Represents the data model for a band.
    /// </summary>
    public partial class BandData
    {
        private bool _needConfig;
        private string _passCode;
        private string _bandEncKey;
        private bool _needPassCode;
        private bool _needKey;
        private int FallMode;
        public bool shakeAfterBuzz;
        public bool panicButton;
        public bool smokeAlarm;
        public bool heatAlarm;
        public bool appBandStatusRequest = false;
        public bool isPanicButton = false;
        public bool bandWatchdog;
        public string CosmosData;
        public Dictionary<string, string> Tags = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets a value indicating whether the band needs configuration.
        /// </summary>
        public bool NeedConfig
        {
            get { return _needConfig; }
            set { _needConfig = value; }
        }

        /// <summary>
        /// Gets or sets the pass code for the band.
        /// </summary>
        public string PassCode
        {
            get { return _passCode; }
            set { _passCode = value; }
        }

        /// <summary>
        /// Gets or sets the encryption key for the band.
        /// </summary>
        public string BandEncKey
        {
            get { return _bandEncKey; }
            set { _bandEncKey = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a pass code is needed.
        /// </summary>
        public bool NeedPassCode
        {
            get { return _needPassCode; }
            set { _needPassCode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a key is needed.
        /// </summary>
        public bool NeedKey
        {
            get { return _needKey; }
            set { _needKey = value; }
        }

        /// <summary>
        /// Gets or sets the fall mode property.
        /// </summary>
        public int FallModeProperty
        {
            get { return FallMode; }
            set { FallMode = value; }
        }
    }
}
