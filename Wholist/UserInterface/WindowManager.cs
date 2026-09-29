using System;
using System.Linq;
using Dalamud.Interface.Windowing;
using Wholist.Common;
using Wholist.UserInterface.Windows.NearbyPlayers;
using Wholist.UserInterface.Windows.Settings;

namespace Wholist.UserInterface
{
    internal sealed class WindowManager : IDisposable
    {
        /// <summary>
        ///     Handle the plugin being logged in.
        /// </summary>
        private void OnLogin()
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.WindowingSystem));

            if (!Services.Configuration.NearbyPlayers.OpenOnLogin)
            {
                return;
            }

            var nearbyWindow = this.windows.FirstOrDefault(window => window is NearbyPlayersWindow);
            nearbyWindow?.IsOpen = true;
        }

        /// <summary>
        ///     Handle the plugin being logged out.
        /// </summary>
        private void OnLogout(int type, int code)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.WindowingSystem));

            var nearbyWindow = this.windows.FirstOrDefault(window => window is NearbyPlayersWindow);
            nearbyWindow?.IsOpen = false;
        }

        private bool disposedValue;

        /// <summary>
        ///     All windows to add to the windowing system, holds all references.
        /// </summary>
        private readonly Window[] windows = [new NearbyPlayersWindow(), new SettingsWindow()];

        /// <summary>
        ///     The windowing system.
        /// </summary>
        private WindowSystem WindowingSystem { get; } = new(Services.PluginInterface.InternalName);

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowManager" /> class.
        /// </summary>
        private WindowManager()
        {
            foreach (var window in this.windows)
            {
                this.WindowingSystem.AddWindow(window);
            }

            Services.PluginInterface.UiBuilder.OpenConfigUi += this.ToggleConfigWindow;
            Services.PluginInterface.UiBuilder.OpenMainUi += this.ToggleMainWindow;
            Services.PluginInterface.UiBuilder.Draw += this.WindowingSystem.Draw;
            Services.ClientState.Login += this.OnLogin;
            Services.ClientState.Logout += this.OnLogout;

            if (Services.ClientState.IsLoggedIn)
            {
                this.OnLogin();
            }
        }

        /// <summary>
        ///     Disposes of the window manager.
        /// </summary>
        public void Dispose()
        {
            if (this.disposedValue)
            {
                return;
            }

            Services.PluginInterface.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
            Services.PluginInterface.UiBuilder.OpenMainUi -= this.ToggleMainWindow;
            Services.PluginInterface.UiBuilder.Draw -= this.WindowingSystem.Draw;
            Services.ClientState.Login -= this.OnLogin;
            Services.ClientState.Logout -= this.OnLogout;

            this.WindowingSystem.RemoveAllWindows();
            foreach (var disposable in this.windows.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            this.disposedValue = true;
        }

        /// <summary>
        ///     Toggles the open state of the configuration window.
        /// </summary>
        internal void ToggleConfigWindow()
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.WindowingSystem));
            this.windows.FirstOrDefault(window => window is SettingsWindow)?.Toggle();
        }

        internal void ToggleMainWindow()
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.WindowingSystem));
            this.windows.FirstOrDefault(window => window is NearbyPlayersWindow)?.Toggle();
        }
    }
}
