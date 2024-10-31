using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer.UIComponents
{
    public class ColorSliderManager
    {
        private readonly Form _form;
        private readonly List<ColorSliderGroup> _sliderGroups;

        public ColorSliderManager(Form form)
        {
            _form = form;
            _sliderGroups = new List<ColorSliderGroup>();
            _form.Resize += (s, e) => RepositionSliders(); 
        }

        public void InitializeSliders(bool isTesseract, bool hasFaces)
        {
            ClearSliders();
            var (withoutAlpha, withAlpha) = GetSliderGroupCounts(isTesseract, hasFaces);

            for (int i = 0; i < withoutAlpha; i++)
            {
                var edgeSliderGroup = new ColorSliderGroup(includeAlpha: false);
                edgeSliderGroup.AddToForm(_form);
                _sliderGroups.Add(edgeSliderGroup);
                edgeSliderGroup.ColorChanged += OnSliderColorChanged;
            }

            for (int i = 0; i < withAlpha; i++)
            {
                var faceSliderGroup = new ColorSliderGroup(includeAlpha: true);
                faceSliderGroup.AddToForm(_form);
                _sliderGroups.Add(faceSliderGroup);
                faceSliderGroup.ColorChanged += OnSliderColorChanged;
            }

            RepositionSliders();
        }


        private void ClearSliders()
        {
            foreach (var group in _sliderGroups)
            {
                _form.Controls.Remove(group.RedSlider);
                _form.Controls.Remove(group.GreenSlider);
                _form.Controls.Remove(group.BlueSlider);
                if (group.AlphaSlider != null)
                    _form.Controls.Remove(group.AlphaSlider);
            }

            _sliderGroups.Clear();
        }

        private (int withoutAlpha, int withAlpha) GetSliderGroupCounts(bool isTesseract, bool hasFaces)
        {
            if (!isTesseract) return hasFaces ? (1, 1) : (1, 0);
            return hasFaces ? (2, 2) : (2, 0);

        }


        private void OnSliderColorChanged(object sender, Color color)
        {
            // 
        }

        private void RepositionSliders()
        {
            int xOffset = _form.ClientSize.Width - 100; // от правого края
            int initialY = 30; //  сверху
            int groupSpacing = 180; // между группами

            for (int i = 0; i < _sliderGroups.Count; i++)
            {
                Point position = new Point(xOffset, initialY + i * groupSpacing);
                _sliderGroups[i].PositionSliders(position);
            }
        }
    }
}