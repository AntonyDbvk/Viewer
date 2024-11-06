using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Viewer.Render;
using Viewer.Resources.Localization;

namespace Viewer.UIComponents
{
    public class ColorSliderManager
    {
        private readonly Form _form;
        private readonly Panel _panel;
        private readonly Panel _drawPanel;
        private readonly Localizer _localizer;
        private readonly List<ColorSliderGroup> _edgeSliderGroups;
        private readonly List<ColorSliderGroup> _faceSliderGroups;

        public ColorSliderManager(Form form, Panel panel, Panel drawPanel)
        {
            _form = form;
            _localizer = Localizer.Instance();
            _edgeSliderGroups = new List<ColorSliderGroup>();
            _faceSliderGroups = new List<ColorSliderGroup>();
            _form.Resize += (s, e) => RepositionSliders();
            _panel = panel;
            _drawPanel = drawPanel;
        }

        public void InitializeSliders(bool isTesseract, bool hasFaces)
        {
            ClearSliders();
            var (withoutAlpha, withAlpha) = GetSliderGroupCounts(isTesseract, hasFaces);

            CreateSliderGroups(_edgeSliderGroups, withoutAlpha, false, _localizer.GetString("EdgeColor"));
            CreateSliderGroups(_faceSliderGroups, withAlpha, true, _localizer.GetString("FaceColor"));

            SetSliderValuesFromSettings();
            RepositionSliders();
        }

        private void SetSliderValuesFromSettings()
        {
            if (_edgeSliderGroups.Count > 0)
                SetSliderGroupValues(_edgeSliderGroups[0], DrawingSettings.Instance.EdgePen1.Color);

            if (_edgeSliderGroups.Count > 1)
                SetSliderGroupValues(_edgeSliderGroups[1], DrawingSettings.Instance.EdgePen2.Color);

            if (_faceSliderGroups.Count > 0)
                SetSliderGroupValues(_faceSliderGroups[0], ((SolidBrush)DrawingSettings.Instance.FaceBrush1).Color);

            if (_faceSliderGroups.Count > 1)
                SetSliderGroupValues(_faceSliderGroups[1], ((SolidBrush)DrawingSettings.Instance.FaceBrush2).Color);
        }

        private void SetSliderGroupValues(ColorSliderGroup sliderGroup, Color color)
        {
            sliderGroup.RedSlider.Value = color.R;
            sliderGroup.GreenSlider.Value = color.G;
            sliderGroup.BlueSlider.Value = color.B;
            if (sliderGroup.AlphaSlider != null)
                sliderGroup.AlphaSlider.Value = color.A;

            sliderGroup.ColorPreviewPanel.BackColor = color;
        }

        private void CreateSliderGroups(List<ColorSliderGroup> sliderGroups, int count, bool includeAlpha, string labelBaseText)
        {
            for (int i = 0; i < count; i++)
            {
                var sliderGroup = new ColorSliderGroup(includeAlpha, $"{labelBaseText} {i + 1}",_localizer.GetString("Color") );
                sliderGroup.AddToForm(_panel);
                sliderGroups.Add(sliderGroup);
                sliderGroup.ColorChanged += OnSliderColorChanged;
            }
        }


        private void ClearSliders()
        {
            ClearSliderGroups(_edgeSliderGroups);
            ClearSliderGroups(_faceSliderGroups);
        }

        private void ClearSliderGroups(List<ColorSliderGroup> sliderGroups)
        {
            foreach (var group in sliderGroups)
            {
                group.Remove(_panel);
            }
            sliderGroups.Clear();
        }

        private (int withoutAlpha, int withAlpha) GetSliderGroupCounts(bool isTesseract, bool hasFaces)
        {
            return isTesseract ? (hasFaces ? (2, 2) : (2, 0)) : (hasFaces ? (1, 1) : (1, 0));
        }


        private void OnSliderColorChanged(object sender, Color color)
        {
            var sliderGroup = (ColorSliderGroup)sender;

            if (_edgeSliderGroups.Contains(sliderGroup))
            {
                if (_edgeSliderGroups.IndexOf(sliderGroup) == 0) 
                    DrawingSettings.Instance.EdgePen1.Color = color;
                else 
                    DrawingSettings.Instance.EdgePen2.Color = color;
            }
            else if (_faceSliderGroups.Contains(sliderGroup))
            {
                if (_faceSliderGroups.IndexOf(sliderGroup) == 0) 
                    DrawingSettings.Instance.FaceBrush1 = new SolidBrush(color);
                else
                    DrawingSettings.Instance.FaceBrush2 = new SolidBrush(color);
            }

            _panel.Invalidate();
            _drawPanel.Invalidate();
        }

        private void RepositionSliders()
        {
            int edgeXOffset = _panel.ClientSize.Width - 130; // правый край для рёбер
            int faceXOffset = _panel.ClientSize.Width - 310; // левее для граней
            int initialY = 40;
            int groupSpacing = 280;
            Color color = Color.BlueViolet;

            PositionSliderGroup(edgeXOffset, initialY, groupSpacing, _edgeSliderGroups);
            PositionSliderGroup(faceXOffset, initialY, groupSpacing, _faceSliderGroups);
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
