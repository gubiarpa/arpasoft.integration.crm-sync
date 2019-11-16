using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFileRepository
    {
        Operation GetNewAgenciaPnr();
        Operation GetNewFile(AgenciaPnr entity);
    }
}
