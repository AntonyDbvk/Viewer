using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;

namespace Viewer.Render.DrawStrategy.GDIStrategy.Base
{
    public interface IDrawStrategy
    {
        void Draw(Graphics g, Shape3D model, DrawingSettings settings, GDICamera gdiCamera, Size clientSize, bool isOrthogonal);
    }
}
