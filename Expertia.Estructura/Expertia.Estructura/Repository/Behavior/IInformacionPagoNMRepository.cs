using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IInformacionPagoNMRepository
    {
        Operation GetInformacionPago(UnidadNegocioKeys? unidadNegocio);
    }
}
