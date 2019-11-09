using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils.Behavior;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Expertia.Estructura.Utils
{
    public static class ExtensionMethods
    {
        #region Nullable
        public static object Coalesce(this object obj, object nullValue = null)
        {
            try
            {
                return obj == null ? (nullValue == null ? DBNull.Value : nullValue) : obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Json
        public static string Stringify(this object obj, bool indented = false)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = indented ? Formatting.Indented : Formatting.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Log
        public static void TryWriteLogObject(this object obj, ILogFileManager logFileManager, IClientFeatures clientFeatures, LogType logType = LogType.Info, bool indented = true)
        {
            try
            {
                logFileManager.WriteText(obj.BuildLogObject(clientFeatures, logType).Stringify(indented) + "\n");
            }
            catch (Exception ex)
            {
                try
                {
                    logFileManager.WriteText(ex.BuildLogObject(clientFeatures, logType).Stringify(indented) + "\n");
                }
                catch
                {
                }
            }
        }

        private static object BuildLogObject(this object obj, IClientFeatures clientFeatures, LogType logType = LogType.Info)
        {
            try
            {
                return new
                {
                    Client = new
                    {
                        clientFeatures.URL,
                        clientFeatures.Method,
                        Log = logType,
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    },
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Keys
        public static string GetKeyValues(this UnidadNegocioKeys unidadNegocioKey)
        {
            try
            {
                switch (unidadNegocioKey)
                {
                    case UnidadNegocioKeys.CondorTravel: return UnidadNegocioShortNames.CondorTravel;
                    case UnidadNegocioKeys.DestinosMundiales: return UnidadNegocioShortNames.DestinosMundiales;
                    case UnidadNegocioKeys.InterAgencias: return UnidadNegocioShortNames.InterAgencias;
                    default: return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string ToConnectionKey(this UnidadNegocioKeys? unidadNegocio)
        {
            try
            {
                switch (unidadNegocio)
                {
                    case UnidadNegocioKeys.CondorTravel: return ConnectionKeys.CondorConnKey;
                    case UnidadNegocioKeys.DestinosMundiales: return ConnectionKeys.DMConnKey;
                    case UnidadNegocioKeys.InterAgencias: return ConnectionKeys.IAConnKey;
                    default: return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static UnidadNegocioKeys? ToUnidadNegocio(this string unidadNegocio)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioLongName.CondorTravel:
                    return UnidadNegocioKeys.CondorTravel;
                case UnidadNegocioLongName.DestinosMundiales:
                    return UnidadNegocioKeys.DestinosMundiales;
                case UnidadNegocioLongName.Interagencias:
                    return UnidadNegocioKeys.InterAgencias;
                default:
                    return null;
            }
        }

        public static string ToString(this ActionMethod actionMethod)
        {
            switch (actionMethod)
            {
                case ActionMethod.Create:
                    return RouteAction.Create;
                case ActionMethod.Update:
                    return RouteAction.Update;
                default:
                    return null;
            }
        }
        #endregion
    }
}