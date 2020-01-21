using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaPta)]
    public class CuentaPtaController : BaseController<object>
    {
        #region Properties
        private ICuentaPtaRepository _cuentaPtaRepository;
        #endregion

        #region Constructor
        public CuentaPtaController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<CuentaPta> cuentasPtas = null;
            string error = string.Empty;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Cuentas PTA
                cuentasPtas = (IEnumerable<CuentaPta>)(_cuentaPtaRepository.Read())[OutParameter.CursorCuentaPta];
                if (cuentasPtas == null || cuentasPtas.ToList().Count.Equals(0)) return Ok(cuentasPtas);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                foreach (var cuentaPta in cuentasPtas)
                {
                    try
                    {
                        /// Envío de CuentaPTA a Salesforce
                        cuentaPta.UnidadNegocio = unidadNegocio.Descripcion;
                        cuentaPta.CodigoError = cuentaPta.MensajeError = string.Empty;
                        var responseCuentaPta = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.CuentaPtaMethod, Method.POST, cuentaPta.ToSalesforceEntity(), true, token);
                        if (responseCuentaPta.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonReponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaPta.Content);
                            cuentaPta.CodigoError = jsonReponse[OutParameter.SF_CodigoError];
                            cuentaPta.MensajeError = jsonReponse[OutParameter.SF_MensajeError];
                            cuentaPta.IdCuentaCrm = jsonReponse[OutParameter.SF_IdCuenta];

                            /// Actualización de estado de Cuenta PTA hacia PTA
                            var updateResponse = _cuentaPtaRepository.Update(cuentaPta);
                            cuentaPta.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                        }
                        else
                        {
                            cuentaPta.CodigoError = responseCuentaPta.StatusCode.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        cuentaPta.CodigoError = ApiResponseCode.ErrorCode;
                        cuentaPta.MensajeError = ex.Message;
                    }   
                }
                return Ok(new { CuentasPta = cuentasPtas });
            }
            catch (Exception ex)
            {
                //cuentasPtas = null;
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Error = error,
                    LegacySystems = cuentasPtas
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _cuentaPtaRepository = new CuentaPta_IA_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
