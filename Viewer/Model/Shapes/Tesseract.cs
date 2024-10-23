using Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public class Tesseract : Shape3D
    {
        public Vertex[] InnerVertices { get; protected set; }

        public Edge[] InnerEdges { get; protected set; }

        public Tesseract()
        {
            Vertices = new[]
            {
                new Vertex(-1, -1, -1), new Vertex(1, -1, -1), new Vertex(1, 1, -1), new Vertex(-1, 1, -1),
                new Vertex(-1, -1, 1), new Vertex(1, -1, 1), new Vertex(1, 1, 1), new Vertex(-1, 1, 1)
            };

            InnerVertices = new[]
            {
                new Vertex(-0.5f, -0.5f, -0.5f), new Vertex(0.5f, -0.5f, -0.5f), new Vertex(0.5f, 0.5f, -0.5f), new Vertex(-0.5f, 0.5f, -0.5f),
                new Vertex(-0.5f, -0.5f, 0.5f), new Vertex(0.5f, -0.5f, 0.5f), new Vertex(0.5f, 0.5f, 0.5f), new Vertex(-0.5f, 0.5f, 0.5f)
            };

            Edges = new[]
            {
                new Edge(0, 1), new Edge(1, 2), new Edge(2, 3), new Edge(3, 0),
                new Edge(4, 5), new Edge(5, 6), new Edge(6, 7), new Edge(7, 4),
                new Edge(0, 4), new Edge(1, 5), new Edge(2, 6), new Edge(3, 7)
            };

            InnerEdges = new[]
            {
                new Edge(0, 1), new Edge(1, 2), new Edge(2, 3), new Edge(3, 0),
                new Edge(4, 5), new Edge(5, 6), new Edge(6, 7), new Edge(7, 4),
                new Edge(0, 4), new Edge(1, 5), new Edge(2, 6), new Edge(3, 7)
            };
        }

    }
}
