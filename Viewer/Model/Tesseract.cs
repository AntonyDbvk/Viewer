using System.Drawing;
using Viewer.Model;

namespace Viewer.Model
{
    public class Tesseract : Shape3D
    {
        // координаты вершин внешнего куба
        private float[,] _outerVertices = new float[,]
        {
            { -1, -1, -1 }, {  1, -1, -1 }, {  1,  1, -1 }, { -1,  1, -1 },
            { -1, -1,  1 }, {  1, -1,  1 }, {  1,  1,  1 }, { -1,  1,  1 }
        };

        // координаты вершин внутреннего куба
        private float[,] _innerVertices = new float[,]
        {
            { -0.5f, -0.5f, -0.5f }, {  0.5f, -0.5f, -0.5f }, {  0.5f,  0.5f, -0.5f }, { -0.5f,  0.5f, -0.5f },
            { -0.5f, -0.5f,  0.5f }, {  0.5f, -0.5f,  0.5f }, {  0.5f,  0.5f,  0.5f }, { -0.5f,  0.5f,  0.5f }
        };

        // ребра кубов
        private int[,] _edges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        public override void Draw(Graphics g, Pen pen, Camera camera, Size clientSize, bool isOrthogonal)
        {
            DrawCube(g, pen, _outerVertices, camera, clientSize, isOrthogonal);

            // отрисовка внутреннего куба с красным цветом
            Pen innerPen = new Pen(Color.Red, 2);
            DrawCube(g, innerPen, _innerVertices, camera, clientSize, isOrthogonal);

            // соединяем вершины внешнего и внутреннего куба
            for (int i = 0; i < 8; i++)
            {
                float x1 = _outerVertices[i, 0];
                float y1 = _outerVertices[i, 1];
                float z1 = _outerVertices[i, 2];

                float x2 = _innerVertices[i, 0];
                float y2 = _innerVertices[i, 1];
                float z2 = _innerVertices[i, 2];

                PointF p1 = camera.Project(x1, y1, z1, clientSize, isOrthogonal);
                PointF p2 = camera.Project(x2, y2, z2, clientSize, isOrthogonal);

                g.DrawLine(pen, p1, p2); // линии между точками
            }
        }

        // функция для отрисовки куба
        private void DrawCube(Graphics g, Pen pen, float[,] vertices, Camera camera, Size clientSize,bool isOrthogonal)
        {
            for (int i = 0; i < _edges.GetLength(0); i++)
            {
                int index1 = _edges[i, 0];
                int index2 = _edges[i, 1];

                float x1 = vertices[index1, 0];
                float y1 = vertices[index1, 1];
                float z1 = vertices[index1, 2];

                float x2 = vertices[index2, 0];
                float y2 = vertices[index2, 1];
                float z2 = vertices[index2, 2];

                PointF p1 = camera.Project(x1, y1, z1, clientSize, isOrthogonal);
                PointF p2 = camera.Project(x2, y2, z2, clientSize, isOrthogonal);

                g.DrawLine(pen, p1, p2);  // ребро куба
            }
        }
    }
}
