using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Telefono : IUnique
    {
        public string ID { get; set; }
        public TipoTelefono Tipo { get; set; }
        public string Numero { get; set; }
    }
}