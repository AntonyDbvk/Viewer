using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Viewer.Render.Cameras
{
    public class OpenTKCamera : CameraBase
    {
        public OpenTKCamera(float initialDistance) : base(initialDistance) { }
        public OpenTKCamera(CameraBase camera) : base(camera) { }

        private float _fieldOfView = 45f;
        private float _nearClip = 0.1f;
        private float _farClip = 1000f;

        public Matrix4 GetViewMatrix()
        {
            var cameraPosition = new Vector3(
                Distance * (float)Math.Cos(AngleX) * (float)Math.Cos(AngleY),
                Distance * (float)Math.Sin(AngleX),  
                Distance * (float)Math.Cos(AngleX) * (float)Math.Sin(AngleY)  
            );
            var targetPosition = Vector3.Zero;
            var upDirection = -Vector3.UnitY;
            return Matrix4.LookAt(cameraPosition, targetPosition, upDirection);
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio, bool isOrtho)
        {
            if (!isOrtho)
            {
                return Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(_fieldOfView),
                    aspectRatio,
                    _nearClip,
                    _farClip
                );
            }

            float scale = 1f + Distance * 0.1f; 
            float orthoSize = (Distance / 2f) * scale;
            return Matrix4.CreateOrthographic(
                orthoSize * aspectRatio,
                orthoSize,
                _nearClip,
                _farClip
            );
        }



    }
}