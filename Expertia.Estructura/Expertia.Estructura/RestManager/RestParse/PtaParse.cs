using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.RestManager.RestParse
{
    public static class PtaParse
    {
        public static object ToSalesforceEntity(this CuentaPta cuentaPta)
        {
            try
            {
                var _unidadNegocio = cuentaPta.UnidadNegocio.ToUnidadNegocio();
                return new
                {
                    info = new
                    {
                        accion = cuentaPta.Accion,
                        dkCuenta_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.DkCuenta.ToString() : null,
                        dkCuenta_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.DkCuenta.ToString() : null,
                        razonSocial = cuentaPta.RazonSocial,
                        nombreComercial = cuentaPta.NombreComercial,
                        tipoCuenta = cuentaPta.TipoCuenta,
                        propietario = cuentaPta.Propietario,
                        fechaAniversario = cuentaPta.FechaAniversario.ToString("dd/MM/yyyy"),
                        tipoDocumentoIdentidad = cuentaPta.TipoDocumentoIdentidad,
                        documentoIdentidad = cuentaPta.DocumentoIdentidad,
                        tipoDireccion = cuentaPta.TipoDireccion,
                        direccionResidencia = cuentaPta.DireccionResidencia,
                        paisResidencia = cuentaPta.PaisResidencia,
                        departamentoResidencia = cuentaPta.DepartamentoResidencia,
                        ciudadResidencia = cuentaPta.CiudadResidencia,
                        distritoResidencia = cuentaPta.DistritoResidencia,
                        direccionFiscal = cuentaPta.DireccionFiscal,
                        tipoTelefono1 = cuentaPta.TipoTelefono1,
                        telefono1 = cuentaPta.Telefono1,
                        tipoTelefono2 = cuentaPta.TipoTelefono2,
                        telefono2 = cuentaPta.Telefono2,
                        tipoTelefono3 = cuentaPta.TipoTelefono3,
                        telefono3 = cuentaPta.Telefono3,
                        telefonoEmergencia = cuentaPta.TelefonoEmergencia,
                        sitioWeb = cuentaPta.SitioWeb,
                        twitter = cuentaPta.Twitter,
                        facebook = cuentaPta.Facebook,
                        linkedin = cuentaPta.LinkedIn,
                        instagram = cuentaPta.Instagram,
                        tipoPresenciaDigital = cuentaPta.TipoPresenciaDigital,
                        urlPresenciaDigital = cuentaPta.UrlPresenciaDigital,
                        tipoCorreo = cuentaPta.TipoCorreo,
                        correo = cuentaPta.Correo,
                        asesor_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.Asesor : null,
                        asesor_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.Asesor : null,
                        puntoContacto = cuentaPta.PuntoContacto,
                        condicionPago_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.CondicionPago : null,
                        condicionPago_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.CondicionPago : null,
                        limiteCredito_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ?  cuentaPta.LimiteCredito.ToString("0.00") : null,
                        limiteCredito_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ?  cuentaPta.LimiteCredito.ToString("0.00") : null,
                        comentario = cuentaPta.Comentario,
                        categValor = cuentaPta.CategoriaValor,
                        categPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                        categPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                        incentivo = cuentaPta.Incentivo,
                        estadoActivacion = cuentaPta.EstadoActivacion,
                        gds = cuentaPta.GDS,
                        herramientas = cuentaPta.Herramientas,
                        facturacionAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ?  cuentaPta.FacturacionAnual : null,
                        facturacionAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.FacturacionAnual : null,
                        proyeccionFactAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        proyeccionFactAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        inicioRelacionComercial_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ?  cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null,
                        inicioRelacionComercial_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ?  cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this ContactoPta contactoPta)
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

        public static object ToSalesforceEntity(this Subcodigo subcodigo)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        UnidadNegocio = subcodigo.UnidadNegocio,
                        Accion = subcodigo.Accion,
                        DkAgencia = subcodigo.DkAgencia.ToString(),
                        CorrelativoSubcodigo = subcodigo.CorrelativoSubcodigo.ToString(),
                        DireccionSucursal = subcodigo.DireccionSucursal,
                        EstadoSucursal = subcodigo.EstadoSucursal,
                        NombreSucursal = subcodigo.NombreSucursal,
                        Promotor = subcodigo.Promotor,
                        CondicionPago = subcodigo.CondicionPago
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}