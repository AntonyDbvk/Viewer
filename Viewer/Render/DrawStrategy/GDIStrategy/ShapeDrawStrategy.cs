using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.GDIStrategy.Base;

namespace Viewer.Render.DrawStrategy.GDIStrategy
{
    public class ShapeDrawStrategy : DrawStrategyBase
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, GDICamera gdiCamera, Size clientSize, bool isOrthogonal)
        {
            var vertices = model.Vertices;
            var edges = model.Edges;

            DrawEdges(g, vertices, edges, settings.EdgePen1, gdiCamera, clientSize, isOrthogonal);
        }
    }
}