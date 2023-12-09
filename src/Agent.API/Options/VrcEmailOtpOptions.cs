namespace Agent.API.Options {
    public class VrcEmailOtpOptions {
        public const string ConfigurationKey = "VrcEmailOtp";

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; } = 993;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
