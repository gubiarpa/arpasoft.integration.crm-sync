using System.Configuration;

namespace Expertia.Estructura.Utils
{
    public class ConfigAccess
    {
        public static string GetValueInAppSettings(string key, string defaultValue = null)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetValueInConnectionString(string key, string defaultValue = null)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}