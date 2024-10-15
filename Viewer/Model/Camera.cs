using System;
using System.Drawing;

namespace Viewer.Model
{
    public class Camera
    {
        public float AngleX { get; private set; }
        public float AngleY { get; private set; }
        public float Distance { get; private set; }

        private const float MIN_DISTANCE = 2f; 
        private const float MAX_DISTANCE = 15; 

        private const float MIN_ANGLE_X= -1.5f;
        private const float MAX_ANGLE_X= 1.5f;

        private const float MIN_DZ = 0.01f;

        public Camera(float initialDistance)
        {
            Distance = initialDistance;
            AngleX = 0;
            AngleY = 0;
        }

        public void Zoom(float delta)
        {
            Distance = Math.Max(MIN_DISTANCE, Math.Min(MAX_DISTANCE, Distance + delta));
        }


        // обновляем углы камеры при движении мыши
        public void UpdateAngles(float deltaX, float deltaY)
        {
            AngleY -= deltaX * 0.01f;
            AngleX = Math.Max(MIN_ANGLE_X, Math.Min(MAX_ANGLE_X, AngleX - deltaY * 0.01f));
        }

        // проекция 3D в 2D
        public PointF Project(float x, float y, float z, Size clientSize)
        {
            float cosX = (float)Math.Cos(AngleX);
            float sinX = (float)Math.Sin(AngleX);
            float cosY = (float)Math.Cos(AngleY);
            float sinY = (float)Math.Sin(AngleY);

            float dx = x * cosY - z * sinY;
            float dz = x * sinY + z * cosY;
            float dy = y * cosX - dz * sinX;
            dz = y * sinX + dz * cosX;

            float safeDistance = Distance;

            if (dz >= safeDistance)
            {
                safeDistance += 0.1f; 
            }


            float factor = Distance / (safeDistance - dz);
            float projectedX = dx * factor * 100 + clientSize.Width / 2;
            float projectedY = dy * factor * 100 + clientSize.Height / 2;

            if (float.IsInfinity(projectedX) || float.IsNegativeInfinity(projectedX)) throw new ArgumentException();

            return new PointF(projectedX, projectedY);

        }
    }
}
