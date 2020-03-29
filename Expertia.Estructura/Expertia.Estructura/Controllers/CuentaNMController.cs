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

        protected override ControllerName _controllerName => ControllerName.CuentaNM;
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
                cuentasNMs = (IEnumerable<CuentaNM>)(_cuentaNMRepository.Read(_unidadNegocio))[OutParameter.CursorCuentaNM];
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


                    var responseCuentaNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.CuentaNMMethod, Method.POST, objEnvio, true, token);
                    if (responseCuentaNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaNM.Content);

                        foreach (var cuentaNM in cuentasNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                if (cuentaNM.eMailCli == jsResponse["EmailCli"])
                                {
                                    cuentaNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                    cuentaNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                    cuentaNM.idCuenta_Sf = jsResponse[OutParameter.SF_IdCuenta];

                                    /// Actualización de Cuenta NM
                                    var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                    cuentaNM.CodigoError = updateResponse[OutParameter.CodigoError].ToString();
                                    cuentaNM.MensajeError = updateResponse[OutParameter.MensajeError].ToString();
                                    //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        error = responseCuentaNM.StatusCode.ToString();
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
