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

                /// Consulta de PNRs a PTA
                var agenciasPnrs = (IEnumerable<AgenciaPnr>)(_operCollection[_unidadNegocio] = _fileCollection[_unidadNegocio].GetNewAgenciaPnr())[OutParameter.CursorAgenciaPnr];

                /// Configuraciones
                var authServer = ConfigAccess.GetValueInAppSettings("AUTH_SERVER");
                var authMethodName = ConfigAccess.GetValueInAppSettings("AUTH_METHODNAME");
                var crmServer = ConfigAccess.GetValueInAppSettings("CRM_SERVER");
                var crmSubcodigoMethod = ConfigAccess.GetValueInAppSettings("FILE_METHODNAME");

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetToken(authServer, authMethodName, Method.POST);

                // Consulta File (1 x 1) al API de Salesforce
                foreach (var agenciaPnr in agenciasPnrs)
                {
                    /// Consulta a Salesforce
                    JsonManager.LoadText(RestBase.Execute(crmServer, crmSubcodigoMethod, Method.POST, agenciaPnr, true, token).Content);
                    agenciaPnr.CodigoError = JsonManager.GetSetting("CODIGO_ERROR");
                    agenciaPnr.MensajeError = JsonManager.GetSetting("MENSAJE_ERROR");
                    agenciaPnr.DkAgencia = int.Parse(JsonManager.GetSetting("ID_CUENTA_SF"));
                    agenciaPnr.IdOportunidad = JsonManager.GetSetting("ID_OPORTUNIDAD_SF");

                    ///

                }

                foreach (var agenciaPnr in agenciasPnrs)
                {
                    var fileSalesforce = new
                    {
                        Id_Oportunidad_Sf = "00663000008wJpzAAE",
                        Resumen = new FileSalesforce
                        {
                            Objeto = "FILE",
                            Estado_File = "Anulado",
                            Unidad_Negocio = agenciaPnr.UnidadNegocio,
                            Sucursal = "",
                            Nombre_Grupo = "Philips x 2",
                            Counter = "Katherine Perez",
                            Fecha_Apertura = new DateTime(2019, 11, 01),
                            Fecha_Inicio = new DateTime(2019, 11, 15),
                            Fecha_Fin = new DateTime(2019, 11, 17),
                            Cliente = "Luciana Travel",
                            Subcodigo = "PREMIO PUBLICO",
                            Condicion_Pago = "C30R",
                            Num_Pasajeros = "2",
                            Costo = "118",
                            Venta = "118",
                            Comision_Agencia = "1"
                        }
                    };
                    var file = RestBase.Execute(authServer, authMethodName, Method.POST, agenciaPnr, true, token);
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
