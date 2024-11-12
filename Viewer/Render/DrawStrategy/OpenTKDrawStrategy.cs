using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using System;
using Viewer.Model.Shapes;
using Viewer.Render.Cameras;
using Viewer.Render.DrawStrategy.Base;

namespace Viewer.Render.DrawStrategy
{
    public class OpenTKDrawStrategy
    {
        private OpenTKCamera camera;
        private bool isDragging = false;
        private Point lastMousePosition;
        private GLControl glControl;

        public OpenTKDrawStrategy(GLControl glControl, OpenTKCamera camera)
        {
            this.glControl = glControl;
            this.camera = camera;
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
           
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = camera.GetProjectionMatrix((float)glControl.Width / glControl.Height, false);
;            GL.LoadMatrix(ref perspective);
        }

        private void GlControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetupCamera();
            DrawCube();
            glControl.SwapBuffers();
        }

        private void SetupCamera()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            var lookAt = camera.GetViewMatrix();
            GL.LoadMatrix(ref lookAt);
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color4(1.0f, 0.0f, 0.0f, 1.0f); 
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color4(0.0f, 0.0f, 1.0f, 0.5f); 
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color4(1.0f, 1.0f, 0.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color4(0.5f, 0.0f, 0.5f, 1.0f); 
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.Color4(0.0f, 1.0f, 1.0f, 1.0f); 
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.End();
        }

    }

}