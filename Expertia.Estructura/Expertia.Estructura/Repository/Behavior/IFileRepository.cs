using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFileRepository
    {
        Operation GetNewAgenciaPnr();
        Operation GetFileAndBoleto(AgenciaPnr entity);
        Operation UpdateFile(File file);
        Operation UpdateBoleto(Boleto boleto);
    }
}
