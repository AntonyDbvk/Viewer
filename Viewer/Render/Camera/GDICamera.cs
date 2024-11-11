using System;
using System.Drawing;

namespace Viewer.Render.Camera
{
    public class GDICamera : CameraBase
    {
        private const float ScaleFactorOrtho = 500f;
        private const float ScaleFactorPerspective = 100f;
        private bool isOrthogonal; 

        public GDICamera(float initialDistance) : base(initialDistance) { }

        public override void SetProjection(int width, int height, bool isOrthogonal)
        {

        }

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

            return isOrthogonal ? OrthogonalProjection(dx, dy, clientSize) : PerspectiveProjection(dx, dy, dz, clientSize);
        }

        private PointF OrthogonalProjection(float dx, float dy, Size clientSize)
        {
            float factor = ScaleFactorOrtho / Distance;
            float projectedX = dx * factor + clientSize.Width / 2;
            float projectedY = dy * factor + clientSize.Height / 2;

            return new PointF(projectedX, projectedY);
        }

        private PointF PerspectiveProjection(float dx, float dy, float dz, Size clientSize)
        {
            float safeDistance = Distance;
            if (dz >= safeDistance) safeDistance += 0.1f;

            float factor = Distance / (safeDistance - dz);
            float projectedX = dx * factor * ScaleFactorPerspective + clientSize.Width / 2;
            float projectedY = dy * factor * ScaleFactorPerspective + clientSize.Height / 2;

            return new PointF(projectedX, projectedY);
        }
    }
}
