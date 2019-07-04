using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Cuenta (B2B, B2C)
    /// </summary>
    public class Cuenta
    {
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
        /// País
        /// </summary>
        public string Pais { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string Departamento { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        public string Ciudad { get; set; }
        /// <summary>
        /// Distrito
        /// </summary>
        public string Distrito { get; set; }
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
        /// Empleado Responsable / Ejecutivo Responsable
        /// </summary>
        public string EmpleadoOrEjecutivoResponsable { get; set; }
        /// <summary>
        /// Supervisor / Kam
        /// </summary>
        public string SupervisorKam { get; set; }
        /// <summary>
        /// Gerente
        /// </summary>
        public string Gerente { get; set; }
        /// <summary>
        /// Unidad de Negocio
        /// </summary>
        public string UnidadNegocio { get; set; }
        /// <summary>
        /// Grupo de Colaboración / Grupo de ejecutivos dedicados a una región - Branch
        /// </summary>
        public string GrupoColabEjecRegionBranch { get; set; }
        /// <summary>
        /// Flag Principal
        /// </summary>
        public string FlagPrincipal { get; set; }
        /// <summary>
        /// Intereses en Productos o Actividad
        /// </summary>
        public IEnumerable<InteresProdActiv> InteresesProdActiv { get; set; }
        /// <summary>
        /// Tipo de Área
        /// </summary>
        public string TipoArea { get; set; }
        /// <summary>
        /// Origen de la Cuenta
        /// </summary>
        public string OrigenCuenta { get; set; }
        /// <summary>
        /// ¿Recibir Información?
        /// </summary>
        public bool RecibirInformacion { get; set; }
        /// <summary>
        /// Canal por recibir la información
        /// </summary>
        public string CanalRecibirInfo { get; set; }
        /// <summary>
        /// Región / Mercado - Branch
        /// </summary>
        public string RegionMercadoBranch { get; set; }
        /// <summary>
        /// Idiomas de Comunicación del Cliente
        /// </summary>
        public IEnumerable<IdiomaComunicCliente> IdiomasComunicCliente { get; set; }
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
        /// Presupuesto Estimado de Venta
        /// </summary>
        public string PresupEstimadoVenta { get; set; }
        /// <summary>
        /// ¿Es Potencial?
        /// </summary>
        public bool EsPotencial { get; set; }
        /// <summary>
        /// ¿Es VIP?
        /// </summary>
        public bool EsVIP { get; set; }
    }
}