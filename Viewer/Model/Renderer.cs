using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model.Shapes;
using Viewer.Model.Strategy;

namespace Viewer.Model
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
