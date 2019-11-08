using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICrud<T>
    {
        // Cuenta, Contacto
        Operation Create(T entity);
        Operation Read(T entity);
        Operation Update(T entity);
        // Cotizacion
        Operation Generate(T entity);
        Operation Asociate(T entity);
    }
}
