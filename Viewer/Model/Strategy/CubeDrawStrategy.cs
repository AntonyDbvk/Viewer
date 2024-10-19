using System.Drawing;
using Viewer.Model.Geometry;

namespace Viewer.Model.Strategy
{
    public class CubeDrawStrategy : IDrawStrategy
    {
        public Vertex[] Vertices { get; private set; }  
        public Edge[] Edges { get; private set; }        

        public CubeDrawStrategy(Vertex[] vertices, Edge[] edges)
        {
            Vertices = vertices;
            Edges = edges;
        }

        public void Draw(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            foreach (var edge in Edges)
            {
                PointF p1 = camera.Project(Vertices[edge.Start].X, Vertices[edge.Start].Y, Vertices[edge.Start].Z, clientSize, isOrthogonal);
                PointF p2 = camera.Project(Vertices[edge.End].X, Vertices[edge.End].Y, Vertices[edge.End].Z, clientSize, isOrthogonal);

                g.DrawLine(settings.EdgePen, p1, p2); 
            }
        }
    }
}
