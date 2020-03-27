using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IOportunidadNMRepository
    {
        Operation GetOportunidades();
        Operation Update(OportunidadNM oportunidadNM);
    }


}
