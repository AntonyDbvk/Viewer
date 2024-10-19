using System.Drawing;
using Viewer.Model;
using Viewer.Model.Geometry;
using Viewer.Model.Strategy;

namespace Viewer.Model
{
    public class Pyramid : Shape3D
    {
        private Vertex[] _vertices = new Vertex[]
        {
        new Vertex(0, -1, 0),
        new Vertex(-1, 1, -1), new Vertex(1, 1, -1), new Vertex(1, 1, 1), new Vertex(-1, 1, 1)
        };

        private Edge[] _edges = new Edge[]
        {
        new Edge(0, 1), new Edge(0, 2), new Edge(0, 3), new Edge(0, 4),
        new Edge(1, 2), new Edge(2, 3), new Edge(3, 4), new Edge(4, 1)
        };

        public Pyramid()
        {
            DrawStrategy = new PyramidDrawStrategy(_vertices, _edges);
        }
    }

}
