using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Contacto B2C
    /// </summary>
    public class ContactoB2C : Cuenta
    {
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
        /// Estado Civil
        /// </summary>
        public string EstadoCivil { get; set; }
        /// <summary>
        /// Género
        /// </summary>
        public string Genero { get; set; }
        /// <summary>
        /// Nacionalidad
        /// </summary>
        public string Nacionalidad { get; set; }
        /// <summary>
        /// Grado de Estudios
        /// </summary>
        public string GradoEstudios { get; set; }
        /// <summary>
        /// Profesión
        /// </summary>
        public string Profesion { get; set; }
        /// <summary>
        /// Preferencias Generales
        /// </summary>
        public string PreferenciasGenerales { get; set; }
        /// <summary>
        /// Consideraciones de Salud
        /// </summary>
        public string ConsideracionesSalud { get; set; }
        /// <summary>
        /// Tipo de viaje
        /// </summary>
        public string TipoViaje { get; set; }
        /// <summary>
        /// Categoría de viaje
        /// </summary>
        public string CategoriaViaje { get; set; }
        /// <summary>
        /// Tipo de Acompañante(antes "Compañía")
        /// </summary>
        public string TipoAcompañante { get; set; }
    }
}