using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Sitio : IUnique
    {
        public string ID { get; set; }
        public TipoSitio Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}