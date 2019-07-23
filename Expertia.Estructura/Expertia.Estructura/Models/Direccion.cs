using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Dirección Registrada
    /// </summary>
    public class Direccion
    {
        /// <summary>
        /// Tipo de Dirección
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Descripción de la Dirección
        /// </summary>
        public string Descripcion { get; set; }
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
    }
}