using System;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer
{
    public partial class Form1 : Form
    {

        private float[,] outerCubeVertices = new float[,]
        {
            { -1, -1, -1 }, {  1, -1, -1 }, {  1,  1, -1 }, { -1,  1, -1 },
            { -1, -1,  1 }, {  1, -1,  1 }, {  1,  1,  1 }, { -1,  1,  1 }
        };


        private float[,] innerCubeVertices = new float[,]
        {
            { -0.5f, -0.5f, -0.5f }, {  0.5f, -0.5f, -0.5f }, {  0.5f,  0.5f, -0.5f }, { -0.5f,  0.5f, -0.5f },
            { -0.5f, -0.5f,  0.5f }, {  0.5f, -0.5f,  0.5f }, {  0.5f,  0.5f,  0.5f }, { -0.5f,  0.5f,  0.5f }
        };

        private int[,] cubeEdges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        private float cameraAngleX = 0;
        private float cameraAngleY = 0;
        private float cameraDistance = 5f;

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

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            startPosition = e.Location;
            isDragging = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                currentPosition = e.Location;
                cameraAngleY -= (currentPosition.X - startPosition.X) * 0.01f;


                cameraAngleX -= (currentPosition.Y - startPosition.Y) * 0.01f;
                cameraAngleX = Math.Max(minCameraAngleX, Math.Min(maxCameraAngleX, cameraAngleX));

                startPosition = currentPosition;
                Invalidate(); 
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private PointF Project(float x, float y, float z)
        {
            float cosX = (float)Math.Cos(cameraAngleX);
            float sinX = (float)Math.Sin(cameraAngleX);
            float cosY = (float)Math.Cos(cameraAngleY);
            float sinY = (float)Math.Sin(cameraAngleY);

            float dx = x * cosY - z * sinY;
            float dz = x * sinY + z * cosY;
            float dy = y * cosX - dz * sinX;
            dz = y * sinX + dz * cosX;

            float distance = cameraDistance;
            float factor = distance / (distance - dz);
            float projectedX = dx * factor * 100 + this.ClientSize.Width / 2;
            float projectedY = dy * factor * 100 + this.ClientSize.Height / 2;
            return new PointF(projectedX, projectedY);
        }


        private void DrawScene(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black, 2);
            Pen innerPen = new Pen(Color.Red, 2);

            DrawCube(g, pen, outerCubeVertices);
            DrawCube(g, innerPen, innerCubeVertices);


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


                g.DrawLine(pen, p1, p2);
            }
        }

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

                g.DrawLine(pen, p1, p2);
            }
        }
    }
}
