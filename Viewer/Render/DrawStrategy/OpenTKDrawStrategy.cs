using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class OpenTKDrawStrategy
    {
        private bool isDragging = false;
        private Point lastMousePosition;

        public void Draw(GLControl glControl, OpenTKCamera camera, Shape3D shape, bool isOrthogonal)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 perspective = camera.GetProjectionMatrix((float)glControl.Width / glControl.Height, isOrthogonal);
            GL.LoadMatrix(ref perspective);

            SetupCamera(camera);

            DrawShape(shape);
            glControl.SwapBuffers();
        }

        private void SetupCamera(OpenTKCamera camera)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            var lookAt = camera.GetViewMatrix();
            GL.LoadMatrix(ref lookAt);
        }

        private void DrawShape(Shape3D shape)
        {
            DrawingSettings settings = DrawingSettings.Instance;
            var edgeColor = settings.EdgePen1.Color;
            var faceColor = settings.FaceBrush1.Color;

            GL.LineWidth(3.0f);
            GL.Color4(edgeColor.R / 255f, edgeColor.G / 255f, edgeColor.B / 255f, 1.0f);
            GL.Begin(PrimitiveType.Lines);
            foreach (var edge in shape.Edges)
            {
                var start = shape.Vertices[edge.Start];
                var end = shape.Vertices[edge.End];
                GL.Vertex3(start.X, start.Y, start.Z);
                GL.Vertex3(end.X, end.Y, end.Z);
            }
            GL.End();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.DepthMask(false); 

    
            foreach (var face in shape.Faces)
            {
                GL.Color4(faceColor.R / 255f, faceColor.G / 255f, faceColor.B / 255f, faceColor.A / 255f);
                GL.Begin(PrimitiveType.Polygon);
                foreach (var vertexIndex in face.Vertices)
                {
                    var vertex = shape.Vertices[vertexIndex];
                    GL.Vertex3(vertex.X, vertex.Y, vertex.Z);
                }
                GL.End();
            }
            GL.DepthMask(true);
            GL.Disable(EnableCap.Blend);
        }
    }
}
