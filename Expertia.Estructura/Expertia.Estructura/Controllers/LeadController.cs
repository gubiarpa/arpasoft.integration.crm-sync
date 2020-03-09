using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Journeyou;
using Expertia.Estructura.Repository.Journeyou;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.LeadCT)]
    public class LeadController : BaseController
    {
        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Lead Lead)
        {
            string exceptionMsg = string.Empty;
            UnidadNegocioKeys? _unidadNegocioKey = null;
            object objEnvio = null;
            LeadResponse Rpta = new LeadResponse();
            try
            {
 
                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// Envío de cotizacion a Salesforce
                var leadSF = new List<object>();
                leadSF.Add(Lead.ToSalesforceEntity());

                try
                {
                    //ClearQuickLog("body_request.json", "CotizacionJY"); /// ♫ Trace
                    objEnvio = new { datos = leadSF };
                    //QuickLog(objEnvio, "body_request.json", "CotizacionJY"); /// ♫ Trace
                    var response = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.LeadCreateMethod, Method.POST, objEnvio, true, token);
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(response.Content);
                        foreach (var jsResponse in jsonResponse["Leads"])
                        {
                            Rpta.CodigoRetorno = jsResponse[OutParameter.SF_CodigoRetorno];
                            Rpta.MensajeRetorno = jsResponse[OutParameter.SF_MensajeRetorno];
                            Rpta.IdLeadSf = jsResponse[OutParameter.SF_IdLead];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Rpta.CodigoRetorno = ApiResponseCode.ErrorCode;
                    Rpta.MensajeRetorno = ex.Message;
                    exceptionMsg = ex.Message;
                }
                return Ok(Rpta);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Body = objEnvio.Stringify(true, false),
                    UnidadNegocio = _unidadNegocioKey.ToString(),
                    Exception = exceptionMsg
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            
            return unidadNegocioKey; // Devuelve el mismo parámetro
        }
       
    }
}
