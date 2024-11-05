using System.Windows.Forms;

namespace Viewer.UIComponents
{
    /// <summary>
    /// Из-за отсутствия прямого доступа к свойству двойной буфферизации
    /// используем механизм наследования
    /// </summary>
    public sealed class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            DoubleBuffered = true;
        }
    }

}