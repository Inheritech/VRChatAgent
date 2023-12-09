using Agent.Client.Requests;
using Agent.Client.Responses.Auth;
using Agent.Client.Responses.Notifications;
using Agent.Client.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Agent.Client {
    public enum LoginResult {
        Failed,
        Succeeded,
        RequiresEmailOtp
    }

    public partial class VRChatService(HttpClient httpClient, IStorage keyedStorage, ICookieContainerProvider cookieContainerProvider, IOptions<VRChatOptions> options) {
        public const string AuthCookieName = "auth";
        public const string TwoFactorAuthCookieName = "twoFactorAuth";

        public CurrentUserInfo? CurrentUser { get; private set; }

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
            Converters = {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        public async Task<LoginResult> LoginAsync() {
            await ReadAuthCookiesAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, Endpoints.Auth.Login);
            request.Headers.Authorization = GetUserAuthenticationHeader();

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.Contains("emailOtp")) {
                return LoginResult.RequiresEmailOtp;
            }
            CurrentUser = JsonSerializer.Deserialize<CurrentUserInfo>(responseContent);
            if (CurrentUser == null) {
                return LoginResult.Failed;
            }
            return LoginResult.Succeeded;
        }

        public async Task<TwoFactorEmailVerificationResult> VerifyTwoFactorEmailOtp(string code) {
            var response = await httpClient.PostAsJsonAsync(Endpoints.Auth.TwoFactorEmailOtp, new TwoFactorEmailCode(code));
            return await response.Content.ReadFromJsonAsync<TwoFactorEmailVerificationResult>();
        }

        public async Task<Notification[]> GetNotificationsAsync() {
            return await httpClient.GetFromJsonAsync<Notification[]>(Endpoints.Notifications.List, _jsonOptions);
        }

        public async Task<bool> AcceptFriendRequestAsync(string notificationId) {
            var response = await httpClient.PutAsync(Endpoints.Notifications.AcceptFriendRequest(notificationId), null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ClearNotificationsAsync() {
            var response = await httpClient.PutAsync(Endpoints.Notifications.Clear, null);
            return response.IsSuccessStatusCode;
        }

        private AuthenticationHeaderValue GetUserAuthenticationHeader() {
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Value.Username}:{options.Value.Password}")));
        }

        private async Task ReadStorageCookieIntoContainerAsync(string cookieName) {
            var uri = httpClient.BaseAddress ?? new Uri(options.Value.ApiUrl);
            var cookieContainer = cookieContainerProvider.CookieContainer;
            var vrChatCookies = cookieContainer.GetCookies(uri);
            var storedCookieValue = await keyedStorage.GetValueAsync(cookieName);
            if (storedCookieValue != null && !vrChatCookies.Any(c => c.Name == cookieName)) {
                cookieContainer.Add(uri, new Cookie(cookieName, storedCookieValue));
            }
        }

        private async Task PersistContainerCookieIntoStorageAsync(string cookieName) {
            var uri = httpClient.BaseAddress ?? new Uri(options.Value.ApiUrl);
            var cookieContainer = cookieContainerProvider.CookieContainer;
            var vrChatCookies = cookieContainer.GetCookies(uri);
            var cookie = vrChatCookies.FirstOrDefault(c => c.Name == cookieName);
            if (cookie != null) {
                await keyedStorage.SetValueAsync(cookieName, cookie.Value);
            }
        }

        private async Task ReadAuthCookiesAsync() {
            await ReadStorageCookieIntoContainerAsync(AuthCookieName);
            await ReadStorageCookieIntoContainerAsync(TwoFactorAuthCookieName);
        }

        private async Task PersistAuthCookiesAsync() {
            await PersistContainerCookieIntoStorageAsync(AuthCookieName);
            await PersistContainerCookieIntoStorageAsync(TwoFactorAuthCookieName);
        }
    }
}
