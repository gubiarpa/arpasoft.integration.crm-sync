using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Sitio : UniqueBase
    {
        public TipoSitio Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}