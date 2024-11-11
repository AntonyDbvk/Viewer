using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class TesseractWithFacesDrawStrategy : FaceDrawStrategyBase
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, GDICamera gdiCamera, Size clientSize, bool isOrthogonal)
        {
            if (!(model is Tesseract tesseract)) return;

            var outerVertices = tesseract.Vertices;
            var outerEdges = tesseract.Edges;
            var innerVertices = tesseract.InnerVertices;
            var innerEdges = tesseract.Edges;
            var faces = tesseract.Faces;

            ConnectCubes(g, settings, gdiCamera, outerVertices, innerVertices, clientSize, isOrthogonal);

            DrawFaces(g, innerVertices, faces, settings.FaceBrush1, gdiCamera, clientSize, isOrthogonal);
            DrawEdges(g, innerVertices, innerEdges, settings.EdgePen1, gdiCamera, clientSize, isOrthogonal);

            DrawFaces(g, outerVertices, faces, settings.FaceBrush2, gdiCamera, clientSize, isOrthogonal);
            DrawEdges(g, outerVertices, outerEdges, settings.EdgePen2, gdiCamera, clientSize, isOrthogonal);

        }

        private void ConnectCubes(Graphics g, DrawingSettings settings, GDICamera gdiCamera, Vertex[] outerVertices, Vertex[] innerVertices, Size clientSize, bool isOrthogonal)
        {
            Pen pen = new Pen(Color.Blue, 2);
            for (int i = 0; i < outerVertices.Length; i++)
            {
                PointF outerPoint = gdiCamera.Project(outerVertices[i].X, outerVertices[i].Y, outerVertices[i].Z, clientSize, isOrthogonal);
                PointF innerPoint = gdiCamera.Project(innerVertices[i].X, innerVertices[i].Y, innerVertices[i].Z, clientSize, isOrthogonal);
                g.DrawLine(pen, outerPoint, innerPoint);
            }
        }
    }
}