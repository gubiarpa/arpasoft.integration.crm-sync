using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    interface IOportunidadRepository
    {
        Operation GetOportunidades();
        Operation Update(Oportunidad oportunidad);
    }
}
