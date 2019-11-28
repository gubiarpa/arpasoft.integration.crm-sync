using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class CuentaPta : ICrmApiResponse, IActualizado
    {
        public string UnidadNegocio { get; set; }
        public string IdCuentaCrm { get; set; }
        public string Accion { get; set; }
        public int DkCuenta { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string TipoCuenta { get; set; }
        public string Propietario { get; set; }
        public DateTime FechaAniversario { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string TipoDireccion { get; set; }
        public string DireccionResidencia { get; set; }
        public string PaisResidencia { get; set; }
        public string DepartamentoResidencia { get; set; }
        public string CiudadResidencia { get; set; }
        public string DistritoResidencia { get; set; }
        public string DireccionFiscal { get; set; }
        public string TipoTelefono1 { get; set; }
        public string Telefono1 { get; set; }
        public string TipoTelefono2 { get; set; }
        public string Telefono2 { get; set; }
        public string TipoTelefono3 { get; set; }
        public string Telefono3 { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string SitioWeb { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string TipoPresenciaDigital { get; set; }
        public string UrlPresenciaDigital { get; set; }
        public string TipoCorreo { get; set; }
        public string Correo { get; set; }
        public string Asesor_IA { get; set; }
        public string Asesor_DM { get; set; }
        public string PuntoContacto { get; set; }
        public string CondicionPago_IA { get; set; }
        public string CondicionPago_DM { get; set; }
        public float LimiteCredito { get; set; }
        public string Comentario { get; set; }
        public string CategoriaValor { get; set; }
        public string CategoriaPerfilActitudTecnologica { get; set; }
        public string CategoriaPerfilFidelidad { get; set; }
        public string Incentivo { get; set; }
        public string EstadoActivacion { get; set; }
        public string GDS { get; set; }
        public string Herramientas { get; set; }
        public float FacturacionAnual { get; set; }
        public float ProyeccionFacturacionAnual { get; set; }
        public DateTime InicioRelacionComercial { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}