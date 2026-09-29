using Dalamud.Plugin;
using Sirensong;
using Wholist.Common;

namespace Wholist
{
    internal sealed class Plugin : IDalamudPlugin
    {
        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        public Plugin(IDalamudPluginInterface pluginInterface)
        {
            SirenCore.Initialize(pluginInterface, pluginInterface.InternalName);
            Services.Initialize(pluginInterface);
        }

        /// <summary>
        ///     Disposes of the plugin.
        /// </summary>
        public void Dispose()
        {
            SirenCore.Dispose();
            Services.Dispose();
        }
    }
}
