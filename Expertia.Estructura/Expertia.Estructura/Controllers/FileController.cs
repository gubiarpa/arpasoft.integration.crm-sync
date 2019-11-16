using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad Exclusiva para Condor Travel e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.File)]
    public class FileController : BaseController<AgenciaPnr>
    {
        #region Properties
        private IDictionary<UnidadNegocioKeys?, IFileRepository> _fileCollection;
        #endregion

        #region Constructor
        public FileController()
        {
            _fileCollection = new Dictionary<UnidadNegocioKeys?, IFileRepository>();
        }
        #endregion

        #region PublicMethods
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            object files = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;
                // Consultar PNRs (Lista)
                var agenciasPnrs = (IEnumerable<AgenciaPnr>)
                    (_operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetNewAgenciaPnr())[OutParameter.CursorAgenciaPnr];
                // Consulta Token para invocar un API
                var server = ConfigAccess.GetValueInAppSettings("AUTH_SERVER");
                var methodName = ConfigAccess.GetValueInAppSettings("AUTH_METHODNAME");
                var token = RestBase.GetToken(server, methodName, Method.POST);
                // Consulta File (1 x 1) al API de Salesforce
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    var file = RestBase.Execute(server, methodName, Method.POST, agenciaPnr, true, token);
                }
                // Consultar File (1 x 1)
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    var file = _fileCollection[_unidadNegocio].GetNewFile(agenciaPnr);
                }
                _instants[InstantKey.Oracle] = DateTime.Now;
                files = new { Interagencias = agenciasPnrs };
                return Ok(files);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                files.TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region NotImplemented
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    break;
                case UnidadNegocioKeys.InterAgencias:
                    _fileCollection.Add(UnidadNegocioKeys.InterAgencias, new File_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
