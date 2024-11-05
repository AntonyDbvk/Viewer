using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class TesseractWithFacesDrawStrategy : FaceDrawStrategyBase
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            if (!(model is Tesseract tesseract)) return;

            var outerVertices = tesseract.Vertices;
            var outerEdges = tesseract.Edges;
            var innerVertices = tesseract.InnerVertices;
            var innerEdges = tesseract.Edges;
            var faces = tesseract.Faces;

            ConnectCubes(g, settings, camera, outerVertices, innerVertices, clientSize, isOrthogonal);

            DrawFaces(g, innerVertices, faces, settings.FaceBrush1, camera, clientSize, isOrthogonal);
            DrawEdges(g, innerVertices, innerEdges, settings.EdgePen1, camera, clientSize, isOrthogonal);

            DrawFaces(g, outerVertices, faces, settings.FaceBrush2, camera, clientSize, isOrthogonal);
            DrawEdges(g, outerVertices, outerEdges, settings.EdgePen2, camera, clientSize, isOrthogonal);

        }

        private void ConnectCubes(Graphics g, DrawingSettings settings, Camera camera, Vertex[] outerVertices, Vertex[] innerVertices, Size clientSize, bool isOrthogonal)
        {
            Pen pen = new Pen(Color.Blue, 2);
            for (int i = 0; i < outerVertices.Length; i++)
            {
                PointF outerPoint = camera.Project(outerVertices[i].X, outerVertices[i].Y, outerVertices[i].Z, clientSize, isOrthogonal);
                PointF innerPoint = camera.Project(innerVertices[i].X, innerVertices[i].Y, innerVertices[i].Z, clientSize, isOrthogonal);
                g.DrawLine(pen, outerPoint, innerPoint);
            }
        }
    }
}