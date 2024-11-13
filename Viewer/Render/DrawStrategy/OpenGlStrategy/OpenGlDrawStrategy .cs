using OpenTK;
using OpenTK.Graphics.OpenGL;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.OpenGlStrategy.Base;

namespace Viewer.Render.DrawStrategy.OpenGlStrategy
{
    public sealed class OpenGlShapeDrawStrategy : OpenGlDrawStrategyBase
    {
        protected override void DrawShape(Shape3D shape)
        {
            DrawingSettings settings = DrawingSettings.Instance;
            DrawEdges(shape.Edges, shape.Vertices, settings.EdgePen1.Color);
        }
    }
}