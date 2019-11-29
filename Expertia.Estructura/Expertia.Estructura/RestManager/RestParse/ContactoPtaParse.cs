using Expertia.Estructura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.RestManager.RestParse
{
    public static class ContactoPtaParse //: ISalesforceParse
    {
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
    }
}