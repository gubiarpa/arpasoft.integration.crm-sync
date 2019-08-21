using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Documento : IUnique
    {
        public string ID { get; set; }
        public TipoDocumento Tipo { get; set; }
        public string Numero { get; set; }
    }
}