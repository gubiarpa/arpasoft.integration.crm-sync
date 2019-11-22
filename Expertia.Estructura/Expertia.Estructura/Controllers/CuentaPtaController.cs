using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaPta)]
    public class CuentaPtaController : BaseController<object>
    {
        #region Properties
        private ICuentaPtaRepository _cuentaPtaRepository;
        #endregion

        #region Constructor
        public CuentaPtaController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<CuentaPta> cuentaPtas = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Cuentas PTA
                cuentaPtas = (IEnumerable<CuentaPta>)(_cuentaPtaRepository.GetCuenta())[OutParameter.CursorCuentaPta];
                if (cuentaPtas == null || cuentaPtas.ToList().Count.Equals(0)) return Ok();

                /// Configuraciones
                var authServer = ConfigAccess.GetValueInAppSettings(SalesforceKeys.AuthServer);
                var authMethodName = ConfigAccess.GetValueInAppSettings(SalesforceKeys.AuthMethod);
                var crmServer = ConfigAccess.GetValueInAppSettings(SalesforceKeys.CrmServer);
                var crmSubcodigoMethod = ConfigAccess.GetValueInAppSettings(SalesforceKeys.CuentaPtaMethod);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetToken(authServer, authMethodName);

                foreach (var cuentaPta in cuentaPtas)
                {
                    var cuentaPtaSf = ToSalesforceEntity(cuentaPta);
                }

                return Ok(cuentaPtas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _cuentaPtaRepository = new CuentaPta_IA_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion

        #region Salesforce
        private object ToSalesforceEntity(CuentaPta cuentaPta)
        {
            try
            {
                return new
                {
                    Accion = cuentaPta.Accion,
                    DkCuenta = cuentaPta.DkCuenta,
                    RazonSocial = cuentaPta.RazonSocial,
                    NombreComercial = cuentaPta.NombreComercial,
                    TipoCuenta = cuentaPta.TipoCuenta,
                    Propietario = cuentaPta.Propietario,
                    FechaAniversario = cuentaPta.FechaAniversario,
                    TipoDocumentoIdentidad = cuentaPta.TipoDocumentoIdentidad,
                    DocumentoIdentidad = cuentaPta.DocumentoIdentidad,
                    TipoDireccion = cuentaPta.TipoDireccion,
                    DireccionResidencia = cuentaPta.DireccionResidencia,
                    PaisResidencia = cuentaPta.PaisResidencia,
                    DepartamentoResidencia = cuentaPta.DepartamentoResidencia,
                    CiudadResidencia = cuentaPta.CiudadResidencia,
                    DistritoResidencia = cuentaPta.DistritoResidencia,
                    DireccionFiscal = cuentaPta.DireccionFiscal,
                    TipoTelefono1 = cuentaPta.TipoTelefono1,
                    Telefono1 = cuentaPta.Telefono1,
                    TipoTelefono2 = cuentaPta.TipoTelefono2,
                    Telefono2 = cuentaPta.Telefono2,
                    TipoTelefono3 = cuentaPta.TipoTelefono3,
                    Telefono3 = cuentaPta.Telefono3,
                    TelefonoEmergencia = cuentaPta.TelefonoEmergencia,
                    SitioWeb = cuentaPta.SitioWeb,
                    Twitter = cuentaPta.Twitter,
                    Facebook = cuentaPta.Facebook,
                    Linkedin = cuentaPta.LinkedIn,
                    Instagram = cuentaPta.Instagram,
                    TipoPresenciaDigital = cuentaPta.TipoPresenciaDigital,
                    UrlPresenciaDigital = cuentaPta.UrlPresenciaDigital,
                    TipoCorreo = cuentaPta.TipoCorreo,
                    Correo = cuentaPta.Correo,
                    AsesorIA = cuentaPta.Asesor_IA,
                    AsesorDM = cuentaPta.Asesor_DM,
                    PuntoContacto = cuentaPta.PuntoContacto,
                    CondicionPagoIA = cuentaPta.CondicionPago_IA,
                    CondicionPagoDM = cuentaPta.CondicionPago_DM,
                    LimiteCredito = cuentaPta.LimiteCredito,
                    Comentario = cuentaPta.Comentario,
                    CategValor = cuentaPta.CategoriaValor,
                    CategPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                    CategPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                    Incentivo = cuentaPta.Incentivo,
                    EstadoActivacion = cuentaPta.EstadoActivacion,
                    Gds = cuentaPta.GDS,
                    Herramientas = cuentaPta.Herramientas,
                    FacturacionAnual = cuentaPta.FacturacionAnual,
                    ProyeccionFactAnual = cuentaPta.ProyeccionFacturacionAnual,
                    InicioRelacionComercial = cuentaPta.InicioRelacionComercial
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
