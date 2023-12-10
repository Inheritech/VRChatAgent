namespace Agent.Client {
    public static class Endpoints {
        public static class Auth {
            public const string Login = "auth/user";
            public const string TwoFactorEmailOtp = "auth/twofactorauth/emailotp/verify";
        }

        public static class Notifications {
            public const string List = "auth/user/notifications";
            public static string AcceptFriendRequest(string notificationId) => $"auth/user/notifications/{notificationId}/accept";
            public const string Clear = "auth/user/notifications/clear";
        }
    }
}
