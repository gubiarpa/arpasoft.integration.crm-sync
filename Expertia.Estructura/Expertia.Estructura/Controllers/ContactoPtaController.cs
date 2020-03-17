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
    [RoutePrefix(RoutePrefix.ContactoPta)]
    public class ContactoPtaController : BaseController<object>
    {
        #region Properties
        private IContactoPtaRepository _contactoPtaRepository;

        protected override ControllerName _controllerName => ControllerName.ContactoPta;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<ContactoPta> contactoPtaList = null;
            string exceptionMsg = string.Empty;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);

                /// I. Consulta de Contactos a BD
                contactoPtaList = (IEnumerable<ContactoPta>)_contactoPtaRepository.GetContactos()[OutParameter.CursorContactoPta];
                if (contactoPtaList == null || contactoPtaList.ToList().Count.Equals(0)) return Ok(contactoPtaList);

                /// Obtiene Token de envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// Por cada Contacto...
                foreach (var contactoPta in contactoPtaList)
                {
                    {
                        try
                        {
                            /// Envío de Contacto a Salesforce
                            contactoPta.UnidadNegocio = unidadNegocio.Descripcion;
                            contactoPta.CodigoError = contactoPta.MensajeError = string.Empty;

                            //var contactoPtaSf = ToSalesforceEntity(contactoPta);
                            var responseContactoPta = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.ContactoPtaMethod, Method.POST, contactoPta.ToSalesforceEntity(), true, token);
                            if (responseContactoPta.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseContactoPta.Content);
                                contactoPta.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                                contactoPta.MensajeError = jsonResponse[OutParameter.SF_MensajeError];
                                contactoPta.IdCuentaCrm = jsonResponse[OutParameter.SF_IdCuenta];
                                contactoPta.IdContactoCrm = jsonResponse[OutParameter.SF_IdContacto];

                                /// Actualización de resultado de Salesforce de Contacto en BD
                                var updateResponse = _contactoPtaRepository.Update(contactoPta);
                                contactoPta.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            contactoPta.CodigoError = ApiResponseCode.ErrorCode;
                            contactoPta.MensajeError = ex.Message;
                        }
                    }
                }
                return Ok(new { Contactos = contactoPtaList });
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = exceptionMsg,
                    LegacySystems = contactoPtaList
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    _contactoPtaRepository = new ContactoPta_IA_Repository(UnidadNegocioKeys.DestinosMundiales);
                    break;
                case UnidadNegocioKeys.Interagencias:
                    _contactoPtaRepository = new ContactoPta_IA_Repository();
                    break;
                case UnidadNegocioKeys.AppWebs:
                    _contactoPtaRepository = new ContactoPta_AW_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
