using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Cuenta
    {
        public string FechaNacimiento { get; set; } // Fecha de Nacimiento/Aniversario
        public string LogoFoto { get; set; } // Logo/Foto
        public string TipoDocumento { get; set; } // Tipo de Documento
        public string NumeroDocumento { get; set; } // Número de Documento
        public string TipoDirección { get; set; } // Tipo de dirección
        public string Direccion { get; set; } // Dirección
        public string Pais { get; set; } // País
        public string Departamento { get; set; } // Departamento
        public string Ciudad { get; set; } // Ciudad
        public string Distrito { get; set; } // Distrito
        public string TipoTeléfono { get; set; } // Tipo de teléfono
        public string NumeroTelefono { get; set; } // Número Telefono
        public string TipoSitio { get; set; } // Tipo de sitio
        public string Sitio { get; set; } // Sitio
        public string TipoCorreo { get; set; } // Tipo de correo
        public string Correo { get; set; } // Correo
        public string EmplEjecResponsable { get; set; } // Empleado Responsable /Ejecutivo responsable
        public string SupervisorKam { get; set; } // Supervisor / Kam
        public string Gerente { get; set; } // Gerente
        public string UnidadNegocio { get; set; } // Unidad de Negocio
        public string GrupoColabEjecRegionBranch { get; set; } // Grupo de Colaboración / Grupo de ejecutivos dedicados a una región - branch
        public string FlagPrincipal { get; set; } // Flag Principal
        public string InteresesProductosActividad { get; set; } // Intereses de productos / actividad
        public string TipoArea { get; set; } // Tipo de Área
        public string OrigenCuenta { get; set; } // Origen de la Cuenta
        public string RecibirInformacion { get; set; } // ¿Recibir Información?
        public string CanalRecibirInfo { get; set; } // Canal por recibir la información
        public string RegionMercadoBranch { get; set; } // Región/mercado-Branch
        public string IdiomaComunicCliente { get; set; } // Idioma de comunicación con el cliente
        public string NivelImportancia { get; set; } // Nivel de Importancia
        public string FechaIniRelacionComercial { get; set; } // Fecha de Inicio de Relación Comercial
        public string Comentarios { get; set; } // Comentarios
        public string TipoCuenta { get; set; } // Tipo cuenta
        public string TipoPersona { get; set; } // Tipo persona
        public string Estado { get; set; } // Estado
        public string PresupEstimadoVenta { get; set; } // Presupuesto estimado de venta
        public bool EsPotencial { get; set; } // Es Potencial
        public bool EsVIP { get; set; } // Es VIP
    }
}