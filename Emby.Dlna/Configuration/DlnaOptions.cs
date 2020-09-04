#nullable enable

namespace Emby.Dlna.Configuration
{
    /// <summary>
    /// The DlnaOptions class contains the user definable parameters for the dlna subsystems.
    /// </summary>
    public class DlnaOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DlnaOptions"/> class.
        /// </summary>
        public DlnaOptions()
        {
            EnablePlayTo = true;
            EnableServer = true;
            ClientDiscoveryIntervalSeconds = 60;
            AliveMessageIntervalSeconds = 1800;
            UDPSendCount = 2;
            UDPSendDelay = 100;
            SSDPTracingFilter = string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value to indicate the status of the dlna playTo subsystem.
        /// </summary>
        public bool EnablePlayTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value to indicate the status of the dlna server subsystem.
        /// </summary>
        public bool EnableServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether detailed dlna server logs are sent to the console/log.
        /// If the setting "Emby.Dlna": "Debug" msut be set in logging.default.json for this property to work.
        /// </summary>
        public bool EnableDebugLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether detailed ssdp logs are sent to the console/log.
        /// If the setting "Emby.Dlna": "Debug" msut be set in logging.default.json for this property to work.
        /// </summary>
        public bool EnableSSDPTracing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an IP address is to be used to filter the detailed ssdp logs that are being sent to the console/log.
        /// If the setting "Emby.Dlna": "Debug" msut be set in logging.default.json for this property to work.
        /// </summary>
        public string SSDPTracingFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether detailed playTo debug logs are sent to the console/log.
        /// If the setting "Emby.Dlna.PlayTo": "Debug" msut be set in logging.default.json for this property to work.
        /// </summary>
        public bool EnablePlayToTracing { get; set; }

        /// <summary>
        /// Gets or sets the ssdp client discovery interval time (in seconds).
        /// This is the time after which the server will send a ssdp search request.
        /// </summary>
        public int ClientDiscoveryIntervalSeconds { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which ssdp alive notifications are transmitted.
        /// </summary>
        public int AliveMessageIntervalSeconds { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which ssdp alive notifications are transmitted. MIGRATING - TO BE REMOVED ONCE WEB HAS BEEN ALTERED.
        /// </summary>
        public int BlastAliveMessageIntervalSeconds
        {
            get
            {
                return AliveMessageIntervalSeconds;
            }

            set
            {
                AliveMessageIntervalSeconds = value;
            }
        }

        /// <summary>
        /// Gets or sets the default user account that the dlna server uses.
        /// </summary>
        public string? DefaultUserId { get; set; }

        /// <summary>
        /// Gets or sets the number of times SSDP UDP messages are sent.
        /// </summary>
        public int UDPSendCount { get; set; }

        /// <summary>
        /// Gets or sets the delay between each groups of SSDP messages (in ms).
        /// </summary>
        public int UDPSendDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether playTo device profiles should be created.
        /// </summary>
        public bool AutoCreatePlayToProfiles { get; set; }
    }
}
