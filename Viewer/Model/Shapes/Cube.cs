using Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public class Cube : Shape3D
    {
        public Cube()
        {

            Vertices = new[]
            {
                new Vertex(-1, -1, -1), new Vertex(1, -1, -1), new Vertex(1, 1, -1), new Vertex(-1, 1, -1),
                new Vertex(-1, -1, 1), new Vertex(1, -1, 1), new Vertex(1, 1, 1), new Vertex(-1, 1, 1)
            };

            Edges = new[]
            {
            new Edge(0, 1), new Edge(1, 2), new Edge(2, 3), new Edge(3, 0),
            new Edge(4, 5), new Edge(5, 6), new Edge(6, 7), new Edge(7, 4),
            new Edge(0, 4), new Edge(1, 5), new Edge(2, 6), new Edge(3, 7)
            };
        }
    }
}


