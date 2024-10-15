using System;
using System.Drawing;
using System.Windows.Forms;
using Viewer.ViewModel;

namespace Viewer
{
    public partial class Form1 : Form
    {
        private ViewerViewModel viewModel;
        private bool isDragging = false;
        private Point startPosition;
        private ComboBox shapeSelector; 

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(OnPaint);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseWheel += new MouseEventHandler(OnMouseWheel);
            this.Resize += new EventHandler(OnResize);
            viewModel = new ViewerViewModel();
            InitUI(); 
        }

        private void InitUI()
        {
            shapeSelector = new ComboBox();
            shapeSelector.Location = new Point(10, 10);
            shapeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            shapeSelector.Items.Add("Тессеракт");
            shapeSelector.Items.Add("Пирамида");
            shapeSelector.Items.Add("Октаэдр");
            shapeSelector.SelectedIndex = 0; 
            shapeSelector.SelectedIndexChanged += OnShapeSelected;
            this.Controls.Add(shapeSelector);
        }

        private void OnResize(object sender, EventArgs e)
        {
            Invalidate(); // Перерисовка в соответствии с размерами окна 
        }

            private void OnShapeSelected(object sender, EventArgs e)
        {
            int selectedIndex = shapeSelector.SelectedIndex;
            viewModel.ChangeShape(selectedIndex);  // изменяем текущую фигуру в ViewModel
            Invalidate();  // обновляем отображение
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            viewModel.Draw(e.Graphics, this.ClientSize);
        }

        // начало движения мыши
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            startPosition = e.Location;
            isDragging = true;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float delta = e.Delta > 0 ? -0.1f : 0.1f; // Уменьшаем или увеличиваем расстояние
            viewModel.Zoom(delta); // Вызываем метод для изменения расстояния камеры
            Invalidate(); // Перерисовываем
        }


        // движение мыши — поворот камеры
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.Location;
                viewModel.UpdateCameraRotation(currentPosition.X - startPosition.X, currentPosition.Y - startPosition.Y);
                startPosition = currentPosition;
                Invalidate(); // перерисовываем
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
