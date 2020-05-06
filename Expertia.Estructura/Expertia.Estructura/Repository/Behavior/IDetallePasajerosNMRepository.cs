using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IDetallePasajerosNMRepository
    {
        Operation GetPasajeros();
        Operation Update(RptaPasajeroSF RptaPasajeroNM);
    }
}
