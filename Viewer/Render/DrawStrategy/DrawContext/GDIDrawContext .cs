using System;
using System.Drawing;

namespace Viewer.Render.DrawStrategy.DrawContext
{
    public class GDIDrawContext : IDrawContext
    {
        public Graphics Graphics { get; }
        public Size Size { get; }

        public GDIDrawContext(Graphics graphics, Size size)
        {
            Graphics = graphics;
            Size = size;
        }
    }

}