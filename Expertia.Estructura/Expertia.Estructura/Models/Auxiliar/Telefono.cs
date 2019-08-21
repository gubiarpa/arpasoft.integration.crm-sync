using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Telefono : IUnique
    {
        public string ID { get; set; }
        public TipoTelefono Tipo { get; set; }
        public string Numero { get; set; }
    }
}