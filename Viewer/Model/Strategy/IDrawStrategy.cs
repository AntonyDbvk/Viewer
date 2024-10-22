using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;

namespace Viewer.Model.Strategy
{
    public interface IDrawStrategy
    {
        void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal);
    }

}
