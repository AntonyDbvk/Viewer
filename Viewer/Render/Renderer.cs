using System.Drawing;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render
{
    public class Renderer
    {
        private readonly DrawingSettings _drawingSettings;
        private IDrawStrategy _drawStrategy;
        public Renderer()
        {
            _drawingSettings = DrawingSettings.Instance;
        }

        public void DrawShape(Graphics g, Shape3D shape, GDICamera gdiCamera, Size clientSize, bool isOrthogonal, DrawStrategyType drawStrategyType)
        {
            _drawStrategy = GetDrawStrategy(shape, drawStrategyType);
            _drawStrategy.Draw(g, shape, _drawingSettings, gdiCamera, clientSize, isOrthogonal);
        }

        private IDrawStrategy GetDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType)
        {
            if (shape is Tesseract)
            {
                return drawStrategyType == DrawStrategyType.WithFaces
                    ? (IDrawStrategy)new TesseractWithFacesDrawStrategy()
                    : new TesseractDrawStrategy();
            }

            return drawStrategyType == DrawStrategyType.WithFaces
                ? (IDrawStrategy)new ShapeWithFacesStrategy()
                : new ShapeDrawStrategy();
        }

    }
}
