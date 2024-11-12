using System;

namespace Viewer.Render.Cameras
{
    public abstract class CameraBase
    {
        public float AngleX { get; protected set; }
        public float AngleY { get; protected set; }
        public float Distance { get; protected set; }

        protected const float MinDistance = 2f;
        protected const float MaxDistance = 15f;

        protected CameraBase(float initialDistance)
        {
            Distance = initialDistance;
            AngleX = 0;
            AngleY = 0;
        }

        public  CameraBase(CameraBase camera)
        {
            Distance = camera.Distance;
            AngleX = camera.AngleX;
            AngleY = camera.AngleY;
        }

        public void Zoom(float delta)
        {
            Distance = Math.Max(MinDistance, Math.Min(MaxDistance, Distance + delta));
        }

        public void UpdateAngles(float deltaX, float deltaY)
        {
            AngleY -= deltaX * 0.01f;
            AngleX = Math.Max(-1.5f, Math.Min(1.5f, AngleX - deltaY * 0.01f));
        }

    }
}