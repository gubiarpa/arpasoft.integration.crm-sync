using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Controllers.Behavior
{
    public interface IMethodTryer<T>
    {
        void CreateOrUpdate(UnidadNegocioKeys? unidadNegocio, T entity);
        void UpdateOrCreate(UnidadNegocioKeys? unidadNegocio, T entity);
    }
}
