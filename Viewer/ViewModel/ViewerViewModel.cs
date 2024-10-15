using System.Drawing;
using Viewer.Model;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        private Camera _camera;
        public Shape3D CurrentShape { get; private set; }

        private Shape3D[] shapes;  // все фигуры

        public ViewerViewModel()
        {
            _camera = new Camera(5f);
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
            Pen pen = new Pen(Color.Black, 2);
            CurrentShape.Draw(g, pen, _camera, clientSize);
        }
    }
}
