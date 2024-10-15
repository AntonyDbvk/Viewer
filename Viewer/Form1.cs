using System;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer
{
    public partial class Form1 : Form
    {
        // координаты вершин внешнего куба
        private float[,] outerCubeVertices = new float[,]
        {
            { -1, -1, -1 }, {  1, -1, -1 }, {  1,  1, -1 }, { -1,  1, -1 },
            { -1, -1,  1 }, {  1, -1,  1 }, {  1,  1,  1 }, { -1,  1,  1 }
        };

        // координаты вершин внутреннего куба
        private float[,] innerCubeVertices = new float[,]
        {
            { -0.5f, -0.5f, -0.5f }, {  0.5f, -0.5f, -0.5f }, {  0.5f,  0.5f, -0.5f }, { -0.5f,  0.5f, -0.5f },
            { -0.5f, -0.5f,  0.5f }, {  0.5f, -0.5f,  0.5f }, {  0.5f,  0.5f,  0.5f }, { -0.5f,  0.5f,  0.5f }
        };

        // ребра кубов
        private int[,] cubeEdges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        // углы камеры
        private float cameraAngleX = 0;
        private float cameraAngleY = 0;
        private float cameraDistance = 5f;

        // ограничение углов обзора
        private float minCameraAngleX = -1.5f; 
        private float maxCameraAngleX = 1.5f;  

        private bool isDragging = false;
        private Point startPosition;
        private Point currentPosition;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(DrawScene);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
        }

        // начало движения мыши
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            startPosition = e.Location;
            isDragging = true;
        }

        // движение мыши — поворот камеры
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                currentPosition = e.Location;
                cameraAngleY -= (currentPosition.X - startPosition.X) * 0.01f;

                
                cameraAngleX -= (currentPosition.Y - startPosition.Y) * 0.01f;
                cameraAngleX = Math.Max(minCameraAngleX, Math.Min(maxCameraAngleX, cameraAngleX));/* ограничиваем нижний и верхний угол для просмотра,
                                                                                                   * дабы избежать непредсказуемого поведения при вращении камеры */

                startPosition = currentPosition;
                Invalidate(); // перерисовываем
            }
        }

 
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        // проекция 3D-координат в 2D
        private PointF Project(float x, float y, float z)
        {
            float cosX = (float)Math.Cos(cameraAngleX);
            float sinX = (float)Math.Sin(cameraAngleX);
            float cosY = (float)Math.Cos(cameraAngleY);
            float sinY = (float)Math.Sin(cameraAngleY);

            // поворачиваем по оси Y
            float dx = x * cosY - z * sinY;
            float dz = x * sinY + z * cosY;

            // поворачиваем по оси X
            float dy = y * cosX - dz * sinX;
            dz = y * sinX + dz * cosX;


            float distance = cameraDistance;
            float factor = distance / (distance - dz);
            float projectedX = dx * factor * 100 + this.ClientSize.Width / 2;
            float projectedY = dy * factor * 100 + this.ClientSize.Height / 2;
            return new PointF(projectedX, projectedY);
        }

        // отрисовка сцены
        private void DrawScene(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black, 2);
            Pen innerPen = new Pen(Color.Red, 2);

            DrawCube(g, pen, outerCubeVertices);
            DrawCube(g, innerPen, innerCubeVertices);

            // соединяем вершины внешнего и внутреннего куба
            for (int i = 0; i < 8; i++)
            {
                float x1 = outerCubeVertices[i, 0];
                float y1 = outerCubeVertices[i, 1];
                float z1 = outerCubeVertices[i, 2];

                float x2 = innerCubeVertices[i, 0];
                float y2 = innerCubeVertices[i, 1];
                float z2 = innerCubeVertices[i, 2];

                PointF p1 = Project(x1, y1, z1);
                PointF p2 = Project(x2, y2, z2);

                g.DrawLine(pen, p1, p2); // линии между точками
            }
        }

        // функция для отрисовки куба
        private void DrawCube(Graphics g, Pen pen, float[,] vertices)
        {
            for (int i = 0; i < cubeEdges.GetLength(0); i++)
            {
                int index1 = cubeEdges[i, 0];
                int index2 = cubeEdges[i, 1];

                float x1 = vertices[index1, 0];
                float y1 = vertices[index1, 1];
                float z1 = vertices[index1, 2];

                float x2 = vertices[index2, 0];
                float y2 = vertices[index2, 1];
                float z2 = vertices[index2, 2];

                PointF p1 = Project(x1, y1, z1);
                PointF p2 = Project(x2, y2, z2);

                g.DrawLine(pen, p1, p2); // ребро куба
            }
        }
    }
}
