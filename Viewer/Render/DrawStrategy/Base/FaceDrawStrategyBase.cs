using System.Drawing;
using Viewer.Model.Geometry;
using Viewer.Model.Geometry.Viewer.Model.Geometry;
using Viewer.Render.Cameras;

namespace Viewer.Render.DrawStrategy.Base
{
    public abstract class FaceDrawStrategyBase : DrawStrategyBase
    {
        protected void DrawFaces(Graphics g, Vertex[] vertices, Face[] faces, Brush faceBrush, GDICamera gdiCamera, Size clientSize, bool isOrthogonal)
        {
            PointF[] projectedPoints = new PointF[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                projectedPoints[i] = gdiCamera.Project(vertices[i].X, vertices[i].Y, vertices[i].Z, clientSize, isOrthogonal);
            }

            foreach (var face in faces)
            {
                PointF[] points = new PointF[face.Vertices.Length];
                for (int i = 0; i < face.Vertices.Length; i++)
                {
                    points[i] = projectedPoints[face.Vertices[i]];
                }
                g.FillPolygon(faceBrush, points);
            }
        }
    }
}
