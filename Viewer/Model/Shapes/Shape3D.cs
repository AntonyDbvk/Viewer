using System.Drawing;
using Viewer.Model.Strategy;

namespace Viewer.Model
{
    public abstract class Shape3D
    {
        public IDrawStrategy DrawStrategy { get; set; }
        public void Draw(Graphics g, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            DrawStrategy?.Draw(g, settings, camera, clientSize, isOrthogonal);
        }
    }
}
