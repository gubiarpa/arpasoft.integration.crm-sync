using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils.Behavior;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data;

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
                    case UnidadNegocioKeys.Interagencias: return UnidadNegocioShortNames.InterAgencias;
                    case UnidadNegocioKeys.AppWebs: return UnidadNegocioShortNames.AppWebs;
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
                    case UnidadNegocioKeys.CondorTravel_CL: return ConnectionKeys.CondorConnKey_CL;
                    case UnidadNegocioKeys.CondorTravel_EC: return ConnectionKeys.CondorConnKey_EC;
                    case UnidadNegocioKeys.CondorTravel_BR: return ConnectionKeys.CondorConnKey_BR;
                    case UnidadNegocioKeys.DestinosMundiales: return ConnectionKeys.DMConnKey;
                    case UnidadNegocioKeys.Interagencias: return ConnectionKeys.IAConnKey;
                    case UnidadNegocioKeys.AppWebs: return ConnectionKeys.AWConnKey;
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
                    return UnidadNegocioKeys.Interagencias;
                default:
                    return null;
            }
        }

        public static string ToLongName(this UnidadNegocioKeys? unidadNegocio)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.CondorTravel:
                    return UnidadNegocioLongName.CondorTravel;
                case UnidadNegocioKeys.DestinosMundiales:
                    return UnidadNegocioLongName.DestinosMundiales;
                case UnidadNegocioKeys.Interagencias:
                    return UnidadNegocioLongName.Interagencias;
                default:
                    return string.Empty;
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

        #region Rows
        public static string StringParse(this DataRow row, string fieldName)
        {
            try
            {
                return (row[fieldName] ?? string.Empty).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int IntParse(this DataRow row, string fieldName)
        {
            try
            {
                if (!int.TryParse(row[fieldName].ToString(), out int result)) result = 0;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static float FloatParse(this DataRow row, string fieldName)
        {
            try
            {
                if (!float.TryParse(row[fieldName].ToString(), out float result)) result = 0;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime DateTimeParse(this DataRow row, string fieldName)
        {
            try
            {
                if (!DateTime.TryParse(row[fieldName].ToString(), out DateTime result)) result = new DateTime(1900, 01, 01);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}