using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model.Geometry
{
    public struct Edge
    {
        public int Start, End;
        public Edge(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
