using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Documento : UniqueBase
    {
        public TipoDocumento Tipo { get; set; }
        public string Numero { get; set; }
    }
}