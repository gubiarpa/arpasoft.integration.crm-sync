using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ISolicitudPagoNMRepository
    {
        Operation GetSolicitudesPago();
        Operation Update(RptaSolicitudPagoSF RptaSolicitudPagoNM);
    }
}
