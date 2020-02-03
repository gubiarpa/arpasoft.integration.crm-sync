using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICotizacion_CT
    {
        Operation GetCotizaciones(CotizacionRequest cotizacionRequest);
    }

    public interface ICotizacion_DM
    {
        Operation GetCotizaciones();
        Operation UpdateCotizacion(Cotizacion_DM cotizacion);
    }
}