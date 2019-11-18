using Expertia.Estructura.Models.Auxiliar;

namespace Expertia.Estructura.Models.Behavior
{
    public class SimpleNegocioDesc : SimpleDesc, IUnidadNegocio
    {
        public SimpleNegocioDesc(string descripcion = null)
        {
            Descripcion = descripcion;
        }

        public SimpleNegocioDesc(string descripcion = null, UnidadNegocio unidadNegocio = null)
        {
            Descripcion = descripcion;
            UnidadNegocio = unidadNegocio;
        }

        public UnidadNegocio UnidadNegocio { get; set; }
    }
}