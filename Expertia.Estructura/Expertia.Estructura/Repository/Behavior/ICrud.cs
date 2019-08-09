using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICrud<T>
    {
        Operation Create(T entity);
        Operation Read(T entity);
        Operation Update(T entity);
        Operation Delete(T entity);
    }
}
