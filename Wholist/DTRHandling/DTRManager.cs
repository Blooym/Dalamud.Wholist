using System;
using System.Linq;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Plugin.Services;
using Wholist.Common;
using Wholist.Game;
using Wholist.Resources.Localization;

namespace Wholist.DTRHandling
{
    public sealed class DTRManager : IDisposable
    {
        private const string NEARBY_PLAYERS_DTR_ICON_PAYLOAD = "\uE033";
        private readonly IDtrBarEntry nearbyPlayersDtrEntry;

        private DTRManager()
        {
            this.nearbyPlayersDtrEntry = Services.DtrBar.Get("Nearby Player Count", $"{NEARBY_PLAYERS_DTR_ICON_PAYLOAD} -");
            this.nearbyPlayersDtrEntry.Tooltip = string.Format(Strings.DTR_NearbyPlayers_Tooltip, "-");
            this.nearbyPlayersDtrEntry.OnClick += DtrOnClick;
            Services.Framework.Update += this.OnFrameworkUpdate;
        }

        public void Dispose()
        {
            Services.Framework.Update -= this.OnFrameworkUpdate;
            this.nearbyPlayersDtrEntry.OnClick -= DtrOnClick;
            this.nearbyPlayersDtrEntry.Remove();
        }

        private static void DtrOnClick(DtrInteractionEvent mouseEventData)
        {
            if (mouseEventData.ClickType == MouseClickType.Left)
            {
                Services.WindowManager.ToggleMainWindow();
            }
            else if (mouseEventData.ClickType == MouseClickType.Right)
            {
                Services.WindowManager.ToggleConfigWindow();
            }
        }

        private void OnFrameworkUpdate(IFramework framework)
        {
            if (this.nearbyPlayersDtrEntry.UserHidden || !Services.ClientState.IsLoggedIn)
            {
                return;
            }

            if (Services.ClientState.IsPvP)
            {
                this.nearbyPlayersDtrEntry.Shown = false;
                return;
            }
            else if (!this.nearbyPlayersDtrEntry.Shown)
            {
                this.nearbyPlayersDtrEntry.Shown = true;
            }

            var nearbyPlayerCount = PlayerManager.GetNearbyPlayers(Services.Configuration.NearbyPlayers.FilterBlockedPlayers).Count();
            this.nearbyPlayersDtrEntry.Text = $"{NEARBY_PLAYERS_DTR_ICON_PAYLOAD} {nearbyPlayerCount}";
            this.nearbyPlayersDtrEntry.Tooltip = string.Format(Strings.DTR_NearbyPlayers_Tooltip, nearbyPlayerCount);
        }
    }
}
