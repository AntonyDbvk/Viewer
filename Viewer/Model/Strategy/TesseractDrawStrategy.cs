using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;

namespace Viewer.Model.Strategy
{
    public class TesseractDrawStrategy : ShapeDrawStrategy
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            var tesseract = model as Tesseract;

            var outerVertices = tesseract.Vertices;
            var outerEdges = tesseract.Edges;

            var innerVertices = tesseract.InnerVertices;
            var innerEdges = tesseract.InnerEdges;

            DrawEdges(g, outerVertices, outerEdges, settings.EdgePen, camera, clientSize, isOrthogonal);
            DrawEdges(g, innerVertices, innerEdges, settings.InnerPen, camera, clientSize, isOrthogonal);
            ConnectCubes(g, settings, camera, outerVertices, innerVertices, clientSize, isOrthogonal);
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
