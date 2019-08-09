﻿using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Filters;
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
    [BasicAuthentication]
    public abstract class BaseController<T> : ApiController
    {
        #region Properties
        private ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        #endregion

        #region Constructor
        public BaseController()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
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

                _logFileManager.WriteLine(LogType.Info, string.Format("Success: {0}", idOperation.ToString()));
                return Ok(obj);
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, string.Format("Fail: {0}", ex.Message));
                return InternalServerError();
            }
        }
        #endregion

        #region Log
        protected void WriteEntityInLog(T entity)
        {
            WriteObjectInLog(entity);
        }

        protected void WriteObjectInLog(object obj, LogType logType = LogType.Info)
        {
            _logFileManager.WriteText(Stringify(BuildObject(obj, logType), true) + "\n");
        }

        private object BuildObject(object obj, LogType logType = LogType.Info)
        {
            try
            {
                var clientFeatures = new ClientFeatures();
                return new
                {
                    Client = new
                    {
                        IP = clientFeatures.IP,
                        Method = clientFeatures.Method,
                        Log = logType,
                        Url = clientFeatures.URL,
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    },
                    Entity = obj
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
