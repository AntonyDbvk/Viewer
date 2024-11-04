using System;
using System.Drawing;
using System.Windows.Forms;
using Viewer.Resources.Localization;
using Viewer.UIComponents;
using Viewer.ViewModel;

namespace Viewer
{
    public sealed partial class Form1 : Form
    {
        private readonly ViewerViewModel _viewModel;
        private readonly ColorSliderManager _colorSliderManager;
        private readonly Localizer _localizer;
        private bool _isDragging;
        private Point _startPosition;
        private ComboBox _shapeSelector;
        private ComboBox _projectionSelector;
        private Button _zoomInButton;
        private Button _zoomOutButton;
        private Button _startStopButton;
        private TextBox _speedTextBox;
        private Timer _autoScrollTimer;
        private TrackBar _speedSlider;
        private ComboBox _drawStrategySelector;
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _toggleSpeedMenuItem;
        private ToolStripMenuItem _languageMenuItem;
        private ToolStripMenuItem _fileMenuItem;


        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Paint += OnPaint;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
            MouseWheel += OnMouseWheel;
            Resize += OnResize;
            _viewModel = new ViewerViewModel();
            _colorSliderManager = new ColorSliderManager(this);
            _localizer = Localizer.Instance();
            _localizer.CultureChanged += UpdateLocalizedText;
            InitUi();
        }

        //-----------------ИНИЦИАЛИЗАЦИЯ_UI-----------------

        private void InitUi()
        {
            InitMenu();
            InitShapeSelector();
            InitProjectionSelector();
            InitZoomButtons();
            InitAutoScrollControls();
            InitDrawStrategySelector();
            UpdateSlidersBasedOnSelection();
        }

        private void InitAutoScrollControls()
        {
            InitStartStopButton();
            InitSpeedTextBox();
            InitSpeedSlider();
            InitAutoScrollTimer();
            PositionAutoScrollControls();
        }

        private void InitMenu()
        {
            _menuStrip = new MenuStrip();
             _fileMenuItem = new ToolStripMenuItem(_localizer.GetString("Options"));

            _toggleSpeedMenuItem = new ToolStripMenuItem(_localizer.GetString("SpeedInDegrees"), null, (s, e) =>
            {
                _viewModel.ToggleRotationSpeedStrategy();
                _toggleSpeedMenuItem.Checked = !_toggleSpeedMenuItem.Checked;
                InitSpeedSlider();
            })
            {
                Checked = false
            };

             _languageMenuItem = new ToolStripMenuItem(_localizer.GetString("Language"));
            var russianMenuItem = new ToolStripMenuItem("Русский", null, (s, e) => ChangeLanguage("ru-RU"));
            var englishMenuItem = new ToolStripMenuItem("English", null, (s, e) => ChangeLanguage("en-US"));

            _languageMenuItem.DropDownItems.Add(russianMenuItem);
            _languageMenuItem.DropDownItems.Add(englishMenuItem);

            _fileMenuItem.DropDownItems.Add(_toggleSpeedMenuItem);
            _fileMenuItem.DropDownItems.Add(_languageMenuItem);
            _menuStrip.Items.Add(_fileMenuItem);
            Controls.Add(_menuStrip);
        }

        private void ChangeLanguage(string cultureCode)
        {
            _localizer.SetCulture(cultureCode); 
            UpdateLocalizedText(); 
        }


        private void InitSpeedSlider()
        {
            if (_speedSlider == null)
            {
                _speedSlider = new TrackBar
                {
                    Minimum = _viewModel.MinSpeed,
                    Maximum = _viewModel.MaxSpeed,
                    TickFrequency = 10,
                    SmallChange = 1,
                    LargeChange = 10,
                    Value = _viewModel.CurrentSpeed,
                    Size = new Size(200, 45),
                };
                _speedSlider.Scroll += OnSpeedSliderScroll;
                Controls.Add(_speedSlider);
                _speedSlider.Anchor = AnchorStyles.Bottom;
            }
            else
            {
                _speedSlider.Minimum = _viewModel.MinSpeed;
                _speedSlider.Maximum = _viewModel.MaxSpeed;
                _speedSlider.Value = _viewModel.CurrentSpeed > _speedSlider.Value
                    ? _viewModel.MaxSpeed
                    : _viewModel.CurrentSpeed;
            }
        }


        private void InitStartStopButton()
        {
            _startStopButton = new Button
            {
                Size = new Size(60, 30)
            };
            _startStopButton.Click += OnStartStopClicked;
            Controls.Add(_startStopButton);
            _startStopButton.Anchor = AnchorStyles.Bottom;
        }

        private void InitSpeedTextBox()
        {
            _speedTextBox = new TextBox
            {
                Size = new Size(60, 30),
                Text = _viewModel.CurrentSpeed.ToString()
            };
            _speedTextBox.KeyDown += OnSpeedTextBoxKeyDown;
            _speedTextBox.TextChanged += OnSpeedTextChanged;
            Controls.Add(_speedTextBox);
            _speedTextBox.Anchor = AnchorStyles.Bottom;
        }

        private void InitAutoScrollTimer()
        {
            _autoScrollTimer = new Timer
            {
                Interval = 13 // частота тика 
            };
            _autoScrollTimer.Tick += OnAutoScrollTick;
        }

        private void InitShapeSelector()
        {
            _shapeSelector = new ComboBox();
            _shapeSelector.Location = new Point(10, 10 + _menuStrip.Height);
            _shapeSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            _shapeSelector.Items.AddRange(new object[]
            {
                _localizer.GetString("Tesseract"),
                _localizer.GetString("Pyramid"),
                _localizer.GetString("Octahedron"),
                _localizer.GetString("Cube")
            });
            _shapeSelector.SelectedIndex = 0;
            _shapeSelector.SelectedIndexChanged += OnShapeSelected;
            Controls.Add(_shapeSelector);
        }

        private void InitProjectionSelector()
        {
            _projectionSelector = new ComboBox();
            _projectionSelector.Location = new Point(10, 40 + +_menuStrip.Height);
            _projectionSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            _projectionSelector.Items.AddRange(new object[]
            { 
                _localizer.GetString("Orthogonal"), 
                _localizer.GetString("Perspective")
            });
            _projectionSelector.SelectedIndex = 1;
            _projectionSelector.SelectedIndexChanged += OnProjectionSelected;
            Controls.Add(_projectionSelector);
        }

        private void InitDrawStrategySelector()
        {
            _drawStrategySelector = new ComboBox
            {
                Location = new Point(10, 70 + _menuStrip.Height),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _drawStrategySelector.Items.AddRange(new object[]
            {
                _localizer.GetString("WithoutFaces"),
                _localizer.GetString("WithFaces")
            });
            _drawStrategySelector.SelectedIndex = 0;
            _drawStrategySelector.SelectedIndexChanged += OnDrawStrategySelected;
            Controls.Add(_drawStrategySelector);
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

            Controls.Add(_zoomInButton);
            Controls.Add(_zoomOutButton);

            int margin = 10;

            _zoomInButton.Location = new Point(ClientSize.Width - _zoomInButton.Width - margin,
                                              ClientSize.Height - _zoomInButton.Height - margin);
            _zoomOutButton.Location = new Point(_zoomInButton.Left - _zoomOutButton.Width - margin,
                                               ClientSize.Height - _zoomOutButton.Height - margin);

            // привязка кнопок к нижнему правому краю
            _zoomInButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _zoomOutButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }
        //центрирование кнопки и TextBox
        private void PositionAutoScrollControls()
        {
            int margin = 10;
            int buttonWidth = _startStopButton.Width;
            int textBoxWidth = _speedTextBox.Width;
            int sliderWidth = _speedSlider.Width;

            int centerX = (ClientSize.Width - buttonWidth) / 2;
            _startStopButton.Location = new Point(centerX, ClientSize.Height - _startStopButton.Height - margin);

            _speedTextBox.Location = new Point(_startStopButton.Left - textBoxWidth - margin, _startStopButton.Top);

            _speedSlider.Location = new Point(centerX - (sliderWidth / 2), _startStopButton.Top - _speedSlider.Height - margin);
        }

        //-----------------ОБРАБОТЧИКИ_СОБЫТИЙ-----------------

        private void OnSpeedTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = null;
                e.Handled = true;
            }
        }

        private void OnSpeedSliderScroll(object sender, EventArgs e)
        {
            _speedTextBox.Text = _speedSlider.Value.ToString();
            _viewModel.UpdateSpeed(_speedSlider.Value);
        }


        private void OnSpeedTextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(_speedTextBox.Text, out var newSpeed))
            {
                _speedTextBox.BackColor = Color.LightCoral;
                return;
            }

            if (newSpeed < _viewModel.MinSpeed || newSpeed > _viewModel.MaxSpeed)
            {
                _speedTextBox.BackColor = Color.LightCoral;
                return;
            }

            _speedTextBox.BackColor = Color.White;
            _viewModel.UpdateSpeed(newSpeed);

            // если значение превышает максимум слайдера, оставляем слайдер на максимуме
            _speedSlider.Value = newSpeed > _viewModel.MaxSpeed ? _viewModel.MaxSpeed : newSpeed;
        }


        private void OnStartStopClicked(object sender, EventArgs e)
        {
            _viewModel.ToggleAutoScroll();
            _startStopButton.Text = _viewModel.IsAutoScrolling ?
                _localizer.GetString("Stop") : _localizer.GetString("Start");

            if (_viewModel.IsAutoScrolling)
                _autoScrollTimer.Start();
            else
                _autoScrollTimer.Stop();
        }

        private void OnAutoScrollTick(object sender, EventArgs e)
        {
            _viewModel.RotateAutomatically();
            Invalidate();
        }

        private void OnResize(object sender, EventArgs e)
        {
            Invalidate(); // Перерисовка в соответствии с размерами окна 
        }

        private void OnZoomInClicked(object sender, EventArgs e)
        {
            _localizer.SetCulture("ru-RU");
            _viewModel.ZoomIn();
            Invalidate();
        }

        private void OnZoomOutClicked(object sender, EventArgs e)
        {
            _viewModel.ZoomOut();
            Invalidate();
        }

        private void OnShapeSelected(object sender, EventArgs e)
        {
            _viewModel.ChangeShape(_shapeSelector.SelectedIndex);  // изменяем текущую фигуру в ViewModel
            UpdateSlidersBasedOnSelection();
            Invalidate();  // обновляем отображение

        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // сглаживание при отрисовке
            _viewModel.Draw(e.Graphics, ClientSize);
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
            if (!_isDragging) return;
            Point currentPosition = e.Location;
            _viewModel.UpdateCameraRotation(currentPosition.X - _startPosition.X, currentPosition.Y - _startPosition.Y);
            _startPosition = currentPosition;
            Invalidate();
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

        private void OnDrawStrategySelected(object sender, EventArgs e)
        {
            _viewModel.ChangeDrawStrategy(_drawStrategySelector.SelectedIndex);

            UpdateSlidersBasedOnSelection();
            Invalidate();
        }


        //-----------------ПРОЧИЕ_МЕТОДЫ-----------------


        private void UpdateSlidersBasedOnSelection()
        {
            bool isTesseract = _shapeSelector.SelectedIndex == 0;
            bool hasFaces = _drawStrategySelector.SelectedIndex == 1;
            _colorSliderManager.InitializeSliders(isTesseract, hasFaces);
        }

        private void UpdateLocalizedText()
        {
            _fileMenuItem.Text = _localizer.GetString("Options");
            _languageMenuItem.Text = _localizer.GetString("Language");
            _toggleSpeedMenuItem.Text = _localizer.GetString("SpeedInDegrees");
            _startStopButton.Text = _viewModel.IsAutoScrolling
                ? _localizer.GetString("Stop")
                : _localizer.GetString("Start");

            _shapeSelector.Items[0] = _localizer.GetString("Tesseract");
            _shapeSelector.Items[1] = _localizer.GetString("Pyramid");
            _shapeSelector.Items[2] = _localizer.GetString("Octahedron");
            _shapeSelector.Items[3] = _localizer.GetString("Cube");

            _projectionSelector.Items[0] = _localizer.GetString("Orthogonal");
            _projectionSelector.Items[1] = _localizer.GetString("Perspective");

            _drawStrategySelector.Items[0] = _localizer.GetString("WithoutFaces");
            _drawStrategySelector.Items[1] = _localizer.GetString("WithFaces");
        }
    }
}
