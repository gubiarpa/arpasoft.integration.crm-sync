using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Filters;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Expertia.Estructura.Controllers.Base
{
    [BasicAuthentication]
    public abstract class BaseController<T> : ApiController
    {
        #region Properties
        protected ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        protected IDictionary<UnidadNegocioKeys?, ICrud<T>> _crmCollection;
        protected IDictionary<UnidadNegocioKeys?, Operation> _operCollection;
        #endregion

        #region Error
        protected string _codigoError;
        #endregion

        #region Constructor
        public BaseController()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
            _clientFeatures = new ClientFeatures();
            _crmCollection = new Dictionary<UnidadNegocioKeys?, ICrud<T>>();
            _operCollection = new Dictionary<UnidadNegocioKeys?, Operation>();
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
                testMessage.TryWriteLogObject(_logFileManager, _clientFeatures);
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
                ex.TryWriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError();
            }
        }
        #endregion

        #region Auxiliar
        protected UnidadNegocioKeys? GetUnidadNegocio(string unidadNegocioName)
        {
            if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.CondorTravel.GetKeyValues()).ToUpper().Split(Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.CondorTravel;
            }
            else if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.DestinosMundiales.GetKeyValues()).ToUpper().Split(Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.DestinosMundiales;
            }
            else if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.InterAgencias.GetKeyValues()).ToUpper().Split(Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.InterAgencias;
            }
            return null;
        }

        protected abstract UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey);
        #endregion
    }
}
