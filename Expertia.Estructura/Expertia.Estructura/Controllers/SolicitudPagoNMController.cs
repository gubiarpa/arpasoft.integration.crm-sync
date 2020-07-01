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
    [RoutePrefix(RoutePrefix.SolicitudPagoNM)]
    public class SolicitudPagoNMController : BaseController<object>
    {
        #region Properties
        private ISolicitudPagoNMRepository _solicitudPagoNMRepository;
        protected override ControllerName _controllerName => ControllerName.SolicitudPagoNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<SolicitudPagoNM> solicitudPagoNMs = null;
            List<RptaSolicitudPagoSF> ListRptaSolicitudPagoSF_Fail = null;
            RptaSolicitudPagoSF _rptaSolicitudPagoSF = null;

            object SFResponse = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Solicitud Pago NM
                solicitudPagoNMs = (IEnumerable<SolicitudPagoNM>)(_solicitudPagoNMRepository.GetSolicitudesPago())[OutParameter.CursorSolicitudPagoNM];
                if (solicitudPagoNMs == null || solicitudPagoNMs.ToList().Count.Equals(0)) return Ok(false);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de hotel para envio a Salesforce
                var solicitudPagoNMSF = new List<object>();
                foreach (var solicitudPago in solicitudPagoNMs)
                {
                    solicitudPagoNMSF.Add(solicitudPago.ToSalesforceEntity());
                }

                try
                {
                    /// Envío de CuentaNM a Salesforce                    
                    objEnvio = new { ListdatosSolicitudPago = solicitudPagoNMSF };
                    QuickLog(objEnvio, "body_request.json", "SolicitudPagoNM", previousClear: true); /// ♫ Trace

                    var responseSolicitudPagoNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.SolicitudPagoNMMethod, Method.POST, objEnvio, true, token);
                    if (responseSolicitudPagoNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseSolicitudPagoNM.Content);
                        SFResponse = jsonResponse["respuestas"];
                        QuickLog(SFResponse, "body_response.json", "SolicitudPagoNM", previousClear: true); /// ♫ Trace
                        ListRptaSolicitudPagoSF_Fail = new List<RptaSolicitudPagoSF>();

                        foreach (var solicitudPagoNM in jsonResponse["respuestas"])
                        {
                            try
                            {
                                #region Deserialize
                                _rptaSolicitudPagoSF = new RptaSolicitudPagoSF();

                                _rptaSolicitudPagoSF.CodigoError = solicitudPagoNM[OutParameter.SF_Codigo];
                                _rptaSolicitudPagoSF.MensajeError = solicitudPagoNM[OutParameter.SF_Mensaje];
                                _rptaSolicitudPagoSF.idOportunidad_SF = solicitudPagoNM[OutParameter.SF_IdOportunidad2];
                                _rptaSolicitudPagoSF.IdRegSolicitudPago_SF = solicitudPagoNM[OutParameter.SF_IdRegSolicitudPago];
                                _rptaSolicitudPagoSF.Identificador_NM = solicitudPagoNM[OutParameter.SF_IdentificadorNM];
                                #endregion

                                #region ReturnToDB
                                var updOperation = _solicitudPagoNMRepository.Update(_rptaSolicitudPagoSF);

                                if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaSolicitudPagoSF.Identificador_NM.ToString() + "||||";
                                    ListRptaSolicitudPagoSF_Fail.Add(_rptaSolicitudPagoSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaSolicitudPagoSF_Fail.Add(_rptaSolicitudPagoSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        QuickLog(SFResponse, "body_response.json", "SolicitudPagoNM", previousClear: true); /// ♫ Trace
                        error = responseSolicitudPagoNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                error = error + " / " + ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                if (objEnvio != null || SFResponse != null || ListRptaSolicitudPagoSF_Fail != null || string.IsNullOrEmpty(error) == false)
                {
                    (new
                    {
                        Request = objEnvio,
                        Response = SFResponse,
                        Rpta_NoUpdate_Fail = ListRptaSolicitudPagoSF_Fail,
                        Exception = error
                        //LegacySystems = solicitudPagoNMs
                    }).TryWriteLogObject(_logFileManager, _clientFeatures);
                }                    
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _solicitudPagoNMRepository = new SolicitudPagoNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
