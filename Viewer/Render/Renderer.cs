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
using Viewer.Render.DrawStrategy.OpenGlStrategy.Base;

namespace Viewer.Render
{
    public class Renderer
    {
        private readonly DrawingSettings _drawingSettings;
        private IDrawStrategy _drawStrategy;
        private IOpenGlDrawStrategy _openGlDrawStrategy;
        private DrawStrategyType _currentDrawStrategyType;
        private DrawStrategyType _currentOpenGlDrawStrategyType;
        private Type _currentShapeType;

        public Renderer()
        {
            _drawingSettings = DrawingSettings.Instance;
        }

        public void DrawShape(Graphics g, Shape3D shape, GDICamera gdiCamera, Size clientSize, bool isOrthogonal, DrawStrategyType drawStrategyType)
        {
            _drawStrategy = GetDrawStrategy(shape, drawStrategyType);
            _drawStrategy.Draw(g, shape, _drawingSettings, gdiCamera, clientSize, isOrthogonal);
        }

        public void DrawShapeOpenTk(GLControl glControl, Shape3D shape, OpenTKCamera camera, bool isOrthogonal, DrawStrategyType drawStrategyType)
        {
            _openGlDrawStrategy = GetOpenGlDrawStrategy(shape, drawStrategyType);
            _openGlDrawStrategy.Draw(glControl, camera, shape, isOrthogonal);
        }

        private IDrawStrategy GetDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType)
        {
            if (_drawStrategy == null || _currentDrawStrategyType != drawStrategyType || _currentShapeType != shape.GetType())
            {
                _currentDrawStrategyType = drawStrategyType;
                _currentShapeType = shape.GetType();
                if (shape is Tesseract)
                {
                    _drawStrategy = drawStrategyType == DrawStrategyType.WithFaces
                        ? (IDrawStrategy)new TesseractWithFacesDrawStrategy()
                        : new TesseractDrawStrategy();
                }
                else
                {
                    _drawStrategy = drawStrategyType == DrawStrategyType.WithFaces
                        ? (IDrawStrategy)new ShapeWithFacesStrategy()
                        : new ShapeDrawStrategy();
                }
            }
            return _drawStrategy;
        }

        private IOpenGlDrawStrategy GetOpenGlDrawStrategy(Shape3D shape, DrawStrategyType drawStrategyType)
        {
            if (_openGlDrawStrategy == null || _currentOpenGlDrawStrategyType != drawStrategyType || _currentShapeType != shape.GetType())
            {
                _currentOpenGlDrawStrategyType = drawStrategyType;
                _currentShapeType = shape.GetType();
                if (shape is Tesseract)
                {
                    _openGlDrawStrategy = drawStrategyType == DrawStrategyType.WithFaces
                        ? (IOpenGlDrawStrategy)new OpenGlTesseractWithFacesStrategy()
                        : new OpenGlTesseractStrategy();
                }
                else
                {
                    _openGlDrawStrategy = drawStrategyType == DrawStrategyType.WithFaces
                        ? (IOpenGlDrawStrategy)new OpenGlShapeWithFacesStrategy()
                        : new OpenGlShapeDrawStrategy();
                }
            }
            return _openGlDrawStrategy;
        }
    }

}
