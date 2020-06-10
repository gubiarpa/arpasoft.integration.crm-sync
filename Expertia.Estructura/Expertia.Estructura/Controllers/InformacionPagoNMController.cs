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
    /// CRMEC003_5 : Registro de Información de pago
    [RoutePrefix(RoutePrefix.InformacionPagoNM)]
    public class InformacionPagoNMController : BaseController
    {
        #region Properties
        private IInformacionPagoNMRepository _informacionPagoNMRepository;
        protected override ControllerName _controllerName => ControllerName.InformacionPagoNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<InformacionPagoNM> informacionPagoNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Informacion Pago NM
                informacionPagoNMs = (IEnumerable<InformacionPagoNM>)(_informacionPagoNMRepository.GetInformacionPago(_unidadNegocio))[OutParameter.CursorInformacionPagoNM];
                if (informacionPagoNMs == null || informacionPagoNMs.ToList().Count.Equals(0)) return Ok(informacionPagoNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de hotel para envio a Salesforce
                var informacionPagoNMSF = new List<object>();
                foreach (var informacionPago in informacionPagoNMs)
                    informacionPagoNMSF.Add(informacionPago.ToSalesforceEntity());

                try
                {
                    /// Envío de Informacion de Pago a Salesforce
                    ClearQuickLog("body_request.json", "InformacionPagoNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = informacionPagoNMSF };
                    QuickLog(objEnvio, "body_request.json", "InformacionPagoNM"); /// ♫ Trace


                    var responseInformacionPagoNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.InformacionPagoNMMethod, Method.POST, objEnvio, true, token);
                    if (responseInformacionPagoNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                       
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseInformacionPagoNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "InformacionPagoNM"); /// ♫ Trace
                        foreach (var informacionPagoNM in informacionPagoNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                informacionPagoNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                informacionPagoNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                informacionPagoNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                informacionPagoNM.IdInformacionPago_SF = jsResponse[OutParameter.SF_IdInformacionPago];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {

                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseInformacionPagoNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "InformacionPagoNM"); /// ♫ Trace
                        error = responseInformacionPagoNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { InformacionPagoNM = informacionPagoNMs});
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
                    LegacySystems = informacionPagoNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _informacionPagoNMRepository = new InformacionPagoNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }

    }
}
