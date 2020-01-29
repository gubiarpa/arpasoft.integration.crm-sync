using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICotizacion_CT
    {
        Operation GetCotizacionCT(CotizacionRequest cotizacionRequest);
    }

    public interface ICotizacion_DM
    {
        Operation GetCotizacionesDM(CotizacionDM cotizacionDM);
    }
}