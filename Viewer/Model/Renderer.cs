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

        public Renderer()
        {
            _drawingSettings = new DrawingSettings
            {
                EdgePen = new Pen(Color.Black, 2)
            };
        }

        public void DrawShape(Graphics g, Shape3D shape, Camera camera, Size clientSize, bool isOrthogonal)
        {
            if (shape is Tesseract)
            {
                var strategy = new TesseractDrawStrategy();
                strategy.Draw(g, shape, _drawingSettings, camera, clientSize, isOrthogonal);
            }
            else if (shape is Cube)
            {
                var strategy = new ShapeDrawStrategy();
                strategy.Draw(g, shape, _drawingSettings, camera, clientSize, isOrthogonal);
            }
            else if (shape is Pyramid)
            {
                var strategy = new ShapeDrawStrategy();
                strategy.Draw(g, shape, _drawingSettings, camera, clientSize, isOrthogonal);
            }
            else if (shape is Octahedron)
            {
                var strategy = new ShapeDrawStrategy();
                strategy.Draw(g, shape, _drawingSettings, camera, clientSize, isOrthogonal);
            }
        }
    }   
}
