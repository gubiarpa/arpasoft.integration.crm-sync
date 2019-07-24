using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Contacto
    /// </summary>
    public class Contacto : ISalesForce
    {
        /// <summary>
        /// IDSalesForce
        /// </summary>
        public string IdSalesForce { get; set; }
        /// <summary>
        /// IDSalesForce de la Cuenta asociada
        /// </summary>
        public string IdCuentaSalesForce { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellido paterno
        /// </summary>
        public string ApePaterno { get; set; }
        /// <summary>
        /// Apellido materno
        /// </summary>
        public string ApeMaterno { get; set; }
        /// <summary>
        /// Fecha de Nacimiento
        /// </summary>
        public string FechaNacimiento { get; set; }
        /// <summary>
        /// Estado Civil
        /// </summary>
        public string EstadoCivil { get; set; }
        /// <summary>
        /// Genero
        /// </summary>
        public string Genero { get; set; }
        /// <summary>
        /// Nacionalidad
        /// </summary>
        public string Nacionalidad { get; set; }
        /// <summary>
        /// Lista de Documentos
        /// </summary>
        public IEnumerable<Documento> Documentos { get; set; }
        /// <summary>
        /// Logo/Foto
        /// </summary>
        public string LogoFoto { get; set; }
        /// <summary>
        /// Hijos
        /// </summary>
        public int Hijos { get; set; }
        /// <summary>
        /// Profesión
        /// </summary>
        public string Profesion { get; set; }
        /// <summary>
        /// Cargo En La Empresa
        /// </summary>
        public string CargoEmpresa { get; set; }
        /// <summary>
        /// Tiempo en La Empresa (Años)
        /// </summary>
        public int TiempoEmpresa { get; set; }
        /// <summary>
        /// Lista de Direcciones
        /// </summary>
        public IEnumerable<Direccion> Direcciones { get; set; }
        /// <summary>
        /// Lista de Teléfonos
        /// </summary>
        public IEnumerable<Telefono> Telefonos { get; set; }
        /// <summary>
        /// Lista de Sitios
        /// </summary>
        public IEnumerable<Sitio> Sitios { get; set; }
        /// <summary>
        /// Lista de Correos
        /// </summary>
        public IEnumerable<Correo> Correos { get; set; }
        /// <summary>
        /// Idiomas de Comunicación del Cliente
        /// </summary>
        public List<IdiomaComunicCliente> IdiomasComunicCliente { get; set; }
        /// <summary>
        /// Nivel de Riesgo
        /// </summary>
        public string NivelRiesgo { get; set; }
        /// <summary>
        /// Región/mercado-Branch
        /// </summary>
        public string RegiónMercadoBranch { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Comentarios
        /// </summary>
        public string Comentarios { get; set; }
        /// <summary>
        /// Origen de Contacto
        /// </summary>
        public string OrigenContacto { get; set; }
    }
}