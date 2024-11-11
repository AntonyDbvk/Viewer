using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class ShapeWithFacesStrategy : FaceDrawStrategyBase
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, GDICamera gdiCamera, Size clientSize, bool isOrthogonal)
        {
            var vertices = model.Vertices;
            var edges = model.Edges;
            var faces = model.Faces;

            DrawFaces(g, vertices, faces, settings.FaceBrush1, gdiCamera, clientSize, isOrthogonal);
            DrawEdges(g, vertices, edges, settings.EdgePen1, gdiCamera, clientSize, isOrthogonal);
        }
    }
}