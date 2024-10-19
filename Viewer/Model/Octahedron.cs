using System.Drawing;
using Viewer.Model;
using Viewer.Model.Geometry;
using Viewer.Model.Strategy;

namespace Viewer.Model
{
    public class Octahedron : Shape3D
    {
        private Vertex[] _vertices = new Vertex[]
        {
        new Vertex(0, 1, 0), new Vertex(0, -1, 0),
        new Vertex(1, 0, 0), new Vertex(-1, 0, 0),
        new Vertex(0, 0, 1), new Vertex(0, 0, -1)
        };

        private Edge[] _edges = new Edge[]
        {
        new Edge(0, 2), new Edge(0, 3), new Edge(0, 4), new Edge(0, 5),
        new Edge(1, 2), new Edge(1, 3), new Edge(1, 4), new Edge(1, 5),
        new Edge(2, 4), new Edge(4, 3), new Edge(3, 5), new Edge(5, 2)
        };

        public Octahedron()
        {
            DrawStrategy = new OctahedronDrawStrategy(_vertices, _edges);
        }
    }

}
