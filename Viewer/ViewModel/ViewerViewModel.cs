using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render;
using Viewer.Render.Cameras;
using Viewer.Render.RotationSpeedStrategy;
using Viewer.Resources.Localization;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        private Localizer _localizer;
        private readonly GDICamera _gdiCamera;
        private readonly Renderer _renderer;
        private Shape3D[] _shapes;
        public Shape3D CurrentShape { get; private set; }
        public bool IsOrthogonal { get; set; }
        public bool IsAutoScrolling { get; private set; }
        public int CurrentSpeed { get; private set; } = DefaultSpeed;
        private const float DefaultCameraZoom = 5f;
        private const int DefaultSpeed = 45;
        private float _rotationSpeed;
        public int MinSpeed { get; set; } = 1;
        public int MaxSpeed { get; set; } = 100;
        private IRotationSpeedStrategy _rotationSpeedStrategy;
        private DrawStrategyType _currentDrawStrategy = DrawStrategyType.WithoutFaces;



        public ViewerViewModel()
        {
            _localizer = Localizer.Instance("Viewer.Resources.Localization.FormElementNamesService", typeof(ViewerViewModel).Assembly);
            _gdiCamera = new GDICamera(DefaultCameraZoom);
            _renderer = new Renderer();
            _rotationSpeedStrategy = new SimpleRotationSpeedStrategy();
            Init_shapes();
            CurrentShape = _shapes[0];  // тессеракт по умолчанию
            UpdateRotationSpeed();
        }

        private void Init_shapes()
        {
            _shapes = new Shape3D[]
            {
            new Tesseract(),
            new Pyramid(),
            new Octahedron(),
            new Cube()
            };
        }

        public void ChangeDrawStrategy(int index)
        {
            _currentDrawStrategy = (DrawStrategyType)index;
        }

        public void ChangeShape(int index)
        {
            if (index >= 0 && index < _shapes.Length)
            {
                CurrentShape = _shapes[index];
            }
        }

        public void UpdateCameraRotation(float deltaX, float deltaY)
        {
            _gdiCamera.UpdateAngles(deltaX, deltaY);
        }

        public void ZoomIn()
        {
            _gdiCamera.Zoom(-0.5f);
        }

        public void ZoomOut()
        {
            _gdiCamera.Zoom(0.5f);
        }

        public void Zoom(float factor)
        {
            _gdiCamera.Zoom(factor);
        }

        public void Draw(Graphics g, Size clientSize)
        {
            _renderer.DrawShape(g, CurrentShape, _gdiCamera, clientSize, IsOrthogonal, _currentDrawStrategy);
        }

        public void ToggleAutoScroll()
        {
            IsAutoScrolling = !IsAutoScrolling;
        }

        public void RotateAutomatically()
        {
            UpdateCameraRotation(_rotationSpeed, 0);
        }

        public void UpdateSpeed(int newSpeed)
        {
            CurrentSpeed = newSpeed;
            UpdateRotationSpeed();
        }

        private void UpdateRotationSpeed()
        {
            _rotationSpeed = _rotationSpeedStrategy.CalculateRotationSpeed(CurrentSpeed);
        }

        public void ToggleRotationSpeedStrategy()
        {
            _rotationSpeedStrategy = _rotationSpeedStrategy is SimpleRotationSpeedStrategy
                ? (IRotationSpeedStrategy)new RotationSpeedStrategyByDegrees()
                : new SimpleRotationSpeedStrategy();

            MaxSpeed = MaxSpeed == 100 ? 360 : 100;
        }

    }
}