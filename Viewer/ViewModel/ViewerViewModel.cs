using System;
using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        private readonly Camera _camera;
        private readonly Renderer _renderer;
        private Shape3D[] _shapes;  // все фигуры
        private const float DefaultCameraZoom = 5f;
        private const int DefaultSpeed = 50;  // начальная скорость
        private const int MaxSpeed = 100;
        private const float RotationFactor = 50f; // коэффициент для расчета скорости вращения
        public Shape3D CurrentShape { get; private set; }
        public bool IsOrthogonal { get; set; }
        public bool IsAutoScrolling { get; private set; }
        public int CurrentSpeed { get; private set; } = DefaultSpeed;
        private float _rotationSpeed;

        public ViewerViewModel()
        {
            _camera = new Camera(DefaultCameraZoom);
            _renderer = new Renderer();
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

        public void ChangeShape(int index)
        {
            if (index >= 0 && index < _shapes.Length)
            {
                CurrentShape = _shapes[index];
            }
        }

        public void UpdateCameraRotation(float deltaX, float deltaY)
        {
            _camera.UpdateAngles(deltaX, deltaY);
        }

        public void ZoomIn()
        {
            _camera.Zoom(-0.5f);
        }

        public void ZoomOut()
        {
            _camera.Zoom(0.5f);
        }

        public void Zoom(float factor)
        {
            _camera.Zoom(factor);
        }

        public void Draw(Graphics g, Size clientSize)
        {
            _renderer.DrawShape(g, CurrentShape, _camera, clientSize, IsOrthogonal);
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
            _rotationSpeed = (float)Math.Pow(CurrentSpeed / (float)MaxSpeed, 2) * RotationFactor; // вычисление скорости вращения
        }
    }
}