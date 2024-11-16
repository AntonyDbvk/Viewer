using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy;
using Viewer.Render.DrawStrategy.GDIStrategy;
using Viewer.Render.DrawStrategy.GDIStrategy.Base;
using Viewer.Render.DrawStrategy.OpenGlStrategy;
using System;
using System.Windows.Forms;
using Viewer.Render.DrawStrategy.DrawContext;
using Viewer.Render.DrawStrategy.OpenGlStrategy.Base;

namespace Viewer.Render
{
    public class Renderer
    {
        private readonly DrawingSettings _drawingSettings;
        private IDrawStrategy _drawStrategy;
        private DrawStrategyType _currentDrawStrategyType;
        private Type _currentShapeType;

        public Renderer()
        {
            _drawingSettings = DrawingSettings.Instance;
        }

        public void DrawShape(IDrawContext context, Shape3D shape, CameraBase gdiCamera, bool isOrthogonal, DrawStrategyType drawStrategyType)
        {
            _drawStrategy = GetDrawStrategy(shape, drawStrategyType, context);
            _drawStrategy.Draw(context, shape, _drawingSettings, gdiCamera, isOrthogonal);
        }

        private IDrawStrategy GetDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType, IDrawContext context)
        {
            return context is GDIDrawContext ? GetGDIDrawStrategy(shape, drawStrategyType) : GetOpenGlDrawStrategy(shape, drawStrategyType);
        }

        private IDrawStrategy GetGDIDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType)
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

        private IDrawStrategy GetOpenGlDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType)
        {
            if (shape is Tesseract)
            {
                return drawStrategyType == DrawStrategyType.WithFaces
                    ? (IDrawStrategy)new OpenGlTesseractWithFacesStrategy()
                    : new OpenGlTesseractStrategy();
            }

            return drawStrategyType == DrawStrategyType.WithFaces
                ? (IDrawStrategy)new OpenGlShapeWithFacesStrategy()
                : new OpenGlShapeDrawStrategy();
        }


    }

}
