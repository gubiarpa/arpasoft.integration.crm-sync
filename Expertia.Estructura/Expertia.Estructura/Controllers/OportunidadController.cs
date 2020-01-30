using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
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
    [RoutePrefix(RoutePrefix.Oportunidad)]
    public class OportunidadController : BaseController<object>
    {
        #region Properties
        private IOportunidadRepository _oportunidadRepository;
        #endregion

        #region Constructor
        public OportunidadController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<Oportunidad> oportunidades = null;
            string exceptionMsg = string.Empty;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);

                /// I. Consulta de Oportunidades
                oportunidades = (IEnumerable<Oportunidad>)_oportunidadRepository.GetOportunidades()[OutParameter.CursorOportunidad];
                if (oportunidades == null || oportunidades.ToList().Count.Equals(0)) return Ok(oportunidades);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod, Method.POST);

                foreach (var oportunidad in oportunidades)
                {
                    /// II. Enviar Oportunidad a Salesforce
                    try
                    {
                        oportunidad.CodigoError = oportunidad.MensajeError = string.Empty;
                        var responseOportunidad = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.OportunidadMethod, Method.POST, oportunidad.ToSalesforceEntity(), true, token);
                        if (responseOportunidad.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseOportunidad.Content);
                            try
                            {
                                oportunidad.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                                oportunidad.MensajeError = jsonResponse[OutParameter.SF_MensajeError];
                                if (string.IsNullOrEmpty(oportunidad.IdOportunidad))
                                    oportunidad.IdOportunidad = jsonResponse[OutParameter.SF_IdOportunidad];
                            }
                            catch
                            {
                            }

                            try
                            {
                                /// Actualización de estado de Oportunidad a PTA
                                var updateResponse = _oportunidadRepository.Update(oportunidad);
                                oportunidad.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionMsg = ex.Message;
                        oportunidad.CodigoError = ApiResponseCode.ErrorCode;
                        oportunidad.MensajeError = ex.Message;
                    }
                }

                return Ok(oportunidades);
            }
            catch (Exception ex)
            {
                oportunidades = null;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = exceptionMsg,
                    LegacySystems = oportunidades
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    _oportunidadRepository = new Oportunidad_IA_Repository(UnidadNegocioKeys.DestinosMundiales);
                    break;
                case UnidadNegocioKeys.Interagencias:
                    _oportunidadRepository = new Oportunidad_IA_Repository();
                    break;
                case UnidadNegocioKeys.AppWebs:
                    _oportunidadRepository = new Oportunidad_AW_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
