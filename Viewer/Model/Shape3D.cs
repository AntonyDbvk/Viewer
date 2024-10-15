using System.Drawing;

namespace Viewer.Model
{
    public abstract class Shape3D
    {
        public abstract void Draw(Graphics g, Pen pen, Camera camera, Size clientSize);
    }
}
