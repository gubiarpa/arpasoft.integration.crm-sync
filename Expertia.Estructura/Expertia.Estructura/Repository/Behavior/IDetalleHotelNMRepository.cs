using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IDetalleHotelNMRepository
    {
        Operation GetDetalleHoteles();
        Operation Update(RptaHotelSF RptaHotelNM);
    }
}
