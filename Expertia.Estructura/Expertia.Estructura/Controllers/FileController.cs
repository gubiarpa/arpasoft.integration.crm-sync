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
using System.Linq;
using System.Threading.Tasks;
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
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            object files = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// Consulta de PNRs a PTA
                var agenciasPnrs = (IEnumerable<AgenciaPnr>)(_operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetNewAgenciaPnr())[OutParameter.CursorAgenciaPnr];
                if (agenciasPnrs == null || agenciasPnrs.ToList().Count.Equals(0)) return Ok();

                /// Configuraciones
                var authServer = ConfigAccess.GetValueInAppSettings("AUTH_SERVER");
                var authMethodName = ConfigAccess.GetValueInAppSettings("AUTH_METHODNAME");
                var crmServer = ConfigAccess.GetValueInAppSettings("CRM_SERVER");
                var crmPnrMethod = ConfigAccess.GetValueInAppSettings("PNR_METHODNAME");

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetToken(authServer, authMethodName, Method.POST);

                /// Consulta File (1 x 1) al API de Salesforce
                //var taskList = new List<Task>();
                //taskList.Add(new Task(() => { }));
                //var tasks = taskList.ToArray();
                
                
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    /// AgencyPnr
                    try
                    {
                        agenciaPnr.UnidadNegocio = unidadNegocio.Descripcion;
                        agenciaPnr.IdOportunidad = agenciaPnr.CodigoError = agenciaPnr.MensajeError = string.Empty;
                        var response = RestBase.Execute(crmServer, crmPnrMethod, Method.POST, agenciaPnr, true, token);
                        JsonManager.LoadText(response.Content);
                        agenciaPnr.CodigoError = JsonManager.GetSetting("CODIGO_ERROR");
                        agenciaPnr.MensajeError = JsonManager.GetSetting("MENSAJE_ERROR");
                        agenciaPnr.DkAgencia = int.Parse(JsonManager.GetSetting("ID_CUENTA_SF"));
                        agenciaPnr.IdOportunidad = JsonManager.GetSetting("ID_OPORTUNIDAD_SF");
                    }
                    catch
                    {
                    }

                    /// FileBoleto
                    try
                    {
                        if (!string.IsNullOrEmpty(agenciaPnr.IdOportunidad))
                        {
                            _operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetNewFileBoleto(agenciaPnr);
                        }
                    }
                    catch
                    {
                    }
                }
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
                case UnidadNegocioKeys.Interagencias:
                    _fileCollection.Add(UnidadNegocioKeys.Interagencias, new File_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
