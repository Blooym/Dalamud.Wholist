using Wholist.Common;
using Wholist.Resources.Localization;
using Wholist.UserInterface.Windows.Settings.Components;

namespace Wholist.UserInterface.Windows.Settings.TableParts.Sidebar
{
    internal static class BehaviourTab
    {
        internal static void Draw()
        {
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_PrioritizeKnown, Strings.UserInterface_Settings_NearbyPlayers_PrioritizeKnown_Description, ref Services.Configuration.NearbyPlayers.PrioritizeKnown);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_FilterAFKPlayers, Strings.UserInterface_Settings_NearbyPlayers_FilterAFKPlayers_Description, ref Services.Configuration.NearbyPlayers.FilterAfk);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_FilterBlockedPlayers, Strings.UserInterface_Settings_NearbyPlayers_FilterBlockedPlayers_Description, ref Services.Configuration.NearbyPlayers.FilterBlockedPlayers);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_FilterLowLevelCharacters, Strings.UserInterface_Settings_NearbyPlayers_FilterLowLevelCharacters_Description, ref Services.Configuration.NearbyPlayers.FilterLowLevel);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_PlayerRelativeDirection, Strings.UserInterface_Settings_NearbyPlayers_PlayerRelativeDirection_Description, ref Services.Configuration.NearbyPlayers.PlayerRelativeDirection);
            Slider.Draw(Strings.UserInterface_Settings_NearbyPlayers_MaxPlayers, Strings.UserInterface_Settings_NearbyPlayers_MaxPlayers_Description, ref Services.Configuration.NearbyPlayers.MaxPlayersToShow, 5, 100);
            EnumCombo.Draw(Strings.UserInterface_Settings_NearbyPlayers_LodestoneRegion, Strings.UserInterface_Settings_NearbyPlayers_LodestoneRegion_Description, ref Services.Configuration.NearbyPlayers.LodestonePlayerSearchRegion);
        }
    }
}
