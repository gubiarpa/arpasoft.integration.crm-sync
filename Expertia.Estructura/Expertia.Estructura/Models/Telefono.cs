using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Telefono : UniqueBase
    {
        public TipoTelefono Tipo { get; set; }
        public string Numero { get; set; }
    }
}