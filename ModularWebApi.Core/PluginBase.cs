using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularWebApi.Core
{
    public class PluginBase : IPlugin
    {
        public virtual string Name { get; set; } = null!;
        public virtual string Description { get; set; } = null!;

        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Base implementation
        }

        public virtual void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            // Base implementation
        }
    }
}
