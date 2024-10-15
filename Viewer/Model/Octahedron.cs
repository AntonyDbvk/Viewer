using System.Drawing;
using Viewer.Model;

namespace Viewer.Model
{
    public class Octahedron : Shape3D
    {
        
        private float[,] vertices = new float[,]
        {
            { 0, 1, 0 }, { 0, -1, 0 }, 
            { 1, 0, 0 }, { -1, 0, 0 }, { 0, 0, 1 }, { 0, 0, -1 }  
        };

   
        private int[,] edges = new int[,]
        {
            { 0, 2 }, { 0, 3 }, { 0, 4 }, { 0, 5 },
            { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 },
            { 2, 4 }, { 4, 3 }, { 3, 5 }, { 5, 2 }
        };

        public override void Draw(Graphics g, Pen pen, Camera camera, Size clientSize)
        {
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                int index1 = edges[i, 0];
                int index2 = edges[i, 1];

                float x1 = vertices[index1, 0];
                float y1 = vertices[index1, 1];
                float z1 = vertices[index1, 2];

                float x2 = vertices[index2, 0];
                float y2 = vertices[index2, 1];
                float z2 = vertices[index2, 2];

                PointF p1 = camera.Project(x1, y1, z1, clientSize);
                PointF p2 = camera.Project(x2, y2, z2, clientSize);

                g.DrawLine(pen, p1, p2);
            }
        }
    }
}
