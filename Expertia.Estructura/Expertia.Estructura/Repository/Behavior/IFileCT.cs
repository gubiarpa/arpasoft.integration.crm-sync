using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFileCT
    {
        Operation GetFileCT(FileCTRequest fileRequest);
    }
}