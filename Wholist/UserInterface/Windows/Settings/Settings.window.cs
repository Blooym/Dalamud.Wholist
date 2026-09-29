using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Sirensong.UserInterface.Style;
using Sirensong.UserInterface.Windowing;
using Wholist.Common;
using Wholist.Resources.Localization;
using Wholist.UserInterface.Windows.Settings.TableParts;

namespace Wholist.UserInterface.Windows.Settings
{
    internal sealed class SettingsWindow : Window
    {
        /// <inheritdoc cref="SettingsLogic" />
        private readonly SettingsLogic logic = new();

        /// <inheritdoc />
        public SettingsWindow() : base(string.Format(Strings.Windows_Settings_Title, Services.PluginInterface.Manifest.Name))
        {
            this.Size = new Vector2(700, 450);
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Flags = ImGuiWindowFlagExtra.NoScroll;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            using var pluginTable = ImRaii.Table("PluginSettings", 2);
            if (pluginTable)
            {
                ImGui.TableSetupColumn("PluginSettingsSidebar", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.25f);
                ImGui.TableSetupColumn("PluginSettingsList", ImGuiTableColumnFlags.WidthFixed, ImGui.GetContentRegionAvail().X * 0.75f);
                ImGui.TableNextRow();

                // Sidebar
                ImGui.TableNextColumn();
                using (var sidebarChild = ImRaii.Child("PluginSettingsSidebarChild", default, true))
                {
                    if (sidebarChild)
                    {
                        SettingsSidebar.Draw(this.logic);
                        ImGui.Dummy(Spacing.SidebarSectionSpacing);
                    }
                }

                // Listings
                ImGui.TableNextColumn();
                using (var listChild = ImRaii.Child("PluginSettingsListChild", default, true))
                {
                    if (listChild)
                    {
                        SettingsActive.Draw(this.logic);
                    }
                }
            }
        }
    }
}
