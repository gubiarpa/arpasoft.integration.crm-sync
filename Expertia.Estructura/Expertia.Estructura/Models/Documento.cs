using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Documento : IUnique
    {
        public string ID { get; set; }
        public TipoDocumento Tipo { get; set; }
        public string Numero { get; set; }
    }
}