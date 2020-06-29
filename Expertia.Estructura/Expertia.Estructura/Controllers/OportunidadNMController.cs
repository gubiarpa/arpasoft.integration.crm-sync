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
    /// CRMEC002 : Envío de datos de Oportunidad y Actualización 
    [RoutePrefix(RoutePrefix.OportunidadNM)]
    public class OportunidadNMController : BaseController
    {
        #region Properties
        private IOportunidadNMRepository _oportunidadNMRepository;
        protected override ControllerName _controllerName => ControllerName.OportunidadNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<OportunidadNM> oportunidadNMs = null;
            List<RptaOportunidadSF> ListRptaOportunidadSF_Fail = new List<RptaOportunidadSF>();
            RptaOportunidadSF _rptaOportunidadSF = null;
                        
            object SFResponse = null;
            string exceptionMsg = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Oportunidades de Nuevo Mundo
                oportunidadNMs = (IEnumerable<OportunidadNM>)_oportunidadNMRepository.GetOportunidades()[OutParameter.CursorOportunidad];
                if (oportunidadNMs == null || oportunidadNMs.ToList().Count.Equals(0)) return Ok(false);

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
                    objEnvio = new { ListadatosOportunidades = oportunidadNMSF};                    
                    QuickLog(objEnvio, "body_request.json", "OportunidadNM",previousClear: true); /// ♫ Trace

                    var responseOportunidadNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.OportunidadNMMethod, Method.POST, objEnvio, true, token);
                    if (responseOportunidadNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseOportunidadNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "OportunidadNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        foreach (var jsResponse in jsonResponse["respuestas"])
                        {
                            try
                            {
                                _rptaOportunidadSF = new RptaOportunidadSF();

                                _rptaOportunidadSF.CodigoError = "OK";
                                _rptaOportunidadSF.MensajeError = "TST";
                                _rptaOportunidadSF.idOportunidad_SF = "001P000001bpIOWIC4";
                                _rptaOportunidadSF.Identificador_NM = "2";

                                _rptaOportunidadSF.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                _rptaOportunidadSF.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                _rptaOportunidadSF.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad2];
                                _rptaOportunidadSF.Identificador_NM = jsResponse[OutParameter.SF_IdentificadorNM];

                                /// Actualización de estado de Oportunidad
                                var updateResponse = _oportunidadNMRepository.Update(_rptaOportunidadSF);

                                if (Convert.IsDBNull(updateResponse[OutParameter.IdActualizados]) == true || updateResponse[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updateResponse[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    exceptionMsg = exceptionMsg +  "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaOportunidadSF.Identificador_NM.ToString() +  "||||";
                                    ListRptaOportunidadSF_Fail.Add(_rptaOportunidadSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                            }
                            catch (Exception ex)
                            {                                
                                exceptionMsg = exceptionMsg  + "Error en el Proceso de Actualizacion - Response SalesForce : " +  ex.Message + "||||";
                                ListRptaOportunidadSF_Fail.Add(_rptaOportunidadSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }             
                    }
                    else 
                    {
                        exceptionMsg = responseOportunidadNM.StatusCode.ToString();
                        if (responseOportunidadNM != null && responseOportunidadNM.Content != null)
                        {
                            QuickLog(responseOportunidadNM.Content, "body_response.json", "OportunidadNM", previousClear: true); /// ♫ Trace
                        }
                    }
                }
                catch (Exception ex)
                {
                    exceptionMsg = ex.Message;
                }

                return Ok(true);
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
                    Request = objEnvio,
                    Response = SFResponse,
                    Rpta_NoUpdate_Fail = ListRptaOportunidadSF_Fail,                    
                    Exception = exceptionMsg
                    //LegacySystems = oportunidadNMs
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
