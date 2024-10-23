using System;
using System.Drawing;
using System.Windows.Forms;
using Viewer.ViewModel;

namespace Viewer
{
    public sealed partial class Form1 : Form
    {
        private readonly ViewerViewModel _viewModel;
        private bool _isDragging = false;
        private Point _startPosition;
        private ComboBox _shapeSelector;
        private ComboBox _projectionSelector;
        private Button _zoomInButton;
        private Button _zoomOutButton;
        private Button _startStopButton;
        private TrackBar _speedSlider;
        private Timer _autoScrollTimer;
        private bool _isAutoScrolling = false;


        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += OnPaint;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.MouseWheel += OnMouseWheel;
            this.Resize += OnResize;
            _viewModel = new ViewerViewModel();
            InitUi();
        }

        //-----------------ИНИЦИАЛИЗАЦИЯ_UI-----------------

        private void InitUi()
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
            _startStopButton = new Button
            {
                Text = "Start",
                Size = new Size(60, 30)
            };
            _startStopButton.Click += OnStartStopClicked;
            this.Controls.Add(_startStopButton);
            _startStopButton.Anchor = AnchorStyles.Bottom;

        }

        private void InitSpeedSlider()
        {
            _speedSlider = new TrackBar
            {
                Minimum = 0,
                Maximum = 99,
                Value = 50,
                Size = new Size(150, 30)
            };
            _speedSlider.Scroll += OnSpeedSliderChanged;
            this.Controls.Add(_speedSlider);
            _speedSlider.Anchor = AnchorStyles.Bottom;

        }

        private void InitAutoScrollTimer()
        {
            _autoScrollTimer = new Timer
            {
                Interval = 50 // начальная скорость
            };
            _autoScrollTimer.Tick += OnAutoScrollTick;
        }


        private void InitShapeSelector()
        {
            _shapeSelector = new ComboBox();
            _shapeSelector.Location = new Point(10, 10);
            _shapeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            _shapeSelector.Items.AddRange(new object[] { "Тессеракт", "Пирамида", "Октаэдр", "Куб" });
            _shapeSelector.SelectedIndex = 0;
            _shapeSelector.SelectedIndexChanged += OnShapeSelected;
            this.Controls.Add(_shapeSelector);
        }

        private void InitProjectionSelector()
        {
            _projectionSelector = new ComboBox();
            _projectionSelector.Location = new Point(10, 40);
            _projectionSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            _projectionSelector.Items.AddRange(new object[] { "Ортогональная", "Перспективная" });
            _projectionSelector.SelectedIndex = 1;
            _projectionSelector.SelectedIndexChanged += OnProjectionSelected;
            this.Controls.Add(_projectionSelector);
        }

        private void InitZoomButtons()
        {
            _zoomInButton = new Button();
            _zoomInButton.Text = "+";
            _zoomInButton.Size = new Size(40, 40);
            _zoomInButton.Click += OnZoomInClicked;

            _zoomOutButton = new Button();
            _zoomOutButton.Text = "-";
            _zoomOutButton.Size = new Size(40, 40);
            _zoomOutButton.Click += OnZoomOutClicked;

            this.Controls.Add(_zoomInButton);
            this.Controls.Add(_zoomOutButton);

            int margin = 10;

            _zoomInButton.Location = new Point(this.ClientSize.Width - _zoomInButton.Width - margin,
                                              this.ClientSize.Height - _zoomInButton.Height - margin);
            _zoomOutButton.Location = new Point(_zoomInButton.Left - _zoomOutButton.Width - margin,
                                               this.ClientSize.Height - _zoomOutButton.Height - margin);

            // привязка кнопок к нижнему правому краю
            _zoomInButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _zoomOutButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }


        //центрирование кнопки и слайдера
        private void PositionAutoScrollControls()
        {
            int margin = 10;
            int totalWidth = _startStopButton.Width + _speedSlider.Width + margin;


            int centerX = (this.ClientSize.Width - totalWidth) / 2;

            _startStopButton.Location = new Point(centerX, this.ClientSize.Height - _startStopButton.Height - margin);
            _speedSlider.Location = new Point(_startStopButton.Right + margin, _startStopButton.Top);
        }

        //-----------------ОБРАБОТЧИКИ_СОБЫТИЙ-----------------

        private void OnStartStopClicked(object sender, EventArgs e)
        {
            _isAutoScrolling = !_isAutoScrolling;
            _startStopButton.Text = _isAutoScrolling ? "Stop" : "Start";

            if (_isAutoScrolling)
                _autoScrollTimer.Start();
            else
                _autoScrollTimer.Stop();
        }

        private void OnSpeedSliderChanged(object sender, EventArgs e)
        {
            _autoScrollTimer.Interval = 100 - _speedSlider.Value;  // изменение интервала таймера в зависимости от скорости
        }

        private void OnAutoScrollTick(object sender, EventArgs e)
        {
            _viewModel.UpdateCameraRotation(1, 0);  // поворот по оси X
            Invalidate();
        }


        private void OnResize(object sender, EventArgs e)
        {
            Invalidate(); // Перерисовка в соответствии с размерами окна 
        }

        private void OnZoomInClicked(object sender, EventArgs e)
        {
            _viewModel.Zoom(-0.5f);
            Invalidate();
        }

        private void OnZoomOutClicked(object sender, EventArgs e)
        {
            _viewModel.Zoom(0.5f);
            Invalidate();
        }

        private void OnShapeSelected(object sender, EventArgs e)
        {
            int selectedIndex = _shapeSelector.SelectedIndex;
            _viewModel.ChangeShape(selectedIndex);  // изменяем текущую фигуру в ViewModel
            Invalidate();  // обновляем отображение
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // сглаживание при отрисовке
            _viewModel.Draw(e.Graphics, this.ClientSize);
        }

        // начало движения мыши
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _startPosition = e.Location;
            _isDragging = true;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float delta = e.Delta > 0 ? -0.1f : 0.1f; // Уменьшаем или увеличиваем расстояние
            _viewModel.Zoom(delta); // Вызываем метод для изменения расстояния камеры
            Invalidate(); // Перерисовываем
        }


        // движение мыши — поворот камеры
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPosition = e.Location;
                _viewModel.UpdateCameraRotation(currentPosition.X - _startPosition.X, currentPosition.Y - _startPosition.Y);
                _startPosition = currentPosition;
                Invalidate(); // перерисовываем
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void OnProjectionSelected(object sender, EventArgs e)
        {
            _viewModel.IsOrthogonal = _projectionSelector.SelectedIndex == 0;
            Invalidate();
        }
    }
}
