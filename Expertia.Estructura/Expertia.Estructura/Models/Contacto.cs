using Expertia.Estructura.Models.Behavior;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Contacto : ISalesForce
    {
        public string IdSalesForce { get; set; }
        public string IdCuentaSalesForce { get; set; }
        public string Nombre { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; }
        public string Genero { get; set; }
        public string Nacionalidad { get; set; }
        public IEnumerable<Documento> Documentos { get; set; }
        public string LogoFoto { get; set; }
        public int Hijos { get; set; }
        public string Profesion { get; set; }
        public string CargoEmpresa { get; set; }
        public int TiempoEmpresa { get; set; }
        public IEnumerable<Direccion> Direcciones { get; set; }
        public IEnumerable<Telefono> Telefonos { get; set; }
        public IEnumerable<Sitio> Sitios { get; set; }
        public IEnumerable<Correo> Correos { get; set; }
        public IEnumerable<IdiomaComunicCliente> IdiomasComunicCliente { get; set; }
        public string NivelRiesgo { get; set; }
        public string RegiónMercadoBranch { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public string OrigenContacto { get; set; }
    }
}