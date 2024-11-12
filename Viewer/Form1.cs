using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenTK;
using Viewer.Render;
using Viewer.Render.Cameras;
using Viewer.Resources.Localization;
using Viewer.UIComponents;
using Viewer.ViewModel;
using Timer = System.Windows.Forms.Timer;

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
        private Control _drawPanel;



        public Form1()
        {
            _viewModel = new ViewerViewModel();
            InitializeComponent();
            InitGdiDrawPanel();
            _colorSliderManager = new ColorSliderManager(this, _rightPanel, _drawPanel);
            _localizer = Localizer.Instance();
            InitUi();
            AddBaseEvents();
        }

        //-----------------ИНИЦИАЛИЗАЦИЯ_UI-----------------

        private void InitUi()
        {
            InitMenu();
            InitShapeSelector();
            InitProjectionSelector();
            InitDrawStrategySelector();
            InitZoomButtons();
            InitAutoScrollControls();
            UpdateSlidersBasedOnSelection();
        }

        private void InitAutoScrollControls()
        {
            InitStartStopButton();
            InitSpeedTextBox();
            InitSpeedSlider();
            InitAutoScrollTimer();
        }

        private void InitGdiDrawPanel()
        {
            _viewModel._gdiCamera = new GDICamera(_viewModel._gdiCamera);
            ReplaceDrawPanel(new BufferedPanel(), OnGDIPaint);
        }

        private void InitOpenGlDrawPanel()
        {
            _viewModel._gdiCamera = new OpenTKCamera(_viewModel._gdiCamera);
            ReplaceDrawPanel(new GLControl(), OnOpenGLPaint);
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

            var renderMenuItem = new ToolStripMenuItem("Способ отрисовки");
            var gdiRenderMenuItem = new ToolStripMenuItem("GDI+", null, (s, e) => InitGdiDrawPanel());
            var openGlRenderMenuItem = new ToolStripMenuItem("OpenGl", null, (s, e) => InitOpenGlDrawPanel());
            renderMenuItem.DropDownItems.Add(gdiRenderMenuItem);
            renderMenuItem.DropDownItems.Add(openGlRenderMenuItem);





            _fileMenuItem.DropDownItems.Add(_toggleSpeedMenuItem);
            _fileMenuItem.DropDownItems.Add(_languageMenuItem);
            _fileMenuItem.DropDownItems.Add(renderMenuItem);
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
                _autoScrollPanel.Controls.Add(_speedSlider, 0, 0);
                _autoScrollPanel.SetColumnSpan(_speedSlider, 2);
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
                Size = new Size(60, 30),
                Text = _localizer.GetString("Start")
            };
            _startStopButton.Click += OnStartStopClicked;
            _startStopButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            _autoScrollPanel.Controls.Add(_startStopButton, 0, 1);
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
            _speedTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            _autoScrollPanel.Controls.Add(_speedTextBox, 1, 1);
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
            _shapeSelector = new ComboBox
            {
                MaximumSize = new Size(200, 20),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _shapeSelector.Items.AddRange(new object[]
            {
                _localizer.GetString("Tesseract"),
                _localizer.GetString("Pyramid"),
                _localizer.GetString("Octahedron"),
                _localizer.GetString("Cube")
            });
            _shapeSelector.SelectedIndex = 0;
            _shapeSelector.SelectedIndexChanged += OnShapeSelected;
            _leftPanel.Controls.Add(_shapeSelector, 0, 0);
        }

        private void InitProjectionSelector()
        {
            _projectionSelector = new ComboBox
            {
                MaximumSize = new Size(200, 20),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _projectionSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            _projectionSelector.Items.AddRange(new object[]
            {
                _localizer.GetString("Orthogonal"),
                _localizer.GetString("Perspective")
            });
            _projectionSelector.SelectedIndex = 1;
            _projectionSelector.SelectedIndexChanged += OnProjectionSelected;
            _leftPanel.Controls.Add(_projectionSelector, 0, 1);
        }

        private void InitDrawStrategySelector()
        {
            _drawStrategySelector = new ComboBox
            {
                MaximumSize = new Size(200, 20),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _drawStrategySelector.Items.AddRange(new object[]
            {
                _localizer.GetString("WithoutFaces"),
                _localizer.GetString("WithFaces")
            });
            _drawStrategySelector.SelectedIndex = 0;
            _drawStrategySelector.SelectedIndexChanged += OnDrawStrategySelected;
            _leftPanel.Controls.Add(_drawStrategySelector, 0, 2);
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


            _buttonPanel.Controls.Add(_zoomOutButton, 0, 0);
            _buttonPanel.Controls.Add(_zoomInButton, 1, 0);


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
            _startStopButton.Text =
                _viewModel.IsAutoScrolling ? _localizer.GetString("Stop") : _localizer.GetString("Start");

            if (_viewModel.IsAutoScrolling)
                _autoScrollTimer.Start();
            else
                _autoScrollTimer.Stop();
        }

        private void OnAutoScrollTick(object sender, EventArgs e)
        {
            _viewModel.RotateAutomatically();
            _drawPanel.Invalidate();
        }

        private void OnResize(object sender, EventArgs e)
        {
            _drawPanel.Invalidate();
        }

        private void OnZoomInClicked(object sender, EventArgs e)
        {
            _viewModel.ZoomIn();
            _drawPanel.Invalidate();
        }

        private void OnZoomOutClicked(object sender, EventArgs e)
        {
            _viewModel.ZoomOut();
            _drawPanel.Invalidate();
        }

        private void OnShapeSelected(object sender, EventArgs e)
        {
            _viewModel.ChangeShape(_shapeSelector.SelectedIndex); // изменяем текущую фигуру в ViewModel
            UpdateSlidersBasedOnSelection();
            _drawPanel.Invalidate(); // обновляем отображение

        }

        private void OnGDIPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // сглаживание при отрисовке
            _viewModel.Draw(e.Graphics, _drawPanel.Size);
        }

        private void OnOpenGLPaint(object sender, PaintEventArgs e)
        {
            _viewModel.Draw((GLControl)_drawPanel, _drawPanel.Size);
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
            _drawPanel.Invalidate(); // Перерисовываем
        }


        // движение мыши — поворот камеры
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            Point currentPosition = e.Location;
            _viewModel.UpdateCameraRotation(currentPosition.X - _startPosition.X, currentPosition.Y - _startPosition.Y);
            _startPosition = currentPosition;
            _drawPanel.Invalidate();
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void OnProjectionSelected(object sender, EventArgs e)
        {
            _viewModel.IsOrthogonal = _projectionSelector.SelectedIndex == 0;
            _drawPanel.Invalidate();
        }

        private void OnDrawStrategySelected(object sender, EventArgs e)
        {
            _viewModel.ChangeDrawStrategy(_drawStrategySelector.SelectedIndex);

            UpdateSlidersBasedOnSelection();
            _drawPanel.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Properties.Settings.Default.CultureLanguage = Thread.CurrentThread.CurrentCulture.Name;
            Properties.Settings.Default.ShapeIndex = _shapeSelector.SelectedIndex;
            Properties.Settings.Default.ProjectionIndex = _projectionSelector.SelectedIndex;
            Properties.Settings.Default.DrawStrategyIndex = _drawStrategySelector.SelectedIndex;
            Properties.Settings.Default.Save();
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

        private void AddBaseEvents()
        {
            MouseWheel += OnMouseWheel;
            Resize += OnResize;
            _localizer.CultureChanged += UpdateLocalizedText;
            AddMouseEvents();
        }

        private void AddMouseEvents()
        {
            foreach (Control control in _tableLayoutPanel.Controls)
            {
                control.MouseDown += OnMouseDown;
                control.MouseMove += OnMouseMove;
                control.MouseUp += OnMouseUp;
            }

            _tableLayoutPanel.MouseDown += OnMouseDown;
            _tableLayoutPanel.MouseMove += OnMouseMove;
            _tableLayoutPanel.MouseUp += OnMouseUp;
        }

        private void LoadSettings()
        {
            _localizer.SetCulture(Properties.Settings.Default.CultureLanguage);
            _shapeSelector.SelectedIndex = Properties.Settings.Default.ShapeIndex;
            _projectionSelector.SelectedIndex = Properties.Settings.Default.ProjectionIndex;
            _drawStrategySelector.SelectedIndex = Properties.Settings.Default.DrawStrategyIndex;
        }


        private void ReplaceDrawPanel(Control newPanel, PaintEventHandler paintHandler)
        {
            if (_drawPanel != null)
            {
                _drawPanel.Paint -= OnGDIPaint;
                _drawPanel.Paint -= OnOpenGLPaint;
                _tableLayoutPanel.Controls.Remove(_drawPanel);
                _drawPanel.Dispose();
            }

            _drawPanel = newPanel;
            _drawPanel.Dock = DockStyle.Fill;
            if (_drawPanel is GLControl) _viewModel.Draw((GLControl)_drawPanel, _drawPanel.Size);
            _drawPanel.Paint += paintHandler;
            _drawPanel.MouseDown += OnMouseDown;
            _drawPanel.MouseMove += OnMouseMove;
            _drawPanel.MouseUp += OnMouseUp;

            _tableLayoutPanel.Controls.Add(_drawPanel, 1, 0);
        }
    }
}
