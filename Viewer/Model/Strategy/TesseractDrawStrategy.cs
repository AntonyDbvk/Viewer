using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model.Geometry;

namespace Viewer.Model.Strategy
{
    public class TesseractDrawStrategy : IDrawStrategy
    {
        private CubeDrawStrategy _outerCubeStrategy;
        private CubeDrawStrategy _innerCubeStrategy;

        public TesseractDrawStrategy(Vertex[] outerVertices, Edge[] outerEdges, Vertex[] innerVertices, Edge[] innerEdges)
        {
            _outerCubeStrategy = new CubeDrawStrategy(outerVertices, outerEdges);
            _innerCubeStrategy = new CubeDrawStrategy(innerVertices, innerEdges);
        }

        public void Draw(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            _outerCubeStrategy.Draw(g, settings, camera, clientSize, isOrthogonal);

            settings.EdgePen = new Pen(Color.Red, 2); 
            _innerCubeStrategy.Draw(g, settings, camera, clientSize, isOrthogonal);

            ConnectCubes(g, settings, camera, clientSize, isOrthogonal);
        }

        private void ConnectCubes(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            settings.EdgePen = new Pen(Color.Blue, 2); 
            for (int i = 0; i < _outerCubeStrategy.Vertices.Length; i++)
            {
                PointF outerPoint = camera.Project(_outerCubeStrategy.Vertices[i].X, _outerCubeStrategy.Vertices[i].Y, _outerCubeStrategy.Vertices[i].Z, clientSize, isOrthogonal);
                PointF innerPoint = camera.Project(_innerCubeStrategy.Vertices[i].X, _innerCubeStrategy.Vertices[i].Y, _innerCubeStrategy.Vertices[i].Z, clientSize, isOrthogonal);
                g.DrawLine(settings.EdgePen, outerPoint, innerPoint);
            }
        }

    }

}
