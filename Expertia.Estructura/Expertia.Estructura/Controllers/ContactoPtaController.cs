using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
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
    [RoutePrefix(RoutePrefix.ContactoPta)]
    public class ContactoPtaController : BaseController<object>
    {
        #region Properties
        private IContactoPtaRepository _contactoPtaRepository;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<ContactoPta> contactoPtaList = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);

                /// I. Consulta de Contactos a BD
                contactoPtaList = (IEnumerable<ContactoPta>)_contactoPtaRepository.GetContactos()[OutParameter.CursorContactoPta];
                if (contactoPtaList == null || contactoPtaList.ToList().Count.Equals(0)) return Ok(contactoPtaList);

                /// Obtiene Token de envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                /// Por cada Contacto...
                foreach (var contactoPta in contactoPtaList)
                {
                    {
                        try
                        {
                            /// Envío de Contacto a Salesforce
                            contactoPta.UnidadNegocio = unidadNegocio.Descripcion;
                            contactoPta.CodigoError = contactoPta.MensajeError = string.Empty;

                            var contactoPtaSf = ToSalesforceEntity(contactoPta);
                            var responseContactoPta = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.ContactoPtaMethod, Method.POST, contactoPtaSf, true, token);
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
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
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

        #region Salesforce
        private object ToSalesforceEntity(ContactoPta contactoPta)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        accion = contactoPta.Accion,
                        dkCuenta = contactoPta.DkAgencia,
                        primerNombre = contactoPta.PrimerNombre,
                        segundoNombre = contactoPta.SegundoNombre,
                        apellidoPaterno = contactoPta.ApellidoPaterno,
                        apellidoMaterno = contactoPta.ApellidoMaterno,
                        estadoCivil = contactoPta.EstadoCivil,
                        cargo = contactoPta.Cargo,
                        tipoContacto = contactoPta.TipoContacto,
                        genero = contactoPta.Genero,
                        fechaNacimiento = contactoPta.FechaNacimiento.ToString("dd/MM/yyyy"),
                        tieneHijos = contactoPta.TieneHijos,
                        tipoDocumentoIdentidad = contactoPta.TipoDocumentoIdentidad,
                        documentoIdentidad = contactoPta.DocumentoIdentidad,
                        direccion = contactoPta.Direccion,
                        twitter = contactoPta.Twitter,
                        facebook = contactoPta.Facebook,
                        linkedin = contactoPta.LinkedIn,
                        instagram = contactoPta.Instagram,
                        tipoPresenciaDigital = contactoPta.TipoPresenciaDigital,
                        urlPresenciaDigital = contactoPta.UrlPresenciaDigital,
                        tipoTelefono1 = contactoPta.TipoTelefono1,
                        telefono1 = contactoPta.Telefono1,
                        tipoTelefono2 = contactoPta.TipoTelefono2,
                        telefono2 = contactoPta.Telefono2,
                        telefonoEmergencia = contactoPta.TelefonoEmergencia,
                        tipoCorreo = contactoPta.TipoCorreo,
                        correo = contactoPta.Correo,
                        estadoContacto = contactoPta.EstadoContacto,
                        esContactoMarketing = contactoPta.EsContactoMarketing
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
