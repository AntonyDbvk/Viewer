using System;
using System.Drawing;
using System.Windows.Forms;

namespace Viewer
{
    public partial class Form1 : Form
    {
        private ViewerViewModel viewModel;
        private bool isDragging = false;
        private Point startPosition;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(DrawScene);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseUp += new MouseEventHandler(OnMouseUp);

            viewModel = new ViewerViewModel();
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
                Point currentPosition = e.Location;
                viewModel.Camera.UpdateAngles(currentPosition.X - startPosition.X, currentPosition.Y - startPosition.Y);
                startPosition = currentPosition;
                Invalidate(); // перерисовываем
            }
        }

 
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        // отрисовка сцены
        private void DrawScene(object sender, PaintEventArgs e)
        {
            viewModel.DrawCubes(e.Graphics, this.ClientSize); // отрисовка кубов через ViewModel
        }
    }
}
