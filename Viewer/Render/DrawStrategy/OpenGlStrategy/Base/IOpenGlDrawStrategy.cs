using OpenTK;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;

namespace Viewer.Render.DrawStrategy.OpenGlStrategy.Base
{
    public interface IOpenGlDrawStrategy
    {
        void Draw(GLControl glControl, OpenTKCamera camera, Shape3D shape, bool isOrthogonal);
    }
}