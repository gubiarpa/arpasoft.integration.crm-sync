using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Sitio : IUnique
    {
        public string ID { get; set; }
        public TipoSitio Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}