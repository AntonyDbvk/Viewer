using System.Drawing;
using Viewer.Model;

namespace Viewer.Model
{
    public class Pyramid : Shape3D
    {
        private float[,] _vertices = new float[,]
        {
            { 0, -1, 0 },    
            { -1, 1, -1 }, { 1, 1, -1 }, { 1, 1, 1 }, { -1, 1, 1 }  
        };

        private int[,] _edges = new int[,]
        {
            { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 },
            { 1, 2 }, { 2, 3 }, { 3, 4 }, { 4, 1 }
        };

        public override void Draw(Graphics g, Pen pen, Camera camera, Size clientSize, bool isOrthogonal)
        {
            for (int i = 0; i < _edges.GetLength(0); i++)
            {
                int index1 = _edges[i, 0];
                int index2 = _edges[i, 1];

                float x1 = _vertices[index1, 0];
                float y1 = _vertices[index1, 1];
                float z1 = _vertices[index1, 2];

                float x2 = _vertices[index2, 0];
                float y2 = _vertices[index2, 1];
                float z2 = _vertices[index2, 2];

                PointF p1 = camera.Project(x1, y1, z1, clientSize, isOrthogonal);
                PointF p2 = camera.Project(x2, y2, z2, clientSize, isOrthogonal);

                g.DrawLine(pen, p1, p2);
            }
        }
    }
}
