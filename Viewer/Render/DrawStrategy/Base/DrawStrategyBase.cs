using System.Drawing;
using Viewer.Model;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;

namespace Viewer.Render.DrawStrategy.Base
{
    public abstract class DrawStrategyBase : IDrawStrategy
    {
        public abstract void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal);

        protected void DrawEdges(Graphics g, Vertex[] vertices, Edge[] edges, Pen pen, Camera camera, Size clientSize, bool isOrthogonal)
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