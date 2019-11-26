using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class JsonTranslater
    {
        private JObject _configuration;
        private IDictionary<string, string> _cache;

        public JsonTranslater(string text)
        {
            _cache = new Dictionary<string, string>();
            _configuration = JObject.Parse(text);
        }

        public string GetSetting(string settingName)
        {
            string actualSettingValue = null;
            JObject actualJsonNode = _configuration;
            if (!_cache.TryGetValue(settingName, out actualSettingValue))
            {
                try
                {
                    foreach (string setting in settingName.Split('.'))
                    {
                        actualSettingValue = actualJsonNode[setting].ToString();
                        actualJsonNode = actualJsonNode[setting] as JObject;
                    }
                    _cache.Add(settingName, actualSettingValue);
                }
                catch
                {
                }
            }
            return actualSettingValue;
        }
    }
}