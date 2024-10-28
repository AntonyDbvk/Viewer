using System;

namespace Viewer.Render.RotationSpeedStrategy
{
    public class SimpleRotationSpeedStrategy : IRotationSpeedStrategy
    {
        private const int MaxSpeed = 100;

        private const float RotationFactor = 78.5398f; //45 градусов

        public float CalculateRotationSpeed(int currentSpeed)
        {
            return (float)Math.Pow(currentSpeed / (float)MaxSpeed, 2) * RotationFactor;
        }
    }
}