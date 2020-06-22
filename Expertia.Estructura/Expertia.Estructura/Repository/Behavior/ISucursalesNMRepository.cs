using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ISucursalesNMRepository
    {
        Operation _Select_SucursalesBy_Vendedor(string pStrIdVend);
    }
}