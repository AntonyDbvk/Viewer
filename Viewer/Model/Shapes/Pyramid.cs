using Viewer.Model.Geometry;
using Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public class Pyramid : Shape3D
    {
        public Pyramid()
        {
            Vertices = new[]
            {
                new Vertex(0, -1, 0),
                new Vertex(-1, 1, -1), new Vertex(1, 1, -1), new Vertex(1, 1, 1), new Vertex(-1, 1, 1)
            };

            Edges = new[]
            {
                new Edge(0, 1), new Edge(0, 2), new Edge(0, 3), new Edge(0, 4),
                new Edge(1, 2), new Edge(2, 3), new Edge(3, 4), new Edge(4, 1)
            };

            Faces = new[]
            {
                new Face(0, 1, 2), 
                new Face(0, 2, 3),
                new Face(0, 3, 4), 
                new Face(0, 4, 1),
                new Face(1, 2, 3, 4) 
            };
        }
    }

}
