using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Viewer.Model.Geometry;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;

namespace Viewer.Render.DrawStrategy.OpenGlStrategy.Base
{
    public abstract class OpenGlDrawStrategyBase : IOpenGlDrawStrategy
    {
        protected abstract void DrawShape(Shape3D shape);
        public virtual void Draw(GLControl glControl, OpenTKCamera camera, Shape3D shape, bool isOrthogonal)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 projectionMatrix = camera.GetProjectionMatrix((float)glControl.Width / glControl.Height, isOrthogonal);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);

            SetupCamera(camera);
            DrawShape(shape);

            glControl.SwapBuffers();
        }

        protected void SetupCamera(OpenTKCamera camera)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            var lookAt = camera.GetViewMatrix();
            GL.LoadMatrix(ref lookAt);
        }

        protected void DrawEdges(Edge[] edges, Vertex[] vertices, Color edgeColor)
        {
            GL.LineWidth(3.0f);
            GL.Color4(edgeColor.R / 255f, edgeColor.G / 255f, edgeColor.B / 255f, 1.0f);
            GL.Begin(PrimitiveType.Lines);
            foreach (var edge in edges)
            {
                var start = vertices[edge.Start];
                var end = vertices[edge.End];
                GL.Vertex3(start.X, start.Y, start.Z);
                GL.Vertex3(end.X, end.Y, end.Z);
            }
            GL.End();
        }

        protected void DrawFaces(Face[] faces, Vertex[] vertices, Color faceColor)
        {
            if (faceColor.A < 255)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.DepthMask(false);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
                GL.DepthMask(true);
            }

            foreach (var face in faces)
            {
                GL.Color4(faceColor.R / 255f, faceColor.G / 255f, faceColor.B / 255f, faceColor.A / 255f);
                GL.Begin(PrimitiveType.Polygon);
                foreach (var vertexIndex in face.Vertices)
                {
                    var vertex = vertices[vertexIndex];
                    GL.Vertex3(vertex.X, vertex.Y, vertex.Z);
                }
                GL.End();
            }

            if (faceColor.A < 255)
            {
                GL.DepthMask(true);
                GL.Disable(EnableCap.Blend);
            }
        }
    }
}