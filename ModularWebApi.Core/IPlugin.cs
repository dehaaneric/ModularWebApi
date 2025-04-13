using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularWebApi.Core
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        // More abstract signature that doesn't expose WebApplication directly
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        // Use a more abstract parameter type
        void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
    }
}
