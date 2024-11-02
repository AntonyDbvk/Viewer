using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer.UIComponents
{
    public class ColorSliderGroup
    {
        public TrackBar RedSlider { get; }
        public TrackBar GreenSlider { get; }
        public TrackBar BlueSlider { get; }
        public TrackBar AlphaSlider { get; } // только для RGBA
        public Label GroupLabel { get; } // подпись группы

        public event EventHandler<Color> ColorChanged;

        public ColorSliderGroup(bool includeAlpha, string labelText)
        {
            RedSlider = CreateSlider();
            GreenSlider = CreateSlider();
            BlueSlider = CreateSlider();
            GroupLabel = new Label { Text = labelText, AutoSize = true };

            if (includeAlpha)
                AlphaSlider = CreateSlider();

            SetupSliderEvents();
        }

        private TrackBar CreateSlider()
        {
            return new TrackBar
            {
                Minimum = 0,
                Maximum = 255,
                TickFrequency = 5,
                SmallChange = 1,
                LargeChange = 10,
            };
        }

        private void SetupSliderEvents()
        {
            RedSlider.Scroll += (s, e) => OnColorChanged();
            GreenSlider.Scroll += (s, e) => OnColorChanged();
            BlueSlider.Scroll += (s, e) => OnColorChanged();
            if (AlphaSlider != null)
                AlphaSlider.Scroll += (s, e) => OnColorChanged();
        }

        private void OnColorChanged()
        {
            var color = AlphaSlider != null
                ? Color.FromArgb(AlphaSlider.Value, RedSlider.Value, GreenSlider.Value, BlueSlider.Value)
                : Color.FromArgb(RedSlider.Value, GreenSlider.Value, BlueSlider.Value);
            ColorChanged?.Invoke(this, color);
        }

        public void AddToForm(Control parent)
        {
            parent.Controls.Add(GroupLabel);
            parent.Controls.Add(RedSlider);
            parent.Controls.Add(GreenSlider);
            parent.Controls.Add(BlueSlider);
            if (AlphaSlider != null)
                parent.Controls.Add(AlphaSlider);
        }

        public void PositionSliders(Point location)
        {
            int spacing = 40;
            GroupLabel.Location = location;
            RedSlider.Location = new Point(location.X, location.Y + spacing);
            GreenSlider.Location = new Point(location.X, location.Y + spacing * 2);
            BlueSlider.Location = new Point(location.X, location.Y + spacing * 3);
            if (AlphaSlider != null)
                AlphaSlider.Location = new Point(location.X, location.Y + spacing * 4);
        }
    }
}
