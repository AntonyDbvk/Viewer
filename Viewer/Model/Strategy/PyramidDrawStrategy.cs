using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model.Geometry;

namespace Viewer.Model.Strategy
{
    public class PyramidDrawStrategy : IDrawStrategy
    {
        private Vertex[] _vertices;
        private Edge[] _edges;

        public PyramidDrawStrategy(Vertex[] vertices, Edge[] edges)
        {
            _vertices = vertices;
            _edges = edges;
        }

        public void Draw(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            foreach (var edge in _edges)
            {
                PointF p1 = camera.Project(_vertices[edge.Start].X, _vertices[edge.Start].Y, _vertices[edge.Start].Z, clientSize, isOrthogonal);
                PointF p2 = camera.Project(_vertices[edge.End].X, _vertices[edge.End].Y, _vertices[edge.End].Z, clientSize, isOrthogonal);

                g.DrawLine(settings.EdgePen, p1, p2); 
            }
        }
    }

}
