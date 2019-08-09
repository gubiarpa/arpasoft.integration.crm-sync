namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Documento de Identidad
    /// </summary>
    public class Documento
    {
        /// <summary>
        /// Tipo de Documento. Ejm. DNI, RUC, CE, etc.
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Número de Documento. Ejm. 20212577001, 71458522
        /// </summary>
        public string Numero { get; set; }
    }
}