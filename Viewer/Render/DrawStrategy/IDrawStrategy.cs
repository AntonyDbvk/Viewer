using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.DrawContext;

namespace Viewer.Render.DrawStrategy
{
    public interface IDrawStrategy
    {
        void Draw(IDrawContext context, Shape3D model, DrawingSettings settings, CameraBase gdiCamera, bool isOrthogonal);
    }
}
