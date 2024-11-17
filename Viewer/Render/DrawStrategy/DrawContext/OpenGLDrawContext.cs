using OpenTK;
using System.Drawing;

namespace Viewer.Render.DrawStrategy.DrawContext
{
    public class OpenGLDrawContext : IDrawContext
    {
        public GLControl GLControl { get; }
        public Size Size { get; }

        public OpenGLDrawContext(GLControl glControl, Size size)
        {
            GLControl = glControl;
            Size = size;
        }
    }
}