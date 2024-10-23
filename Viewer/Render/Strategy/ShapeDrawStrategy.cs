using System.Drawing;
using Viewer.Model;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;

namespace Viewer.Render.Strategy
{
    public class ShapeDrawStrategy : IDrawStrategy
    {
        public virtual void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            var vertices = model.Vertices;
            var edges = model.Edges;

            DrawEdges(g, vertices, edges, settings.EdgePen, camera, clientSize, isOrthogonal);
        }

        public void DrawEdges(Graphics g, Vertex[] vertices, Edge[] edges, Pen pen, Camera camera, Size clientSize, bool isOrthogonal)
        {
            foreach (var edge in edges)
            {
                PointF p1 = camera.Project(vertices[edge.Start].X, vertices[edge.Start].Y, vertices[edge.Start].Z, clientSize, isOrthogonal);
                PointF p2 = camera.Project(vertices[edge.End].X, vertices[edge.End].Y, vertices[edge.End].Z, clientSize, isOrthogonal);
                g.DrawLine(pen, p1, p2);
            }
        }
    }

}
