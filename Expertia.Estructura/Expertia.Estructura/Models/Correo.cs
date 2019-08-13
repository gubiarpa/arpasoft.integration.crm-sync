using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Correo : UniqueBase
    {
        public TipoCorreo Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}