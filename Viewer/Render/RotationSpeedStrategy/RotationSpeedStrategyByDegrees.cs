using System;

namespace Viewer.Render.RotationSpeedStrategy
{
    public class RotationSpeedStrategyByDegrees : IRotationSpeedStrategy
    {
        private const float RotationFactor = 100; //45 градусов
        public float CalculateRotationSpeed(int currentSpeed)
        {
            float radians = currentSpeed * (float)(Math.PI / 180);
            return radians * RotationFactor;
        }
    }
}