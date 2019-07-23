using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Participante
    {
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
        public bool FlagPrincipal { get; set; }
    }
}