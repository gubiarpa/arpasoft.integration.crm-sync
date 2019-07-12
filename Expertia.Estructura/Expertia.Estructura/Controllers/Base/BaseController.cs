using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Http;

namespace Expertia.Estructura.Controllers.Base
{
    /// <summary>
    /// Mantenimiento de entidad
    /// </summary>
    /// <typeparam name="T">Entidad Genérica</typeparam>
    public abstract class BaseController<T> : ApiController, IClientFeatures
    {
        #region Properties
        protected IFileIO logFileMainManager;
        protected IFileIO logFileExtendManager;

        public string Ip => HttpContext.Current.Request.UserHostAddress;
        public string Method => HttpContext.Current.Request.HttpMethod;
        public string Uri => HttpContext.Current.Request.Url.LocalPath;
        #endregion

        #region Constructor
        public BaseController()
        {
            LoadLogSettings();
        }
        #endregion

        #region Json
        protected string Stringify(object obj, bool indented = false)
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

        #region LogMethods
        protected void LoadLogSettings()
        {
            try
            {
                #region LogMainManager
                var mainFilePath = ConfigAccess.GetValueInAppSettings("LogMainFilePath");
                var mainFileName = ConfigAccess.GetValueInAppSettings("LogMainFileName");
                var mainFileDateFormat = ConfigAccess.GetValueInAppSettings("LogMainFileDateFormat");

                var mainLineFormat = ConfigAccess.GetValueInAppSettings("LogMainLineFormat");
                var mainLineDateFormat = ConfigAccess.GetValueInAppSettings("LogMainLineDateFormat");

                logFileMainManager = new FileIO(mainFilePath, string.Format(mainFileName, DateTime.Now.ToString(mainFileDateFormat))) { LogFormat = mainLineFormat };
                #endregion

                #region LogExtendManager
                var extendFilePath = ConfigAccess.GetValueInAppSettings("LogExtendFilePath");
                var extendFileName = ConfigAccess.GetValueInAppSettings("LogExtendFileName");
                var extendfileDateFormat = ConfigAccess.GetValueInAppSettings("LogExtendFileDateFormat");

                var extendLineFormat = ConfigAccess.GetValueInAppSettings("LogExtendLineFormat");
                var extendLineDateFormat = ConfigAccess.GetValueInAppSettings("LogExtendLineDateFormat");

                logFileExtendManager = new FileIO(extendFilePath, string.Format(extendFileName, DateTime.Now.ToString(extendfileDateFormat))) { LogFormat = extendLineFormat };
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void WriteLog()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region HttpMethods
        /// <summary>
        /// Agrega una entidad
        /// </summary>
        /// <param name="entity">Nueva entidad</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public abstract IHttpActionResult Create(T entity);

        /// <summary>
        /// Actualiza una entidad
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        /// <returns>Status de transacción</returns>
        [HttpPost]
        public abstract IHttpActionResult Update(T entity);

        /// <summary>
        /// Método de Prueba
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteAction.Read)]
        public IHttpActionResult Read()
        {
            try
            {
                Guid idOperation = Guid.NewGuid();

                object obj = new
                {
                    IdOperation = idOperation,
                    Ip = Ip,
                    DateResponse = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Sender = "Expertia"
                };

                var request = new string[] { string.Format("Request: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")), Stringify(obj, true) };
                logFileExtendManager.WriteContent(request);
                logFileMainManager.WriteContent(string.Format("IP: {0} :: Uri: {1} :: Method: {2} ", Ip, Uri, Method));
                return Ok(obj);
            }
            catch (Exception ex)
            {
                logFileMainManager.WriteContent(ex.Message);
                return InternalServerError();
            }
        }
        #endregion
    }
}
