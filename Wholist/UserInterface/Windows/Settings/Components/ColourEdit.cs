using System.Numerics;
using Dalamud.Bindings.ImGui;

namespace Wholist.UserInterface.Windows.Settings.Components
{
    internal sealed class ColourEdit
    {
        /// <summary>
        ///     Draw a colour picker for a Vector4
        /// </summary>
        /// <param name="label">The label to display</param>
        /// <param name="colour">The reference to the colour to edit</param>
        /// <returns>True if the colour was changed and the item was deactivated</returns>
        internal static bool Draw(string label, ref Vector4 colour)
        {
            ImGui.ColorEdit4(label, ref colour, ImGuiColorEditFlags.DisplayRgb | ImGuiColorEditFlags.AlphaPreviewHalf);
            return ImGui.IsItemDeactivatedAfterEdit();
        }
    }
}
