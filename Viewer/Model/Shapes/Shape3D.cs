using Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public abstract class Shape3D
    {
        public Vertex[] Vertices { get; protected set; }
        public Edge[] Edges { get; protected set; }
    }
}
