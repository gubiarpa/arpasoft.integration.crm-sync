using Expertia.Estructura.Models.Auxiliar;

namespace Expertia.Estructura.Models.Behavior
{
    public class SimpleNegocioDesc : SimpleDesc, IUnidadNegocio
    {
        public UnidadNegocio UnidadNegocio { get; set; }
    }
}