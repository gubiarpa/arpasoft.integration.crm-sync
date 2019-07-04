using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactoB2C : Cuenta
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nombre { get; set; } // Nombre
        /// <summary>
        /// 
        /// </summary>
        public string ApePaterno { get; set; } // Apellido paterno
        /// <summary>
        /// 
        /// </summary>
        public string ApeMaterno { get; set; } // Apellido materno
        /// <summary>
        /// 
        /// </summary>
        public string EstadoCivil { get; set; } // Estado Civil
        /// <summary>
        /// 
        /// </summary>
        public string Genero { get; set; } // Genero
        /// <summary>
        /// 
        /// </summary>
        public string Nacionalidad { get; set; } // Nacionalidad
        /// <summary>
        /// 
        /// </summary>
        public string GradoEstudios { get; set; } // Grado de estudios
        /// <summary>
        /// 
        /// </summary>
        public string Profesion { get; set; } // Profesión
        /// <summary>
        /// 
        /// </summary>
        public string PreferenciasGenerales { get; set; } // Preferencias generales
        /// <summary>
        /// 
        /// </summary>
        public string ConsideracionesSalud { get; set; } // Consideraciones de salud
        /// <summary>
        /// 
        /// </summary>
        public string TipoViaje { get; set; } // Tipo de viaje
        /// <summary>
        /// 
        /// </summary>
        public string CategoriaViaje { get; set; } // Categoría de viaje
        /// <summary>
        /// 
        /// </summary>
        public string TipoAcompañante { get; set; } // Tipo de Acompañante(antes "Compañía")
    }
}