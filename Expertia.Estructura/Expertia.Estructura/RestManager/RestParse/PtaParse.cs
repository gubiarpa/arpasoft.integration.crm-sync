using Expertia.Estructura.Models;
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
                return new
                {
                    info = new
                    {
                        accion = cuentaPta.Accion,
                        dkCuenta = cuentaPta.DkCuenta.ToString(),
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
                        asesorIA = cuentaPta.Asesor_IA,
                        asesorDM = cuentaPta.Asesor_DM,
                        puntoContacto = cuentaPta.PuntoContacto,
                        condicionPagoIA = cuentaPta.CondicionPago_IA,
                        condicionPagoDM = cuentaPta.CondicionPago_DM,
                        limiteCredito = cuentaPta.LimiteCredito.ToString("0.00"),
                        comentario = cuentaPta.Comentario,
                        categValor = cuentaPta.CategoriaValor,
                        categPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                        categPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                        incentivo = cuentaPta.Incentivo,
                        estadoActivacion = cuentaPta.EstadoActivacion,
                        gds = cuentaPta.GDS,
                        herramientas = cuentaPta.Herramientas,
                        facturacionAnual = cuentaPta.FacturacionAnual,
                        proyeccionFactAnual = cuentaPta.ProyeccionFacturacionAnual,
                        inicioRelacionComercial = cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy")
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