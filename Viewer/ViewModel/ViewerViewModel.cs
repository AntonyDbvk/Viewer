using System.Drawing;
using Viewer.Model;

namespace Viewer.ViewModel
{
    public class ViewerViewModel
    {
        public Camera Camera { get; private set; }
        public Shape3D CurrentShape { get; private set; }

        private Shape3D[] shapes;  // все фигуры

        public ViewerViewModel()
        {
            Camera = new Camera(5f);
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
            Camera.UpdateAngles(deltaX, deltaY);
        }

        public void Draw(Graphics g, Size clientSize)
        {
            Pen pen = new Pen(Color.Black, 2);
            CurrentShape.Draw(g, pen, Camera, clientSize);
        }
    }
}
