using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Correo : IUnique
    {
        public string ID { get; set; }
        public TipoCorreo Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}