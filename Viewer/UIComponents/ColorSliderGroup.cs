using System;
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
        private Label RedLabel { get; }
        private Label GreenLabel { get; }
        private Label BlueLabel { get; }
        private Label AlphaLabel { get; }
        public Button ColorDialogButton { get; }
        public Panel ColorPreviewPanel { get; }
        private readonly ColorDialog _colorDialog;


        public event EventHandler<Color> ColorChanged;

        public ColorSliderGroup(bool includeAlpha, string labelText, string buttonText)
        {
            RedSlider = CreateSlider();
            GreenSlider = CreateSlider();
            BlueSlider = CreateSlider();
            GroupLabel = new Label { Text = labelText, AutoSize = true };
            RedLabel = CreateColorLabel("R");
            GreenLabel = CreateColorLabel("G");
            BlueLabel = CreateColorLabel("B");
            ColorDialogButton = new Button { Text = buttonText };
            _colorDialog = new ColorDialog();
            ColorPreviewPanel = new Panel 
            {
                Size = new Size(25, 25), 
                BackColor = Color.Black, 
                BorderStyle = BorderStyle.FixedSingle
            };
            if (includeAlpha)
            {
                AlphaSlider = CreateSlider();
                AlphaLabel = CreateColorLabel("A");
            }

            SetupSliderEvents();
            SetupColorDialogButton();
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

        private Label CreateColorLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
        }

        private void SetupSliderEvents()
        {
            RedSlider.Scroll += (s, e) => NotifyColorChanged();
            GreenSlider.Scroll += (s, e) => NotifyColorChanged();
            BlueSlider.Scroll += (s, e) => NotifyColorChanged();
            if (AlphaSlider != null)
                AlphaSlider.Scroll += (s, e) => NotifyColorChanged();
        }

        private void SetupColorDialogButton()
        {
            ColorDialogButton.Click += (s, e) =>
            {
                if (_colorDialog.ShowDialog() == DialogResult.OK)
                {
                    var selectedColor = _colorDialog.Color;
                    UpdateSlidersFromColor(selectedColor);
                    NotifyColorChanged();
                }
            };
        }

        private void NotifyColorChanged()
        {
            var color = AlphaSlider != null
                ? Color.FromArgb(AlphaSlider.Value, RedSlider.Value, GreenSlider.Value, BlueSlider.Value)
                : Color.FromArgb(RedSlider.Value, GreenSlider.Value, BlueSlider.Value);

            ColorPreviewPanel.BackColor = color;

            ColorChanged?.Invoke(this, color);
        }

        public void AddToForm(Control parent)
        {
            parent.Controls.Add(GroupLabel);
            parent.Controls.Add(RedSlider);
            parent.Controls.Add(GreenSlider);
            parent.Controls.Add(BlueSlider);
            parent.Controls.Add(RedLabel);
            parent.Controls.Add(GreenLabel);
            parent.Controls.Add(BlueLabel);
            parent.Controls.Add(ColorPreviewPanel);
            if (AlphaSlider != null)
            {
                parent.Controls.Add(AlphaSlider);
                parent.Controls.Add(AlphaLabel);
            }
            parent.Controls.Add(ColorDialogButton);
        }

        public void PositionSliders(Point location)
        {
            const int spacing = 50;
            const int labelOffset = -20;
            GroupLabel.Location = location;

            RedLabel.Location = new Point(location.X + labelOffset, location.Y + spacing);
            RedSlider.Location = new Point(location.X, location.Y + spacing);

            GreenLabel.Location = new Point(location.X + labelOffset, location.Y + spacing * 2);
            GreenSlider.Location = new Point(location.X, location.Y + spacing * 2);

            BlueLabel.Location = new Point(location.X + labelOffset, location.Y + spacing * 3);
            BlueSlider.Location = new Point(location.X, location.Y + spacing * 3);

            if (AlphaSlider != null)
            {
                AlphaLabel.Location = new Point(location.X + labelOffset, location.Y + spacing * 4);
                AlphaSlider.Location = new Point(location.X, location.Y + spacing * 4);
                ColorDialogButton.Location = new Point(location.X, location.Y + spacing * 5);
            }
            else ColorDialogButton.Location = new Point(location.X, location.Y + spacing * 4);

            ColorPreviewPanel.Location = new Point(ColorDialogButton.Left + ColorDialogButton.Width + 5, ColorDialogButton.Top);

        }

        public void Remove(Control form)
        {
            form.Controls.Remove(RedSlider);
            form.Controls.Remove(GreenSlider);
            form.Controls.Remove(BlueSlider);
            if (AlphaSlider != null) form.Controls.Remove(AlphaSlider);

            form.Controls.Remove(GroupLabel);
            form.Controls.Remove(RedLabel);
            form.Controls.Remove(GreenLabel);
            form.Controls.Remove(BlueLabel);
            form.Controls.Remove(ColorDialogButton);
            form.Controls.Remove(ColorPreviewPanel);
            _colorDialog.Dispose();
            if (AlphaLabel != null) form.Controls.Remove(AlphaLabel);
        }

        private void UpdateSlidersFromColor(Color color)
        {
            RedSlider.Value = color.R;
            GreenSlider.Value = color.G;
            BlueSlider.Value = color.B;
            ColorPreviewPanel.BackColor = color;
            if (AlphaSlider != null)
                AlphaSlider.Value = color.A;
        }
    }
}
