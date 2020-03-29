﻿using Expertia.Estructura.Controllers.Base;
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
        protected override ControllerName _controllerName => ControllerName.OportunidadNM;
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

                        foreach (var oportunidadNM in oportunidadNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                if (oportunidadNM.idCuenta_SF == jsResponse["idCuenta_SF"])
                                {
                                    oportunidadNM.CodigoError = jsResponse[OutParameter.SF_CodigoError];
                                    oportunidadNM.MensajeError = jsResponse[OutParameter.SF_MensajeError];
                                    oportunidadNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad2];

                                    /// Actualización de estado de Oportunidad
                                    var updateResponse = _oportunidadNMRepository.Update(oportunidadNM);
                                    oportunidadNM.CodigoError = updateResponse[OutParameter.CodigoError].ToString();
                                    oportunidadNM.MensajeError = updateResponse[OutParameter.MensajeError].ToString();
                                }
                                
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
