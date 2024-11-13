using OpenTK.Graphics;
using System.Drawing;

namespace Viewer.Render
{
    public static class ColorConverter
    {
        // Конвертировать из Color в Color4 (RGBA)
        public static Color4 ToColor4(Color color)
        {
            return new Color4(
                color.R / 255f, // Красный компонент
                color.G / 255f, // Зеленый компонент
                color.B / 255f, // Синий компонент
                color.A / 255f  // Альфа компонент
            );
        }

        // Конвертировать из SolidBrush в Color4 (RGBA)
        public static Color4 ToColor4(SolidBrush brush)
        {
            return ToColor4(brush.Color); // Просто конвертируем через Color
        }
    }
}