using System;
using System.Drawing;

namespace Viewer
{
    public class ViewerViewModel
    {
        public CubeModel OuterCube { get; private set; }
        public CubeModel InnerCube { get; private set; }
        public Camera Camera { get; private set; }

        public ViewerViewModel()
        {
            OuterCube = new CubeModel(1f);
            InnerCube = new CubeModel(0.5f);

            Camera = new Camera(5f);
        }

        // отрисовка кубов
        public void DrawCubes(Graphics g, Size clientSize)
        {
            Pen outerPen = new Pen(Color.Black, 2);
            Pen innerPen = new Pen(Color.Red, 2);

            DrawCube(g, outerPen, OuterCube, clientSize);
            DrawCube(g, innerPen, InnerCube, clientSize);

            // соединяем вершины внешнего и внутреннего кубов
            for (int i = 0; i < 8; i++)
            {
                PointF p1 = Camera.Project(
                    OuterCube.Vertices[i, 0],
                    OuterCube.Vertices[i, 1],
                    OuterCube.Vertices[i, 2],
                    clientSize);

                PointF p2 = Camera.Project(
                    InnerCube.Vertices[i, 0],
                    InnerCube.Vertices[i, 1],
                    InnerCube.Vertices[i, 2],
                    clientSize);

                g.DrawLine(outerPen, p1, p2); // линии между кубами
            }
        }

        private void DrawCube(Graphics g, Pen pen, CubeModel cube, Size clientSize)
        {
            for (int i = 0; i < CubeModel.Edges.GetLength(0); i++)
            {
                int index1 = CubeModel.Edges[i, 0];
                int index2 = CubeModel.Edges[i, 1];

                PointF p1 = Camera.Project(
                    cube.Vertices[index1, 0],
                    cube.Vertices[index1, 1],
                    cube.Vertices[index1, 2],
                    clientSize);

                PointF p2 = Camera.Project(
                    cube.Vertices[index2, 0],
                    cube.Vertices[index2, 1],
                    cube.Vertices[index2, 2],
                    clientSize);

                g.DrawLine(pen, p1, p2); // рисуем ребро куба
            }
        }
    }
}
