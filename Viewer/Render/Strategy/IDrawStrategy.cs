using System.Drawing;
using Viewer.Model;
using Viewer.Model.Shapes;

namespace Viewer.Render.Strategy
{
    public interface IDrawStrategy
    {
        void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal);
    }

}
