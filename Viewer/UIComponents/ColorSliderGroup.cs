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

        public event EventHandler<Color> ColorChanged;

        public ColorSliderGroup(bool includeAlpha)
        {
            RedSlider = CreateSlider();
            GreenSlider = CreateSlider();
            BlueSlider = CreateSlider();

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

        public void AddToForm(Form form)
        {
            form.Controls.Add(RedSlider);
            form.Controls.Add(GreenSlider);
            form.Controls.Add(BlueSlider);
            if (AlphaSlider != null)
                form.Controls.Add(AlphaSlider);
        }

        public void PositionSliders(Point location)
        {
            int spacing = 40;
            RedSlider.Location = location;
            GreenSlider.Location = new Point(location.X, location.Y + spacing);
            BlueSlider.Location = new Point(location.X, location.Y + spacing * 2);
            if (AlphaSlider != null)
                AlphaSlider.Location = new Point(location.X, location.Y + spacing * 3);
        }
    }

}
