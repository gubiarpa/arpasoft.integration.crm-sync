using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IChatterNMRepository
    {
        Operation GetPostCotizaciones();
        Operation Update(RptaChatterSF RptaChatterNM);
    }
}
