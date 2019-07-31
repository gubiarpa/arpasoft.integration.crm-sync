using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Contacto B2C
    /// </summary>
    public class CuentaB2C : Cuenta
    {
        #region Properties
        /// <summary>
        /// Nombre
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellido Paterno
        /// </summary>
        public string ApePaterno { get; set; }
        /// <summary>
        /// Apellido Materno
        /// </summary>
        public string ApeMaterno { get; set; }
        /// <summary>
        /// Preferencias Generales
        /// </summary>
        public string PreferenciasGenerales { get; set; }
        /// <summary>
        /// Consideraciones de Salud
        /// </summary>
        public string ConsideracionesSalud { get; set; }
        #endregion

        #region ForeignKey
        /// <summary>
        /// Estado Civil
        /// </summary>
        public EstadoCivil EstadoCivil { get; set; }
        /// <summary>
        /// Género
        /// </summary>
        public Genero Genero { get; set; }
        /// <summary>
        /// Nacionalidad
        /// </summary>
        public Nacionalidad Nacionalidad { get; set; }
        /// <summary>
        /// Grado de Estudios
        /// </summary>
        public GradoEstudios GradoEstudios { get; set; }
        /// <summary>
        /// Profesión
        /// </summary>
        public Profesion Profesion { get; set; }
        /// <summary>
        /// Tipo de viaje
        /// </summary>
        public TipoViaje TipoViaje { get; set; }
        /// <summary>
        /// Categoría de viaje
        /// </summary>
        public CategoriaViaje CategoriaViaje { get; set; }
        /// <summary>
        /// Tipo de Acompañante(antes "Compañía")
        /// </summary>
        public TipoAcompanante TipoAcompanante { get; set; }
        #endregion
    }
}