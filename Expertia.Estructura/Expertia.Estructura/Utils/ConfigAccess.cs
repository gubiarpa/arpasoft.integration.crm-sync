using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

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