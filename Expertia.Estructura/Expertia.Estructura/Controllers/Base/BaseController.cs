﻿using Expertia.Estructura.Controllers.Behavior;
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
    public abstract class BaseController<T> : ApiController
    {
        #region Properties
        protected ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        #endregion

        #region Constructor
        public BaseController()
        {
            _logFileManager = new LogFileManager("LogPath", "LogName");
            _clientFeatures = new ClientFeatures();
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
                    IpClient = _clientFeatures.IP,
                    DateResponse = DateTime.Now.ToString(FormatTemplate.LongDate),
                    Sender = "Expertia"
                };

                _logFileManager.WriteLine(LogType.Info, idOperation.ToString());
                return Ok(obj);
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError();
            }
        }
        #endregion
    }
}
