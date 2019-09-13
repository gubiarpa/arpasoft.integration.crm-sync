using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Filters;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
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
        protected ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        #endregion

        #region Constructor
        public BaseController()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
            _clientFeatures = new ClientFeatures();
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
            if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.CondorTravel.GetName())))
            {
                return UnidadNegocioKeys.CondorTravel;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.DestinosMundiales.GetName())))
            {
                return UnidadNegocioKeys.DestinosMundiales;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.InterAgencias.GetName())))
            {
                return UnidadNegocioKeys.InterAgencias;
            }
            else if (unidadNegocioName.Equals(ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.NuevoMundo.GetName())))
            {
                return UnidadNegocioKeys.NuevoMundo;
            }
            return null;
        }
        #endregion
    }
}
