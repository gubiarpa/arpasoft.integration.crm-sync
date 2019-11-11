using Expertia.Estructura.Models.Auxiliar;

namespace Expertia.Estructura.Models.Behavior
{
    public class SimpleNegocioDesc : SimpleDesc, IUnidadNegocio
    {
        public SimpleNegocioDesc(string descripcion = null)
        {
            Descripcion = descripcion;
        }

        public UnidadNegocio UnidadNegocio { get; set; }
    }
}