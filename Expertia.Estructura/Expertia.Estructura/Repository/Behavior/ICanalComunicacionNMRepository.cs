using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICanalComunicacionNMRepository
    {
        Operation Send(UnidadNegocioKeys? unidadNegocio);
    }
}
