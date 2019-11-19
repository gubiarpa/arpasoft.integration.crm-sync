using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ISubcodigoRepository
    {
        Operation Create(Subcodigo subcodigo);
        Operation Read();
        Operation Update(Subcodigo subcodigo);
    }
}
