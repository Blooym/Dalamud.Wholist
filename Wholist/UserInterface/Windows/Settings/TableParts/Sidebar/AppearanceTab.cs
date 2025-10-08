using Dalamud.Bindings.ImGui;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;
using Wholist.Common;
using Wholist.Configuration;
using Wholist.Resources.Localization;
using Wholist.UserInterface.Windows.Settings.Components;

namespace Wholist.UserInterface.Windows.Settings.TableParts.Sidebar
{
    internal static class AppearanceTab
    {
        internal static void Draw()
        {
            SiGui.Heading(Strings.UserInterface_Settings_Display_Heading);
            DrawDisplayOptions();

            SiGui.Heading(Strings.UserInterface_Settings_Colours_Heading);
            SiGui.TextDisabled(Strings.UserInterface_Settings_Colours_NameColours);
            DrawNameColours();
            ImGui.Dummy(Spacing.SectionSpacing);
            SiGui.TextDisabled(Strings.UserInterface_Settings_Colours_JobColours);
            var isPerJobColours = Services.Configuration.Colours.JobColMode == PluginConfiguration.ColourConfiguration.JobColourMode.Job;
            if (Checkbox.Draw(Strings.UserInterface_Settings_Colours_UsePerJobColours, Strings.UserInterface_Settings_Colours_UsePerJobColours_Description, ref isPerJobColours))
            {
                Services.Configuration.Colours.JobColMode = isPerJobColours ? PluginConfiguration.ColourConfiguration.JobColourMode.Job : PluginConfiguration.ColourConfiguration.JobColourMode.Role;
                Services.Configuration.Save();
            }
            if (isPerJobColours)
            {
                DrawJobColours();
            }
            else
            {
                DrawRoleColours();
            }
            ImGui.Dummy(Spacing.SectionSpacing);
            DrawOtherColourOptions();
        }

        private static void DrawDisplayOptions()
        {
            TextWithDescription.Draw(Strings.UserInterface_Settings_NearbyPlayers_ShownRows, Strings.UserInterface_Settings_NearbyPlayers_ShownRows_Description);
            TextWithDescription.Draw(Strings.UserInterface_Settings_NearbyPlayers_InfoBarPlayers, Strings.UserInterface_Settings_NearbyPlayers_InfoBarPlayers_Description);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_JobAbbreviations, Strings.UserInterface_Settings_NearbyPlayers_JobAbbreviations_Description, ref Services.Configuration.NearbyPlayers.UseJobAbbreviations);
            Checkbox.Draw(Strings.UserInterface_Settings_NearbyPlayers_ShowSearchbar, Strings.UserInterface_Settings_NearbyPlayers_ShowSearchbar_Description, ref Services.Configuration.NearbyPlayers.ShowSearchBar);
        }

        private static void DrawNameColours()
        {
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Default, ref Services.Configuration.Colours.Name.Default))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Friend, ref Services.Configuration.Colours.Name.Friend))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Party, ref Services.Configuration.Colours.Name.Party))
            {
                Services.Configuration.Save();
            }
            if (Services.IpcManager.LightlessActivePairsIpcAvailable && ColourEdit.Draw("Sync Pair [3rd party]", ref Services.Configuration.Colours.Name.SyncServicePaired))
            {
                Services.Configuration.Save();
            }
        }

        private static void DrawRoleColours()
        {
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Healer, ref Services.Configuration.Colours.Role.Healer))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Tank, ref Services.Configuration.Colours.Role.Tank))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_MeleeDPS, ref Services.Configuration.Colours.Role.MeleeDps))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_RangedDPS, ref Services.Configuration.Colours.Role.RangedDps))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_Colours_Other, ref Services.Configuration.Colours.Role.Other))
            {
                Services.Configuration.Save();
            }
        }
        private static void DrawJobColours()
        {
            // Tanks
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Paladin, ref Services.Configuration.Colours.Job.Paladin))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Warrior, ref Services.Configuration.Colours.Job.Warrior))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_DarkKnight, ref Services.Configuration.Colours.Job.DarkKnight))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Gunbreaker, ref Services.Configuration.Colours.Job.Gunbreaker))
            {
                Services.Configuration.Save();
            }

            // Healers
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_WhiteMage, ref Services.Configuration.Colours.Job.WhiteMage))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Scholar, ref Services.Configuration.Colours.Job.Scholar))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Astrologian, ref Services.Configuration.Colours.Job.Astrologian))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Sage, ref Services.Configuration.Colours.Job.Sage))
            {
                Services.Configuration.Save();
            }

            // Melee DPS
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Monk, ref Services.Configuration.Colours.Job.Monk))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Dragoon, ref Services.Configuration.Colours.Job.Dragoon))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Ninja, ref Services.Configuration.Colours.Job.Ninja))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Samurai, ref Services.Configuration.Colours.Job.Samurai))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Reaper, ref Services.Configuration.Colours.Job.Reaper))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Viper, ref Services.Configuration.Colours.Job.Viper))
            {
                Services.Configuration.Save();
            }

            /// Ranged DPS
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Bard, ref Services.Configuration.Colours.Job.Bard))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Machinist, ref Services.Configuration.Colours.Job.Machinist))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Dancer, ref Services.Configuration.Colours.Job.Dancer))
            {
                Services.Configuration.Save();
            }

            // Casters
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_BlackMage, ref Services.Configuration.Colours.Job.BlackMage))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Summoner, ref Services.Configuration.Colours.Job.Summoner))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_RedMage, ref Services.Configuration.Colours.Job.RedMage))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Pictomancer, ref Services.Configuration.Colours.Job.Pictomancer))
            {
                Services.Configuration.Save();
            }
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_BlueMage, ref Services.Configuration.Colours.Job.BlueMage))
            {
                Services.Configuration.Save();
            }

            // Other
            if (ColourEdit.Draw(Strings.UserInterface_Settings_NearbyPlayers_Job_Other, ref Services.Configuration.Colours.Job.Other))
            {
                Services.Configuration.Save();
            }
        }

        private static void DrawOtherColourOptions()
        {
            if (ImGui.Button(Strings.UserInterface_Settings_Colours_ResetAll))
            {
                Services.Configuration.Colours = new PluginConfiguration.ColourConfiguration();
                Services.Configuration.Save();
            }
        }
    }
}
