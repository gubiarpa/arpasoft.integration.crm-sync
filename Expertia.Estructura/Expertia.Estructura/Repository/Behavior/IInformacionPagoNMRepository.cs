using Expertia.Estructura.Models;
using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IInformacionPagoNMRepository
    {
        Operation GetInformacionPago(UnidadNegocioKeys? unidadNegocio);
        Operation GetListPagosServicio(int pIntCodWeb_in, int pIntCodCot_in, int pIntCodSuc_in, string tpCharTipoPaquete);
        Operation Update(RptaInformacionPagoSF oRptaInformacionPagoSF);
    }
}
