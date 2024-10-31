using System.Drawing;
using Viewer.Model;
using Viewer.Model.Shapes;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class ShapeWithFacesStrategy : FaceDrawStrategyBase
    {
        public override void Draw(Graphics g, Shape3D model, DrawingSettings settings, Camera camera, Size clientSize, bool isOrthogonal)
        {
            var vertices = model.Vertices;
            var edges = model.Edges;

            DrawFaces(g, vertices, edges, new SolidBrush(Color.FromArgb(20, Color.Violet)), camera, clientSize, isOrthogonal);
            DrawEdges(g, vertices, edges, settings.EdgePen, camera, clientSize, isOrthogonal);
        }
    }
}