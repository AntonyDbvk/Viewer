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
                Dock = DockStyle.Fill
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

        public void AddToForm(TableLayoutPanel parent, bool includeAlpha, int index)
        {
            int baseColumn = includeAlpha ? 0 : 2;
            int baseRow = index * 7;

            parent.Controls.Add(GroupLabel, baseColumn + 1, baseRow);

            AddSliderWithLabel(parent, RedLabel, RedSlider, baseColumn, baseRow + 1);
            AddSliderWithLabel(parent, GreenLabel, GreenSlider, baseColumn, baseRow + 2);
            AddSliderWithLabel(parent, BlueLabel, BlueSlider, baseColumn, baseRow + 3);

            if (AlphaSlider != null)
            {
                AddSliderWithLabel(parent, AlphaLabel, AlphaSlider, baseColumn, baseRow + 4);
            }

            parent.Controls.Add(ColorDialogButton, baseColumn + 1, baseRow + 5);
            parent.Controls.Add(ColorPreviewPanel, baseColumn + 1, baseRow + 6);
        }

        private void AddSliderWithLabel(TableLayoutPanel parent, Label label, TrackBar slider, int column, int row)
        {
            parent.Controls.Add(label, column, row);
            parent.Controls.Add(slider, column + 1, row);
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
