using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IRegionCuentaRepository
    {
        Operation Register(RegionCuenta regionCuenta);
    }
}
