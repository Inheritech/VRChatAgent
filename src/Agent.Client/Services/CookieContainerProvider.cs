using System.Net;

namespace Agent.Client.Services {
    public class CookieContainerProvider : ICookieContainerProvider {
        public CookieContainer CookieContainer { get; } = new CookieContainer();
    }
}
