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
    [RoutePrefix(RoutePrefix.CanalComunicacionNM)]
    public class CanalComunicacionNMController : BaseController<object>
    {
        #region Properties
        private ICanalComunicacionNMRepository _canalComunicacionNMRepository;
        protected override ControllerName _controllerName => ControllerName.CanalComunicacionNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<CanalComunicacionNM> canalComunicacionNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Informacion Pago NM
                canalComunicacionNMs = (IEnumerable<CanalComunicacionNM>)(_canalComunicacionNMRepository.Send(_unidadNegocio))[OutParameter.CursorCanalComunicacionNM];
                if (canalComunicacionNMs == null || canalComunicacionNMs.ToList().Count.Equals(0)) return Ok(canalComunicacionNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación del canal de comunicacion para envio a Salesforce
                var canalComunicacionNMSF = new List<object>();
                foreach (var canalComunicacion in canalComunicacionNMs)
                {
                    canalComunicacionNMSF.Add(canalComunicacion.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de Informacion de Canal de Comunicacion a Salesforce
                    ClearQuickLog("body_request.json", "CanalComunicacionNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = canalComunicacionNMSF };
                    QuickLog(objEnvio, "body_request.json", "CanalComunicacionNM"); /// ♫ Trace


                    var responseCanalComunicacionNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.CanalComunicacionNMMethod, Method.POST, objEnvio, true, token);
                    if (responseCanalComunicacionNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseCanalComunicacionNM.Content);

                        foreach (var canalComunicacionNM in canalComunicacionNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                canalComunicacionNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                canalComunicacionNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseCanalComunicacionNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { CanalComunicacionNM = canalComunicacionNMs});
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
                    LegacySystems = canalComunicacionNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _canalComunicacionNMRepository = new CanalComunicacionNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
