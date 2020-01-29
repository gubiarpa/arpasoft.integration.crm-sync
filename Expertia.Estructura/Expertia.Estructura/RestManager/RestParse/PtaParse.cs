using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;

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
                        Accion = cuentaPta.Accion,
                        DkCuenta_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.DkCuenta.ToString() : null,
                        DkCuenta_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.DkCuenta.ToString() : null,
                        RazonSocial = cuentaPta.RazonSocial,
                        NombreComercial = cuentaPta.NombreComercial,
                        TipoCuenta = cuentaPta.TipoCuenta,
                        Propietario = cuentaPta.Propietario,
                        FechaAniversario = cuentaPta.FechaAniversario.ToString("dd/MM/yyyy"),
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
                        Asesor_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.Asesor : null,
                        Asesor_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.Asesor : null,
                        PuntoContacto = cuentaPta.PuntoContacto,
                        CondicionPago_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.CondicionPago : null,
                        CondicionPago_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.CondicionPago : null,
                        LimiteCredito_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.LimiteCredito.ToString("0.00") : null,
                        LimiteCredito_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.LimiteCredito.ToString("0.00") : null,
                        Comentario = cuentaPta.Comentario,
                        CategValor = cuentaPta.CategoriaValor,
                        CategPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                        CategPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                        Incentivo = cuentaPta.Incentivo,
                        EstadoActivacion = cuentaPta.EstadoActivacion,
                        Gds = cuentaPta.GDS,
                        Herramientas = cuentaPta.Herramientas,
                        FacturacionAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.FacturacionAnual : null,
                        FacturacionAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.FacturacionAnual : null,
                        ProyeccionFactAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        ProyeccionFactAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        InicioRelacionComercial_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null,
                        InicioRelacionComercial_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null
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
                    datos = new
                    {
                        accion = contactoPta.Accion,
                        dkCuenta_DM = contactoPta.UnidadNegocio.ToUnidadNegocio().Equals(UnidadNegocioKeys.DestinosMundiales) ? contactoPta.DkAgencia : null,
                        dkCuenta_IA = contactoPta.UnidadNegocio.ToUnidadNegocio().Equals(UnidadNegocioKeys.Interagencias) ? contactoPta.DkAgencia : null,
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
                        Accion = subcodigo.Accion,
                        DkCuenta_DM = subcodigo.DkAgencia_DM,
                        DkCuenta_IA = subcodigo.DkAgencia_IA,
                        CorrelativoSubcodigo_DM = subcodigo.CorrelativoSubcodigo_DM,
                        CorrelativoSubcodigo_IA = subcodigo.CorrelativoSubcodigo_IA,
                        DireccionSucursal = subcodigo.DireccionSucursal,
                        EstadoSucursal = subcodigo.EstadoSucursal,
                        NombreSucursal = subcodigo.NombreSucursal,
                        Promotor_DM = subcodigo.Promotor_DM,
                        Promotor_IA = subcodigo.Promotor_IA,
                        CondicionPago_DM = subcodigo.CondicionPago_DM,
                        CondicionPago_IA = subcodigo.CondicionPago_IA
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