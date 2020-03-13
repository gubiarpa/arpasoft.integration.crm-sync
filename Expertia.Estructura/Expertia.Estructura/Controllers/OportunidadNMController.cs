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
    [RoutePrefix(RoutePrefix.OportunidadNM)]
    public class OportunidadNMController : BaseController<object>
    {
        #region Properties
        private IOportunidadNMRepository _oportunidadNMRepository;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<OportunidadNM> oportunidadNMs = null;
            string exceptionMsg = string.Empty;
            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Oportunidades de Nuevo Mundo
                oportunidadNMs = (IEnumerable<OportunidadNM>)_oportunidadNMRepository.GetOportunidades()[OutParameter.CursorOportunidad];
                if (oportunidadNMs == null || oportunidadNMs.ToList().Count.Equals(0)) return Ok(oportunidadNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de la oportunidad para envio a Salesforce
                var oportunidadNMSF = new List<object>();
                foreach (var oportunidad in oportunidadNMs)
                {
                    oportunidadNMSF.Add(oportunidad.ToSalesforceEntity());
                }
                    /// II. Enviar Oportunidad a Salesforce
                try
                {
                    //oportunidad.CodigoError = oportunidad.MensajeError = string.Empty;
                    ClearQuickLog("body_request.json", "OportunidadNM"); /// ♫ Trace
                    var objEnvio = new {cotizaciones = oportunidadNMSF};
                    QuickLog(objEnvio, "body_request.json", "OportunidadNM"); /// ♫ Trace

                    var responseOportunidadNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.OportunidadNMMethod, Method.POST, objEnvio, true, token);
                    if (responseOportunidadNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseOportunidadNM.Content);

                        foreach (var oportunidad in oportunidadNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                oportunidad.CodigoError = jsResponse[OutParameter.SF_CodigoError];
                                oportunidad.MensajeError = jsResponse[OutParameter.SF_MensajeError];

                                /// Actualización de estado de Oportunidad a PTA
                                //var updateResponse = _oportunidadRepository.Update(oportunidad);
                                //oportunidad.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else 
                    {
                        exceptionMsg = responseOportunidadNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    exceptionMsg = ex.Message;
                }

                return Ok(oportunidadNMs);
            }
            catch (Exception ex)
            {
                oportunidadNMs = null;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = exceptionMsg,
                    LegacySystems = oportunidadNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _oportunidadNMRepository = new OportunidadNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
