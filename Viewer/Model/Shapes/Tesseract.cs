using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Strategy;

namespace Viewer.Model
{
    public class Tesseract : Shape3D
    {
        public Tesseract()
        {
            DrawStrategy = new TesseractDrawStrategy(_outerVertices, _outerEdges, _innerVertices, _innerEdges);
        }

        private Vertex[] _outerVertices = new Vertex[]
        {
        new Vertex(-1, -1, -1), new Vertex(1, -1, -1), new Vertex(1, 1, -1), new Vertex(-1, 1, -1),
        new Vertex(-1, -1, 1), new Vertex(1, -1, 1), new Vertex(1, 1, 1), new Vertex(-1, 1, 1)
        };

        private Edge[] _outerEdges = new Edge[]
        {
        new Edge(0, 1), new Edge(1, 2), new Edge(2, 3), new Edge(3, 0),
        new Edge(4, 5), new Edge(5, 6), new Edge(6, 7), new Edge(7, 4),
        new Edge(0, 4), new Edge(1, 5), new Edge(2, 6), new Edge(3, 7)
        };

        private Vertex[] _innerVertices = new Vertex[]
        {
        new Vertex(-0.5f, -0.5f, -0.5f), new Vertex(0.5f, -0.5f, -0.5f), new Vertex(0.5f, 0.5f, -0.5f), new Vertex(-0.5f, 0.5f, -0.5f),
        new Vertex(-0.5f, -0.5f, 0.5f), new Vertex(0.5f, -0.5f, 0.5f), new Vertex(0.5f, 0.5f, 0.5f), new Vertex(-0.5f, 0.5f, 0.5f)
        };

        private Edge[] _innerEdges = new Edge[]
        {
        new Edge(0, 1), new Edge(1, 2), new Edge(2, 3), new Edge(3, 0),
        new Edge(4, 5), new Edge(5, 6), new Edge(6, 7), new Edge(7, 4),
        new Edge(0, 4), new Edge(1, 5), new Edge(2, 6), new Edge(3, 7)
        };
   
    }
}
