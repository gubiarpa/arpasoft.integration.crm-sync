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
    /// CRMEC001 : Registro de Cuenta
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
            List<RptaCuentaSF> ListRptaCuentaSF_Fail = new List<RptaCuentaSF>();
            RptaCuentaSF _rptaCuentaSF = null;
                        
            object SFResponse = null;
            string exceptionMsg = string.Empty;
            object objEnvio = null;

            try
            {                                
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Cuentas NM                                
                cuentasNMs = (IEnumerable<CuentaNM>)_cuentaNMRepository.GetCuentas()[OutParameter.CursorCuentaNM];
                if (cuentasNMs == null || cuentasNMs.ToList().Count.Equals(0)) return Ok(false);

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
                    objEnvio = new { listadatosCuenta = cuentaNMSF };
                    QuickLog(objEnvio, "body_request.json", "CuentaNM", previousClear: true); /// ♫ Trace
                    
                    var responseCuentaNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.CuentaNMMethod, Method.POST, objEnvio, true, token);
                    if (responseCuentaNM.StatusCode.Equals(HttpStatusCode.OK))
                    {   
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "CuentaNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        foreach (var jsResponse in jsonResponse["respuestas"])
                        {
                            try
                            {
                                _rptaCuentaSF = new RptaCuentaSF();

                                _rptaCuentaSF.CodigoError = "OK";
                                _rptaCuentaSF.MensajeError = "TST";
                                _rptaCuentaSF.idCuenta_SF = "001P002201bpIOWIC4";
                                _rptaCuentaSF.Identificador_NM = "2";

                                _rptaCuentaSF.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                _rptaCuentaSF.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                _rptaCuentaSF.idCuenta_SF = jsResponse[OutParameter.SF_IdCuenta2];
                                _rptaCuentaSF.Identificador_NM = jsResponse[OutParameter.SF_IdentificadorNM];

                                /// Actualización de estado de la Cuenta
                                var updateResponse = _cuentaNMRepository.Update(_rptaCuentaSF);
                                
                                if (Convert.IsDBNull(updateResponse[OutParameter.IdActualizados]) == true || updateResponse[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updateResponse[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    exceptionMsg = exceptionMsg + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaCuentaSF.Identificador_NM.ToString() + "||||";
                                    ListRptaCuentaSF_Fail.Add(_rptaCuentaSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionMsg = exceptionMsg + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaCuentaSF_Fail.Add(_rptaCuentaSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        exceptionMsg = responseCuentaNM.StatusCode.ToString();
                        if (responseCuentaNM != null && responseCuentaNM.Content != null)
                        {
                            QuickLog(responseCuentaNM.Content, "body_response.json", "CuentaNM", previousClear: true); /// ♫ Trace
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
                cuentasNMs = null;
                exceptionMsg = exceptionMsg + " / " + ex.Message;
                return InternalServerError(ex);                
            }
            finally
            {
                (new
                {
                    Request = objEnvio,
                    Response = SFResponse,
                    Rpta_NoUpdate_Fail = ListRptaCuentaSF_Fail,                    
                    Exception = exceptionMsg 
                    //LegacySystems = cuentasNMs
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
