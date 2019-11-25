using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICuentaPtaRepository
    {
        Operation Read();
        Operation Update(CuentaPta cuentaPta);
    }
}
