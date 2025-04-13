using Microsoft.Extensions.Logging;
using ModularWebApi.Core;
using System.Reflection;

namespace YourApp.Infrastructure
{
    public class PluginManager
    {
        private readonly ILogger<PluginManager> _logger;
        private readonly List<IPlugin> _plugins = new();

        public PluginManager(ILogger<PluginManager> logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();

        public void DiscoverPlugins(string pluginsPath)
        {
            _logger.LogInformation("Discovering plugins in: {Path}", pluginsPath);

            // Ensure plugins directory exists
            if (!Directory.Exists(pluginsPath))
            {
                _logger.LogWarning("Plugins directory not found: {Path}", pluginsPath);
                return;
            }

            // Load plugin assemblies
            foreach (var dllPath in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    var pluginTypes = assembly.GetTypes()
                        .Where(t => !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));

                    foreach (var pluginType in pluginTypes)
                    {
                        if (Activator.CreateInstance(pluginType) is IPlugin plugin)
                        {
                            _plugins.Add(plugin);
                            _logger.LogInformation("Loaded plugin: {PluginName} - {PluginDescription}",
                                plugin.Name, plugin.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load plugin from {DllPath}", dllPath);
                }
            }
        }
    }
}
