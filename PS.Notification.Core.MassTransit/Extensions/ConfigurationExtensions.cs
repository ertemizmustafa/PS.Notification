

using Microsoft.Extensions.DependencyInjection;

namespace PS.Notification.Core.MassTransit.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddPsMassTransit(this IServiceCollection services)
        {


            return services;
        }
    }
}
