using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Strategy;

namespace Viewer.Model.Shapes
{
    public abstract class Shape3D
    {
        public Vertex[] Vertices { get; protected set; }

        public Edge[] Edges { get; protected set; }
    }
}
