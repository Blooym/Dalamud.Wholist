using Dalamud.Bindings.ImGui;
using Sirensong.UserInterface;
using Sirensong.UserInterface.Style;
using Wholist.Common;

namespace Wholist.UserInterface.Windows.Settings.Components
{
    internal static class Slider
    {
        /// <summary>
        ///     Draws a slider with custom input validation.
        /// </summary>
        /// <param name="label">The label/title of the slider</param>
        /// <param name="description">The description to show underneath the slider</param>
        /// <param name="value">the reference to the current slider value</param>
        /// <param name="min">The minimum slider value</param>
        /// <param name="max">The maximum slider value</param>
        /// <returns>If the slider has changed.</returns>
        internal static bool Draw(string label, string description, ref int value, int min, int max)
        {
            SiGui.Text(label);
            var sliderChanged = SiGui.SliderInt($"##{label}", ref value, min, max, true);
            if (sliderChanged)
            {
                Services.Configuration.Save();
            }
            SiGui.TextDisabledWrapped(description);
            ImGui.Dummy(Spacing.SectionSpacing);
            return sliderChanged;
        }
    }
}
