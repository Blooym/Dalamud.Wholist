using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using Sirensong.Game.Helpers;
using Sirensong.UserInterface;
using Wholist.Common;
using Wholist.DataStructures;
using Wholist.Resources.Localization;

namespace Wholist.UserInterface.Windows.NearbyPlayers
{
    internal sealed class NearbyPlayersWindow : Window, IDisposable
    {
        /// <inheritdoc cref="NearbyPlayersLogic" />
        private readonly NearbyPlayersLogic logic = new();

        /// <summary>
        ///     Creates a new instance of the <see cref="NearbyPlayersWindow" />.
        /// </summary>
        internal NearbyPlayersWindow() : base(Strings.Windows_Who_Title)
        {
            this.Size = new Vector2(450, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.TitleBarButtons =
            [
                new()
                {
                    Icon = FontAwesomeIcon.Cog,
                    ShowTooltip = () => SiGui.AddTooltip(Strings.UserInterface_Settings_NearbyPlayers_SettingsTip),
                    Click = (btn) => Services.WindowManager.ToggleConfigWindow()
                },
                new()
                {
                    Icon = FontAwesomeIcon.Heart,
                    ShowTooltip = () => SiGui.AddTooltip(Strings.UserInterface_Settings_NearbyPlayers_DonateTip),
                    Click = (btn) => Util.OpenLink(Constants.KoFiLink)
                }
            ];
        }

        public void Dispose() => this.logic.Dispose();

        public override bool DrawConditions()
        {
            if (Services.Configuration.NearbyPlayers.HideInCombat && Services.Condition[ConditionFlag.InCombat])
            {
                return false;
            }

            if (Services.Configuration.NearbyPlayers.HideInInstance && (ConditionHelper.IsBoundByDuty() || ConditionHelper.IsInIslandSanctuary()))
            {
                return false;
            }

            if (NearbyPlayersLogic.IsPvP)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Draws the window.
        /// </summary>
        public override void Draw()
        {
            var playersToDraw = this.logic.GetNearbyPlayers();
            var childSize = Services.Configuration.NearbyPlayers.ShowSearchBar ? new Vector2(0, -55) : new Vector2(0, -25);

            using (var nearbyChild = ImRaii.Child("##NearbyChild", childSize))
            {
                if (nearbyChild)
                {
                    this.DrawNearbyPlayersTable(playersToDraw);
                }
            }

            // Draw the search box & total players text if not in minimal mode
            if (Services.Configuration.NearbyPlayers.ShowSearchBar)
            {
                DrawSearchBar(ref this.logic.SearchText);
            }

            DrawTotalPlayers(playersToDraw.Count);
        }

        /// <summary>
        ///     Draws the nearby players table.
        /// </summary>
        /// <param name="playersToDraw"></param>
        private void DrawNearbyPlayersTable(List<PlayerInfoSlim> playersToDraw)
        {
            using var nearbyTable = ImRaii.Table("##NearbyTable", 6, ImGuiTableFlags.ScrollY | ImGuiTableFlags.Borders | ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Resizable);
            if (nearbyTable)
            {
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Name, ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoHide, 220);
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Job, ImGuiTableColumnFlags.WidthStretch, 150);
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Level, ImGuiTableColumnFlags.WidthStretch, 80);
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Homeworld, ImGuiTableColumnFlags.WidthStretch, 150);
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Company, ImGuiTableColumnFlags.WidthStretch, 120);
                ImGui.TableSetupColumn(Strings.UserInterface_NearbyPlayers_Players_Distance, ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.DefaultHide, 80);
                ImGui.TableSetupScrollFreeze(0, 1);
                ImGui.TableHeadersRow();
                ImGuiClip.ClippedDraw(playersToDraw, this.DrawPlayer, ImGui.GetTextLineHeightWithSpacing());
            }
        }

        /// <summary>
        ///     Draws the search bar.
        /// </summary>
        /// <param name="searchText"></param>
        private static void DrawSearchBar(ref string searchText)
        {
            ImGui.SetNextItemWidth(-23 * ImGuiHelpers.GlobalScale);
            ImGui.InputTextWithHint("##NearbySearch", Strings.UserInterface_NearbyPlayers_Search, ref searchText, 100);
            ImGuiComponents.HelpMarker(@"By default search checks only player name. Use advanced filters for more specific searches.

Filters:
- ""name:"" (contains)
- ""level:"" (exact)
- ""job:"" job name (contains) or abbreviation (exact)
- ""homeworld:"" (contains)
- ""company:"" (exact)

Filters are additive, meaning each one will add results that match. This may change in the future.

Any number of filters can be used in one search (e.g. ""name:meteor level:50 job:warrior"").");

        }

        /// <summary>
        ///     Draws a player in the table.
        /// </summary>
        /// <param name="obj"></param>
        private void DrawPlayer(PlayerInfoSlim obj)
        {
            ImGui.TableNextColumn();

            // Name.
            SiGui.TextColoured(obj.NameColour, obj.Name);
            this.DrawPlayerContextMenu(obj);
            ImGui.TableNextColumn();

            // Job.
            SiGui.TextColoured(NearbyPlayersLogic.GetJobColour(obj.Job), NearbyPlayersLogic.GetJobName(obj.Job));
            ImGui.TableNextColumn();

            // Level.
            SiGui.Text(obj.Level.ToString());
            ImGui.TableNextColumn();

            // HomeWorld.
            SiGui.Text(obj.HomeWorld);
            ImGui.TableNextColumn();

            // Company.
            SiGui.Text(obj.CompanyTag);
            ImGui.TableNextColumn();

            // Distance.
            DrawDirectionArrow(NearbyPlayersLogic.GetPlayerDirection(obj));
            SiGui.Text($"{obj.Distance} yalms");
        }

        private static void DrawTotalPlayers(int totalPlayers) => ImGuiHelpers.CenteredText(string.Format(Strings.UserInterface_NearbyPlayers_Players_Total, totalPlayers.ToString()));

        /// <summary>
        ///     Draws the context menu for a player.
        /// </summary>
        /// <param name="obj"></param>
        private void DrawPlayerContextMenu(PlayerInfoSlim obj)
        {
            using var popupCtx = ImRaii.ContextPopupItem($"{obj.Name}{obj.HomeWorld}##WholistPopContext");
            if (popupCtx)
            {
                // Heading.
                SiGui.Heading(string.Format(Strings.UserInterface_NearbyPlayers_Players_Submenu_Heading, $"{obj.Name}@{obj.HomeWorld}"));

                // Examine.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_Examine))
                {
                    obj.OpenExamine();
                }

                // View Character Card.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_AdventurePlate))
                {
                    obj.OpenCharaCard();
                }

                // Target.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_Target))
                {
                    obj.Target();
                }

                // Focus target.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_FocusTarget))
                {
                    obj.FocusTarget();
                }

                // Set tell target
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_Tell))
                {
                    NearbyPlayersLogic.SetChatTellTarget(obj.Name, obj.HomeWorld);
                }

                // Invite to party
                using (ImRaii.Disabled(obj.IsInParty))
                {
                    if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_InviteToParty))
                    {
                        this.logic.InviteToParty(obj.Name, obj.HomeWorld);
                    }
                }

                // Add to blacklist.
                using (ImRaii.Disabled(obj.IsFriend))
                {
                    if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_AddToBlacklist))
                    {
                        this.logic.PromptUserBlacklist(obj.Name, obj.HomeWorld);
                    }
                }

                // Find on Map.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_OpenOnMap))
                {
                    NearbyPlayersLogic.FlagAndOpen(obj.Position, obj.Name);
                }

                // Find on Lodestone.
                if (ImGui.Selectable(Strings.UserInterface_NearbyPlayers_Players_Submenu_Lodestone))
                {
                    NearbyPlayersLogic.SearchPlayerOnLodestone(obj.Name, obj.HomeWorld);
                }
            }
        }

        private static void DrawDirectionArrow(double? maybeDirection)
        {
            const double AngleShift = 0.5235987755982988;

            ImGui.Dummy(new Vector2(ImGui.GetTextLineHeight()));
            ImGui.SameLine(0.0f, ImGui.GetStyle().ItemInnerSpacing.X);
            if (maybeDirection is not { } direction)
            {
                return;
            }

            var foregroundColor = ImGui.GetColorU32(ImGuiCol.Text);
            var rectMin = ImGui.GetItemRectMin();
            var rectMax = ImGui.GetItemRectMax();
            var rectMid = Vector2.Lerp(rectMin, rectMax, 0.5f);
            var innerHalfSize = (rectMax - rectMin) * 0.4f;
            var mainVec = UnitVector(direction);
            var arrowHead = rectMid + (innerHalfSize * mainVec);
            var arrowTail = rectMid - (innerHalfSize * mainVec * 0.33333333f);
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddTriangleFilled(arrowHead, arrowTail,
                rectMid - (innerHalfSize * UnitVector(direction + AngleShift)), foregroundColor);
            drawList.AddTriangleFilled(arrowTail, arrowHead,
                rectMid - (innerHalfSize * UnitVector(direction - AngleShift)), foregroundColor);
        }

        private static Vector2 UnitVector(double angle)
        {
            var (sin, cos) = Math.SinCos(angle);
            return new Vector2((float)cos, (float)sin);
        }
    }
}
