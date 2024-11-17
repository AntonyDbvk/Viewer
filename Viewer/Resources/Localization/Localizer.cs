using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Viewer.Resources.Localization
{
    public class Localizer
    {
        private static Localizer _instance;
        private static readonly object Lock = new object();
        private readonly ResourceManager _resourceManager;
        public event Action CultureChanged;

        private Localizer(string baseName, Assembly assembly)
        {
            _resourceManager = new ResourceManager(baseName, assembly);
        }

        public static Localizer Instance(string baseName, Assembly assembly)
        {
            if (_instance != null) return _instance;
            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = new Localizer(baseName, assembly);
                }
            }
            return _instance;
        }

        public static Localizer Instance()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("Экземпляр локализатора не был создан.");
            }
            return _instance;
        }

        public void SetCulture(string cultureCode)
        {
            CultureInfo culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureChanged?.Invoke();
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key);
        }
    }
}