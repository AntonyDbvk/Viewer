using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model.Geometry
{
    public struct Vertex
    {
        public float X, Y, Z;
        public Vertex(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }
}
