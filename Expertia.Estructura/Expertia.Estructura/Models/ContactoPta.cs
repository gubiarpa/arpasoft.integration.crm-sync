using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class ContactoPta : ICrmApiResponse, IActualizado
    {
        public string UnidadNegocio { get; set; }
        public string Accion { get; set; }
        public string DkAgencia { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string EstadoCivil { get; set; }
        public string Cargo { get; set; }
        public string TipoContacto { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool TieneHijos { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Direccion { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string TipoPresenciaDigital { get; set; }
        public string UrlPresenciaDigital { get; set; }
        public string TipoTelefono1 { get; set; }
        public string Telefono1 { get; set; }
        public string TipoTelefono2 { get; set; }
        public string Telefono2 { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string TipoCorreo { get; set; }
        public string Correo { get; set; }
        public string EstadoContacto { get; set; }
        public string EsContactoMarketing { get; set; }
        public int Correlativo { get; set; }
        public int UsuarioWebId { get; set; }
        public string IdCuentaCrm { get; set; }
        public string IdContactoCrm { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}