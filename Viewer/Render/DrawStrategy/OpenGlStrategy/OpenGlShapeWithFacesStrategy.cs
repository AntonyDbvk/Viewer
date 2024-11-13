using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.OpenGlStrategy.Base;

namespace Viewer.Render.DrawStrategy.OpenGlStrategy
{
    public sealed class OpenGlShapeWithFacesStrategy : OpenGlDrawStrategyBase
    {
        protected override void DrawShape(Shape3D shape)
        {
            DrawingSettings settings = DrawingSettings.Instance;

            DrawEdges(shape.Edges, shape.Vertices, settings.EdgePen1.Color);
            DrawFaces(shape.Faces, shape.Vertices, settings.FaceBrush1.Color);
        }
    }
}
