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
    [RoutePrefix(RoutePrefix.ChatterNM)]
    public class ChatterNMController : BaseController<object>
    {
        #region Properties
        private IChatterNMRepository _chatterNMRepository;
        protected override ControllerName _controllerName => ControllerName.ChatterNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<ChatterNM> chatterNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Informacion Pago NM
                chatterNMs = (IEnumerable<ChatterNM>)(_chatterNMRepository.Send(_unidadNegocio))[OutParameter.CursorChatterNM];
                if (chatterNMs == null || chatterNMs.ToList().Count.Equals(0)) return Ok(chatterNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación del canal de comunicacion para envio a Salesforce
                var chatterNMSF = new List<object>();
                foreach (var chatter in chatterNMs)
                {
                    chatterNMSF.Add(chatter.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de Informacion de Canal de Comunicacion a Salesforce
                    ClearQuickLog("body_request.json", "ChatterNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = chatterNMSF };
                    QuickLog(objEnvio, "body_request.json", "ChatterNM"); /// ♫ Trace


                    var responseChatterNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.ChatterNMMethod, Method.POST, objEnvio, true, token);
                    if (responseChatterNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseChatterNM.Content);

                        foreach (var chatterNM in chatterNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                chatterNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                chatterNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseChatterNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { ChatterNM = chatterNMs});
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
                    LegacySystems = chatterNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _chatterNMRepository = new ChatterNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
