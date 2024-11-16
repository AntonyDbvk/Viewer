using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.DrawContext;
using Viewer.Render.DrawStrategy.GDIStrategy.Base;

namespace Viewer.Render.DrawStrategy.GDIStrategy
{
    public class ShapeWithFacesStrategy : FaceDrawStrategyBase
    {
        public override void Draw(IDrawContext context, Shape3D model, DrawingSettings settings, CameraBase gdiCamera, bool isOrthogonal)
        {
            var currentContext = context as GDIDrawContext;
            var vertices = model.Vertices;
            var edges = model.Edges;
            var faces = model.Faces;

            DrawFaces(currentContext.Graphics, vertices, faces, settings.FaceBrush1, (GDICamera)gdiCamera, currentContext.Size, isOrthogonal);
            DrawEdges(currentContext.Graphics, vertices, edges, settings.EdgePen1, (GDICamera)gdiCamera, currentContext.Size, isOrthogonal);
        }
    }
}