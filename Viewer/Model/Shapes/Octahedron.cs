using Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public class Octahedron : Shape3D
    {
        public Octahedron()
        {
            Vertices = new[] 
            {
                new Vertex(0, 1, 0), new Vertex(0, -1, 0),
                new Vertex(1, 0, 0), new Vertex(-1, 0, 0),
                new Vertex(0, 0, 1), new Vertex(0, 0, -1)
            };

            Edges = new[] 
            {
                new Edge(0, 2), new Edge(0, 3), new Edge(0, 4), new Edge(0, 5),
                new Edge(1, 2), new Edge(1, 3), new Edge(1, 4), new Edge(1, 5),
                new Edge(2, 4), new Edge(4, 3), new Edge(3, 5), new Edge(5, 2)
            };
        }
    }

}
