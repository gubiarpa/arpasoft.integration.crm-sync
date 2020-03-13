using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaNM)]
    public class CuentaNMController : BaseController<object>
    {
        #region Properties
        private ICuentaNMRepository _cuentaNMRepository;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<CuentaNM> cuentasNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Cuentas NM
                cuentasNMs = (IEnumerable<CuentaNM>)(_cuentaNMRepository.Read(_unidadNegocio))[OutParameter.CursorCuentaPta];
                if (cuentasNMs == null || cuentasNMs.ToList().Count.Equals(0)) return Ok(cuentasNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de cuenta para envio a Salesforce
                var cuentaNMSF = new List<object>();
                foreach (var cuenta in cuentasNMs)
                {
                    cuentaNMSF.Add(cuenta.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de CuentaNM a Salesforce
                    ClearQuickLog("body_request.json", "CuentaNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = cuentaNMSF };
                    QuickLog(objEnvio, "body_request.json", "CuentaNM"); /// ♫ Trace


                    var responseCuentaPta = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.CuentaNMMethod, Method.POST, objEnvio, true, token);
                    if (responseCuentaPta.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaPta.Content);

                        foreach (var cuenta in cuentasNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                cuenta.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                cuenta.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                cuenta.idCuentaCrm = jsResponse[OutParameter.SF_IdCuenta];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseCuentaPta.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                
                return Ok(new { CuentasNM = cuentasNMs });
            }
            catch (Exception ex)
            {
                error = error + " / " + ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Error = error,
                    LegacySystems = cuentasNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _cuentaNMRepository = new CuentaNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion

    }
}
