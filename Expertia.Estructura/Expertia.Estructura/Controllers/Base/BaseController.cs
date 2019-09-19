using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Filters;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Collections.Generic;
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
        protected ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        protected IDictionary<UnidadNegocioKeys?, ICrud<T>> _crmCollection;
        protected Operation _operation;
        #endregion

        #region Constructor
        public BaseController()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
            _clientFeatures = new ClientFeatures();
            _crmCollection = new Dictionary<UnidadNegocioKeys?, ICrud<T>>();
            _operation = new Operation();
        }
        #endregion

        #region HttpMethods
        [HttpPost]
        public abstract IHttpActionResult Create(T entity);

        [HttpPost]
        public abstract IHttpActionResult Update(T entity);

        [HttpGet]
        [Route(RouteAction.Read)]
        public IHttpActionResult Read()
        {
            try
            {
                var testMessage = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff");
                testMessage.WriteLogObject(_logFileManager, _clientFeatures);
                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success
                    },
                    Entity = testMessage
                });
            }
            catch (Exception ex)
            {
                ex.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError();
            }
        }
        #endregion

        #region Auxiliar
        protected UnidadNegocioKeys? GetUnidadNegocio(string unidadNegocioName) // Ejm. "CONDOR TRAVEL"
        {
            if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.CondorTravel.GetKeyName())))
            {
                return UnidadNegocioKeys.CondorTravel;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.DestinosMundiales.GetKeyName())))
            {
                return UnidadNegocioKeys.DestinosMundiales;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.InterAgencias.GetKeyName())))
            {
                return UnidadNegocioKeys.InterAgencias;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.NuevoMundo.GetKeyName())))
            {
                return UnidadNegocioKeys.NuevoMundo;
            }
            return null;
        }

        //protected string GetUnidadNegocio(UnidadNegocioKeys? unidadNegocioKey)
        //{
        //    switch (unidadNegocioKey)
        //    {
        //        case UnidadNegocioKeys.CondorTravel:
        //            return UnidadNegocioNames.CondorTravel;
        //        case UnidadNegocioKeys.DestinosMundiales:
        //            return UnidadNegocioNames.DestinosMundiales;
        //        case UnidadNegocioKeys.NuevoMundo:
        //            return UnidadNegocioNames.NuevoMundo;
        //        case UnidadNegocioKeys.InterAgencias:
        //            return UnidadNegocioNames.InterAgencias;
        //        default:
        //            return null;
        //    }
        //}

        protected abstract UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey);
        #endregion
    }
}
