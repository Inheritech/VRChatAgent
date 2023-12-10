using Agent.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Agent.Client.Extensions {
    public static class IServiceCollectionExtensions {
        public static IServiceCollection AddVRChatService<TStorage>(this IServiceCollection services) where TStorage : class, IStorage {
            services.AddSingleton<IStorage, TStorage>();
            services.AddSingleton<ICookieContainerProvider, CookieContainerProvider>();
            services.AddHttpClient<VRChatService>((services, client) => {
                var options = services.GetRequiredService<IOptions<VRChatOptions>>();
                client.BaseAddress = new Uri(options.Value.ApiUrl);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(options.Value.UserAgent, "1.0.0"));
            })
            .ConfigurePrimaryHttpMessageHandler(services => {
                var cookieContainerProvider = services.GetRequiredService<ICookieContainerProvider>();
                var handler = new HttpClientHandler {
                    CookieContainer = cookieContainerProvider.CookieContainer
                };
                return handler;
            });
            return services;
        } 
    }
}
