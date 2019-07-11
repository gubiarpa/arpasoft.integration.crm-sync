using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers.Base
{
    /// <summary>
    /// Mantenimiento de entidad
    /// </summary>
    /// <typeparam name="T">Entidad Genérica</typeparam>
    public abstract class BaseController<T> : ApiController
    {
        #region Properties
        protected IFileIO fileManager;
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
                var filePath = ConfigAccess.GetValueInAppSettings("LogFilePath");
                var fileName = ConfigAccess.GetValueInAppSettings("LogFileName");
                var fileDateFormat = ConfigAccess.GetValueInAppSettings("LogFileDateFormat");

                var lineFormat = ConfigAccess.GetValueInAppSettings("LogLineFormat");
                var lineDateFormat = ConfigAccess.GetValueInAppSettings("LogLineDateFormat");

                fileManager = new FileIO(filePath, string.Format(fileName, DateTime.Now.ToString(fileDateFormat))) { LogFormat = lineFormat };
            }
            catch (Exception ex)
            {
                throw ex;
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
        #endregion
    }
}
