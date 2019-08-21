using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Correo : IUnique
    {
        public string ID { get; set; }
        public TipoCorreo Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}