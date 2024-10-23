using System.Drawing;

namespace Viewer.Model
{
    public class DrawingSettings
    {
        public Pen EdgePen { get; set; } = new Pen(Color.Black, 2);
        public Pen InnerPen { get; set; } = new Pen(Color.Red, 2);
    }

}
