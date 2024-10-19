using System.Drawing;
using Viewer.Model;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        private Camera _camera;

        private const float DEFAULT_CAMERA_ZOOM = 5f;

        public Shape3D CurrentShape { get; private set; }

        private Shape3D[] shapes;  // все фигуры
        public bool IsOrthogonal { get; set; }

        public ViewerViewModel()
        {
            _camera = new Camera(DEFAULT_CAMERA_ZOOM);
            InitShapes();
            CurrentShape = shapes[0];  // тессеракт по умолчанию
        }

        private void InitShapes()
        {
            shapes = new Shape3D[]
            {
                new Tesseract(),
                new Pyramid(),
                new Octahedron()
            };
        }

        public void ChangeShape(int index)
        {
            if (index >= 0 && index < shapes.Length)
            {
                CurrentShape = shapes[index];
            }
        }

        public void UpdateCameraRotation(float deltaX, float deltaY)
        {
            _camera.UpdateAngles(deltaX, deltaY);
        }

        public void Zoom(float delta)
        {
            _camera.Zoom(delta);
        }

        public void Draw(Graphics g, Size clientSize)
        {
            DrawingSettings settings = new DrawingSettings
            {
                EdgePen = new Pen(Color.Black, 2)
            };

            CurrentShape.Draw(g, settings, _camera, clientSize, IsOrthogonal);
        }
    }
}
