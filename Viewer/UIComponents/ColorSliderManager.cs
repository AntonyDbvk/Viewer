using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer.UIComponents
{
    public class ColorSliderManager
    {
        private readonly Form _form;
        private readonly List<ColorSliderGroup> edgeSliderGroups;
        private readonly List<ColorSliderGroup> faceSliderGroups;

        public ColorSliderManager(Form form)
        {
            _form = form;
            edgeSliderGroups = new List<ColorSliderGroup>();
            faceSliderGroups = new List<ColorSliderGroup>();
            _form.Resize += (s, e) => RepositionSliders();
        }

        public void InitializeSliders(bool isTesseract, bool hasFaces)
        {
            ClearSliders();
            var (withoutAlpha, withAlpha) = GetSliderGroupCounts(isTesseract, hasFaces);

            for (int i = 0; i < withoutAlpha; i++)
            {
                string labelText = $"Слайдер для рёбер {i + 1}";
                var edgeSliderGroup = new ColorSliderGroup(includeAlpha: false, labelText);
                edgeSliderGroup.AddToForm(_form);
                edgeSliderGroups.Add(edgeSliderGroup);
                edgeSliderGroup.ColorChanged += OnSliderColorChanged;
            }

            for (int i = 0; i < withAlpha; i++)
            {
                string labelText = $"Слайдер для граней {i + 1}";
                var faceSliderGroup = new ColorSliderGroup(includeAlpha: true, labelText);
                faceSliderGroup.AddToForm(_form);
                faceSliderGroups.Add(faceSliderGroup);
                faceSliderGroup.ColorChanged += OnSliderColorChanged;
            }

            RepositionSliders();
        }

        private void ClearSliders()
        {
            foreach (var group in edgeSliderGroups)
            {
                RemoveGroupFromForm(group);
            }
            foreach (var group in faceSliderGroups)
            {
                RemoveGroupFromForm(group);
            }

            edgeSliderGroups.Clear();
            faceSliderGroups.Clear();
        }

        private void RemoveGroupFromForm(ColorSliderGroup group)
        {
            _form.Controls.Remove(group.RedSlider);
            _form.Controls.Remove(group.GreenSlider);
            _form.Controls.Remove(group.BlueSlider);
            _form.Controls.Remove(group.GroupLabel);
            if (group.AlphaSlider != null)
                _form.Controls.Remove(group.AlphaSlider);
        }

        private (int withoutAlpha, int withAlpha) GetSliderGroupCounts(bool isTesseract, bool hasFaces)
        {
            if (!isTesseract) return hasFaces ? (1, 1) : (1, 0);
            return hasFaces ? (2, 2) : (2, 0);
        }

        private void OnSliderColorChanged(object sender, Color color)
        {
            // Event handler for slider color changes
        }

        private void RepositionSliders()
        {
            int edgeXOffset = _form.ClientSize.Width - 130; // правый край для рёбер
            int faceXOffset = _form.ClientSize.Width - 310; // левее для граней
            int initialY = 40;
            int groupSpacing = 220;

            PositionSliderGroup(edgeXOffset, initialY, groupSpacing, edgeSliderGroups);
            PositionSliderGroup(faceXOffset, initialY, groupSpacing, faceSliderGroups);
        }

        private void PositionSliderGroup(int xOffset, int initialY, int groupSpacing, List<ColorSliderGroup> sliderGroups)
        {
            for (int i = 0; i < sliderGroups.Count; i++)
            {
                Point position = new Point(xOffset, initialY + i * groupSpacing);
                sliderGroups[i].PositionSliders(position);
            }
        }
    }
}
