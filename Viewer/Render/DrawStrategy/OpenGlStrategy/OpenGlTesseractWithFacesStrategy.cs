using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.DrawStrategy.OpenGlStrategy.Base;

namespace Viewer.Render.DrawStrategy.OpenGlStrategy
{
    public sealed class OpenGlTesseractWithFacesStrategy : OpenGlDrawStrategyBase
    {
        protected override void DrawShape(Shape3D shape)
        {
            var tesseract = shape as Tesseract;
            DrawingSettings settings = DrawingSettings.Instance;

            ConnectCubes(tesseract.Vertices, tesseract.InnerVertices, Color.Black);
            GL.DepthMask(true);
            DrawEdges(tesseract.Edges, tesseract.InnerVertices, settings.EdgePen1.Color);
            DrawEdges(tesseract.Edges, tesseract.Vertices, settings.EdgePen2.Color);

            DrawFaces(tesseract.Faces, tesseract.InnerVertices, settings.FaceBrush1.Color);
            DrawFaces(tesseract.Faces, tesseract.Vertices, settings.FaceBrush2.Color);
        }

        protected void ConnectCubes(Vertex[] outerVertices, Vertex[] innerVertices, Color color)
        {
            GL.LineWidth(3.0f);
            GL.Color4(color.R / 255f, color.G / 255f, color.B / 255f, 1.0f);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < outerVertices.Length; i++)
            {
                var outerVertex = outerVertices[i];
                var innerVertex = innerVertices[i];
                GL.Vertex3(outerVertex.X, outerVertex.Y, outerVertex.Z);
                GL.Vertex3(innerVertex.X, innerVertex.Y, innerVertex.Z);
            }
            GL.End();
        }
    }
}