using System.Drawing;
using Viewer.Model;
using Viewer.Model.Shapes;
using Viewer.Render.Strategy;

namespace Viewer.Render
{
    public class Renderer
    {
        private readonly DrawingSettings _drawingSettings;
        private IDrawStrategy _drawStrategy;
        public Renderer()
        {
            _drawingSettings = new DrawingSettings
            {
                EdgePen = new Pen(Color.Black, 2)
            };
        }

        public void DrawShape(Graphics g, Shape3D shape, Camera camera, Size clientSize, bool isOrthogonal)
        {
            _drawStrategy = shape is Tesseract ? new TesseractDrawStrategy() : new ShapeDrawStrategy();

            _drawStrategy.Draw(g, shape, _drawingSettings, camera, clientSize, isOrthogonal);
        }
    }   
}
