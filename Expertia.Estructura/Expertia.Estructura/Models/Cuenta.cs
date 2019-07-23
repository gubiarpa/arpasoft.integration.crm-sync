using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Cuenta (B2B, B2C)
    /// </summary>
    public class Cuenta : ISalesForce
    {
        /// <summary>
        /// ID Proveniente de SalesForce
        /// </summary>
        public string IdSalesForce { get; set; }
        /// <summary>
        /// Tipo Persona: Natura o Jurídica
        /// </summary>
        public string TipoPersona { get; set; }
        /// <summary>
        /// Fecha de Nacimiento / Aniversario
        /// </summary>
        public DateTime? FechaNacimOrAniv { get; set; }
        /// <summary>
        /// Logo / Foto
        /// </summary>
        public string LogoFoto { get; set; }
        /// <summary>
        /// Lista de Documentos
        /// </summary>
        public IEnumerable<Documento> Documentos { get; set; }        
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
        /// Lista de Participantes
        /// </summary>
        public IEnumerable<Participante> Participantes { get; set; }       
        /// <summary>
        /// Intereses en Productos o Actividad
        /// </summary>
        public IEnumerable<InteresProdActiv> InteresesProdActiv { get; set; }
        /// <summary>
        /// Origen de la Cuenta
        /// </summary>
        public string PuntoContacto { get; set; }
        /// <summary>
        /// ¿Recibir Información?
        /// </summary>
        public bool RecibirInformacion { get; set; }
        /// <summary>
        /// Canal por recibir la información
        /// </summary>
        public IEnumerable<CanalInformacion> CanalesRecibirInfo { get; set; }
        /// <summary>
        /// Región / Mercado - Branch
        /// </summary>
        public IEnumerable<Branch> Branches { get; set; }
        /// <summary>
        /// Idiomas de Comunicación del Cliente
        /// </summary>
        public List<IdiomaComunicCliente> IdiomasComunicCliente { get; set; }
        /// <summary>
        /// Nivel de Importancia
        /// </summary>
        public string NivelImportancia { get; set; } // ♫ Es un número
        /// <summary>
        /// Fecha de Inicio de Relación Comercial
        /// </summary>
        public DateTime? FechaIniRelacionComercial { get; set; }
        /// <summary>
        /// Comentarios
        /// </summary>
        public string Comentarios { get; set; }
        /// <summary>
        /// Tipo cuenta
        /// </summary>
        public string TipoCuenta { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// País de Procedencia
        /// </summary>
        public string PaisProcedencia { get; set; }
    }
}