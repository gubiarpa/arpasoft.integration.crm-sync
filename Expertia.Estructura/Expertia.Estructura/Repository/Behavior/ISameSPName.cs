using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ISameSPName<T>
    {
        Operation ExecuteOperation(T entity, string SPName, string userName);
    }
}
