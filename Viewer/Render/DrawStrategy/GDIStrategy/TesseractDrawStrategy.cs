using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.DrawContext;

namespace Viewer.Render.DrawStrategy.GDIStrategy
{
    public class TesseractDrawStrategy : ShapeDrawStrategy
    {
        public override void Draw(IDrawContext context, Shape3D model, DrawingSettings settings, CameraBase gdiCamera, bool isOrthogonal)
        {
            var currentContext = context as GDIDrawContext;
            if (!(model is Tesseract tesseract)) return;
            var outerVertices = tesseract.Vertices;
            var outerEdges = tesseract.Edges;

            var innerVertices = tesseract.InnerVertices;
            var innerEdges = tesseract.Edges;

            ConnectCubes(currentContext.Graphics, settings, (GDICamera)gdiCamera, outerVertices, innerVertices, currentContext.Size, isOrthogonal);
            DrawEdges(currentContext.Graphics, innerVertices, innerEdges, settings.EdgePen1, (GDICamera)gdiCamera, currentContext.Size, isOrthogonal);
            DrawEdges(currentContext.Graphics, outerVertices, outerEdges, settings.EdgePen2, (GDICamera)gdiCamera, currentContext.Size, isOrthogonal);
        }

        private void ConnectCubes(Graphics g, DrawingSettings settings, GDICamera gdiCamera, Vertex[] outerVertices, Vertex[] innerVertices, Size clientSize, bool isOrthogonal)
        {
            Pen pen = new Pen(Color.Black, 2);
            for (int i = 0; i < outerVertices.Length; i++)
            {
                PointF outerPoint = gdiCamera.Project(outerVertices[i].X, outerVertices[i].Y, outerVertices[i].Z, clientSize, isOrthogonal);
                PointF innerPoint = gdiCamera.Project(innerVertices[i].X, innerVertices[i].Y, innerVertices[i].Z, clientSize, isOrthogonal);
                g.DrawLine(pen, outerPoint, innerPoint);
            }
        }
    }
}