using System.Drawing;
namespace Viewer.Render
{
    public sealed class DrawingSettings
    {
        public Pen EdgePen1 { get; set; }
        public Pen EdgePen2 { get; set; }
        public SolidBrush FaceBrush1 { get; set; }
        public SolidBrush FaceBrush2 { get; set; }

        private DrawingSettings()
        {
            EdgePen1 = new Pen(Color.Black, 3);
            EdgePen2 = new Pen(Color.Black, 3);
            FaceBrush1 = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            FaceBrush2 = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
        }

        public static DrawingSettings Instance { get; } = new DrawingSettings();
    }

}