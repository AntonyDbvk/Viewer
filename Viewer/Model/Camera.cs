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

        private const float MIN_ANGLE_X = -1.5f;
        private const float MAX_ANGLE_X = 1.5f;

        private const float SCALE_FACTOR_PERSPECTIVE = 100f;
        private const float SCALE_FACTOR_ORTHO = 500f;

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

        
        public PointF Project(float x, float y, float z, Size clientSize, bool isOrthogonal = false)
        {
            float cosX = (float)Math.Cos(AngleX);
            float sinX = (float)Math.Sin(AngleX);
            float cosY = (float)Math.Cos(AngleY);
            float sinY = (float)Math.Sin(AngleY);

           
            float dx = x * cosY - z * sinY;
            float dz = x * sinY + z * cosY;
            float dy = y * cosX - dz * sinX;
            dz = y * sinX + dz * cosX;

            return isOrthogonal ? OrthogonalProjection(dx, dy, clientSize) : PerspectiveProjection(dx, dy, dz, clientSize);
        }

        private PointF OrthogonalProjection(float dx, float dy, Size clientSize)
        {
            float factor = SCALE_FACTOR_ORTHO / Distance;
            float projectedX = dx * factor + clientSize.Width / 2;
            float projectedY = dy * factor + clientSize.Height / 2;

            return new PointF(projectedX, projectedY);
        }

        private PointF PerspectiveProjection(float dx, float dy, float dz, Size clientSize)
        {
            
            float safeDistance = Distance;

            if (dz >= safeDistance)
            {
                safeDistance += 0.1f;
            }

            float factor = Distance / (safeDistance - dz);
            float projectedX = dx * factor * SCALE_FACTOR_PERSPECTIVE + clientSize.Width / 2;
            float projectedY = dy * factor * SCALE_FACTOR_PERSPECTIVE + clientSize.Height / 2;

            if (float.IsInfinity(projectedX) || float.IsNegativeInfinity(projectedX))
            {
                throw new ArithmeticException("Проекцирование привело к бесконечному значению для координаты X.");
            }
            if (float.IsInfinity(projectedY) || float.IsNegativeInfinity(projectedY))
            {
                throw new ArithmeticException("Проекцирование привело к бесконечному значению для координаты Y.");
            }

            return new PointF(projectedX, projectedY);
        }

    }
}
