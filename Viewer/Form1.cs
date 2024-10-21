using System;
using System.Drawing;
using System.IO;
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
        private ComboBox projectionSelector;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button startStopButton;
        private TrackBar speedSlider;
        private Timer autoScrollTimer;
        private bool isAutoScrolling = false;


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

        //-----------------ИНИЦИАЛИЗАЦИЯ_UI-----------------

        private void InitUI()
        {
            InitShapeSelector();
            InitProjectionSelector();
            InitZoomButtons();
            InitAutoScrollControls();
            
        }

        private void InitAutoScrollControls()
        {
            InitStartStopButton();
            InitSpeedSlider();
            InitAutoScrollTimer();
            PositionAutoScrollControls();
        }

        private void InitStartStopButton()
        {
            startStopButton = new Button
            {
                Text = "Start",
                Size = new Size(60, 30)
            };
            startStopButton.Click += OnStartStopClicked;
            this.Controls.Add(startStopButton);
            startStopButton.Anchor = AnchorStyles.Bottom;

        }

        private void InitSpeedSlider()
        {
            speedSlider = new TrackBar
            {
                Minimum = 0,
                Maximum = 99,
                Value = 50,
                Size = new Size(150, 30)
            };
            speedSlider.Scroll += OnSpeedSliderChanged;
            this.Controls.Add(speedSlider);
            speedSlider.Anchor = AnchorStyles.Bottom; 

        }

        private void InitAutoScrollTimer()
        {
            autoScrollTimer = new Timer
            {
                Interval = 50 // начальная скорость
            };
            autoScrollTimer.Tick += OnAutoScrollTick;
        }


        private void InitShapeSelector()
        {
            shapeSelector = new ComboBox();
            shapeSelector.Location = new Point(10, 10);
            shapeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            shapeSelector.Items.AddRange(new object[] { "Тессеракт", "Пирамида", "Октаэдр", "Куб" });
            shapeSelector.SelectedIndex = 0;
            shapeSelector.SelectedIndexChanged += OnShapeSelected;
            this.Controls.Add(shapeSelector);
        }

        private void InitProjectionSelector()
        {
            projectionSelector = new ComboBox();
            projectionSelector.Location = new Point(10, 40);
            projectionSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            projectionSelector.Items.AddRange(new object[] { "Ортогональная", "Перспективная" });
            projectionSelector.SelectedIndex = 1;
            projectionSelector.SelectedIndexChanged += OnProjectionSelected;
            this.Controls.Add(projectionSelector);
        }

        private void InitZoomButtons()
        {
            zoomInButton = new Button();
            zoomInButton.Text = "+";
            zoomInButton.Size = new Size(40, 40);
            zoomInButton.Click += OnZoomInClicked;

            zoomOutButton = new Button();
            zoomOutButton.Text = "-";
            zoomOutButton.Size = new Size(40, 40);
            zoomOutButton.Click += OnZoomOutClicked;

            this.Controls.Add(zoomInButton);
            this.Controls.Add(zoomOutButton);

            int margin = 10;

            zoomInButton.Location = new Point(this.ClientSize.Width - zoomInButton.Width - margin,
                                              this.ClientSize.Height - zoomInButton.Height - margin);
            zoomOutButton.Location = new Point(zoomInButton.Left - zoomOutButton.Width - margin,
                                               this.ClientSize.Height - zoomOutButton.Height - margin);

            // привязка кнопок к нижнему правому краю
            zoomInButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            zoomOutButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }


        //центрирование кнопки и слайдера
        private void PositionAutoScrollControls()
        {
            int margin = 10;
            int totalWidth = startStopButton.Width + speedSlider.Width + margin;


            int centerX = (this.ClientSize.Width - totalWidth) / 2;

            startStopButton.Location = new Point(centerX, this.ClientSize.Height - startStopButton.Height - margin);
            speedSlider.Location = new Point(startStopButton.Right + margin, startStopButton.Top);
        }

        //-----------------ОБРАБОТЧИКИ_СОБЫТИЙ-----------------

        private void OnStartStopClicked(object sender, EventArgs e)
        {
            isAutoScrolling = !isAutoScrolling;
            startStopButton.Text = isAutoScrolling ? "Stop" : "Start";

            if (isAutoScrolling)
                autoScrollTimer.Start();
            else
                autoScrollTimer.Stop();
        }

        private void OnSpeedSliderChanged(object sender, EventArgs e)
        {
            autoScrollTimer.Interval = 100 - speedSlider.Value;  // изменение интервала таймера в зависимости от скорости
        }

        private void OnAutoScrollTick(object sender, EventArgs e)
        {
            viewModel.UpdateCameraRotation(1, 0);  // поворот по оси X
            Invalidate(); 
        }


        private void OnResize(object sender, EventArgs e)
        {
            Invalidate(); // Перерисовка в соответствии с размерами окна 
        }

        private void OnZoomInClicked(object sender, EventArgs e)
        {
            viewModel.Zoom(-0.5f);
            Invalidate();
        }

        private void OnZoomOutClicked(object sender, EventArgs e)
        {
            viewModel.Zoom(0.5f);
            Invalidate();
        }

        private void OnShapeSelected(object sender, EventArgs e)
        {
            int selectedIndex = shapeSelector.SelectedIndex;
            viewModel.ChangeShape(selectedIndex);  // изменяем текущую фигуру в ViewModel
            Invalidate();  // обновляем отображение
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // сглаживание при отрисовке
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

        private void OnProjectionSelected(object sender, EventArgs e)
        {
            viewModel.IsOrthogonal = projectionSelector.SelectedIndex == 0;
            Invalidate();
        }
    }
}
