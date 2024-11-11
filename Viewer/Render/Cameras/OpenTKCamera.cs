using System.Security.Cryptography;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Viewer.Render.Cameras
{
    public class OpenTKCamera : CameraBase
    {
        public OpenTKCamera(float initialDistance) : base(initialDistance) { }

        public void ApplyTransformations(int width, int height, bool isOrthogonal)
        {
            SetProjection(width, height, isOrthogonal);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0.0f, 0.0f, -Distance);
            GL.Rotate(AngleX * (180.0f / (float)Math.PI), 1.0f, 0.0f, 0.0f);
            GL.Rotate(AngleY * (180.0f / (float)Math.PI), 0.0f, 1.0f, 0.0f);
        }

        private void SetProjection(int width, int height, bool isOrthogonal)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            if (isOrthogonal)
            {
                float aspectRatio = width / (float)height;
                GL.Ortho(-aspectRatio * Distance, aspectRatio * Distance, -Distance, Distance, 1.0f, 100.0f);
            }
            else
            {
                Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(45.0f), width / (float)height, 1.0f, 100.0f);
                GL.LoadMatrix(ref perspective);
            }

            GL.MatrixMode(MatrixMode.Modelview);
        }
    }
}