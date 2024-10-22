using System.Drawing;
using Viewer.Model;
using Viewer.Model.Shapes;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        private readonly Camera _camera;
        private readonly Renderer _renderer;

        private const float DefaultCameraZoom = 5f;

        public Shape3D CurrentShape { get; private set; }

        private Shape3D[] _shapes;  // все фигуры
        public bool IsOrthogonal { get; set; }

        public ViewerViewModel()
        {
            _camera = new Camera(DefaultCameraZoom);
            _renderer = new Renderer();
            Init_shapes();
            CurrentShape = _shapes[0];  // тессеракт по умолчанию
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

        public void Zoom(float delta)
        {
            _camera.Zoom(delta);
        }

        public void Draw(Graphics g, Size clientSize)
        {
            _renderer.DrawShape(g, CurrentShape, _camera, clientSize, IsOrthogonal);
        }
    }
}