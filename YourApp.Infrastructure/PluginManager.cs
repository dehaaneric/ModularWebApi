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

            if (!Directory.Exists(pluginsPath))
            {
                _logger.LogWarning("Plugins directory not found: {Path}", pluginsPath);
                return;
            }

            foreach (var dllPath in Directory.EnumerateFiles(pluginsPath, "*.dll"))
            {
                if (!IsValidModule(dllPath)) continue;

                LoadPluginsFromAssembly(dllPath);
            }
        }

        private void LoadPluginsFromAssembly(string dllPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));

                foreach (var plugin in InstantiatePlugins(pluginTypes))
                {
                    _plugins.Add(plugin);
                    _logger.LogInformation("Loaded plugin: {PluginName} - {PluginDescription}",
                        plugin.Name, plugin.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load plugin from {DllPath}", dllPath);
            }
        }

        private IEnumerable<IPlugin> InstantiatePlugins(IEnumerable<Type> pluginTypes)
        {
            foreach (var pluginType in pluginTypes)
            {
                IPlugin? plugin = null;
                try
                {
                    plugin = Activator.CreateInstance(pluginType) as IPlugin;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to instantiate plugin of type {PluginType}", pluginType.FullName);
                }

                if (plugin != null)
                {
                    yield return plugin;
                }
            }
        }

        private bool IsValidModule(string dllPath)
        {
            return Path.GetFileName(dllPath)
                ?.StartsWith("ModularWebApi.Plugin", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }
}
