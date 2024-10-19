using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model.Strategy
{
    public interface IDrawStrategy
    {
        void Draw(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal);
    }

}
