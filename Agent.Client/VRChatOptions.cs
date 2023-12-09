namespace Agent.Client {
    public class VRChatOptions {
        public const string ConfigurationKey = "VRChat";

        /// <summary>
        /// User agent to use when making requests to VRChat's API
        /// </summary>
        public string UserAgent { get; set; } = "VRChatAutomatedAgent";

        /// <summary>
        /// URL to VRChat's API
        /// </summary>
        public string ApiUrl { get; set; } = "https://api.vrchat.cloud/api/1/";

        /// <summary>
        /// API Key to make calls to VRChat
        /// </summary>
        public string ApiKey { get; set; } = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26";

        /// <summary>
        /// The username of the agent
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of the agent
        /// </summary>
        public string Password { get; set; }
    }
}
