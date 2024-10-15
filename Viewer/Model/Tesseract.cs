using System.Drawing;
using Viewer.Model;

namespace Viewer.Model
{
    public class Tesseract : Shape3D
    {
        // координаты вершин внешнего куба
        private float[,] outerVertices = new float[,]
        {
            { -1, -1, -1 }, {  1, -1, -1 }, {  1,  1, -1 }, { -1,  1, -1 },
            { -1, -1,  1 }, {  1, -1,  1 }, {  1,  1,  1 }, { -1,  1,  1 }
        };

        // координаты вершин внутреннего куба
        private float[,] innerVertices = new float[,]
        {
            { -0.5f, -0.5f, -0.5f }, {  0.5f, -0.5f, -0.5f }, {  0.5f,  0.5f, -0.5f }, { -0.5f,  0.5f, -0.5f },
            { -0.5f, -0.5f,  0.5f }, {  0.5f, -0.5f,  0.5f }, {  0.5f,  0.5f,  0.5f }, { -0.5f,  0.5f,  0.5f }
        };

        // ребра кубов
        private int[,] edges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        public override void Draw(Graphics g, Pen pen, Camera camera, Size clientSize)
        {
            DrawCube(g, pen, outerVertices, camera, clientSize);

            // отрисовка внутреннего куба с красным цветом
            Pen innerPen = new Pen(Color.Red, 2);
            DrawCube(g, innerPen, innerVertices, camera, clientSize);

            // соединяем вершины внешнего и внутреннего куба
            for (int i = 0; i < 8; i++)
            {
                float x1 = outerVertices[i, 0];
                float y1 = outerVertices[i, 1];
                float z1 = outerVertices[i, 2];

                float x2 = innerVertices[i, 0];
                float y2 = innerVertices[i, 1];
                float z2 = innerVertices[i, 2];

                PointF p1 = camera.Project(x1, y1, z1, clientSize);
                PointF p2 = camera.Project(x2, y2, z2, clientSize);

                g.DrawLine(pen, p1, p2); // линии между точками
            }
        }

        // функция для отрисовки куба
        private void DrawCube(Graphics g, Pen pen, float[,] vertices, Camera camera, Size clientSize)
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

                g.DrawLine(pen, p1, p2);  // ребро куба
            }
        }
    }
}
