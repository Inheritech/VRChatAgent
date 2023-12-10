using System.Net;

namespace Agent.Client.Services {
    public interface ICookieContainerProvider {
        public CookieContainer CookieContainer { get; }
    }
}
