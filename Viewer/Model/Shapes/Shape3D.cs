using Viewer.Model.Geometry;
using Viewer.Model.Geometry.Viewer.Model.Geometry;

namespace Viewer.Model.Shapes
{
    public abstract class Shape3D
    {
        public Vertex[] Vertices { get; protected set; }
        public Edge[] Edges { get; protected set; }
        public Face[] Faces { get; protected set; }
    }
}
