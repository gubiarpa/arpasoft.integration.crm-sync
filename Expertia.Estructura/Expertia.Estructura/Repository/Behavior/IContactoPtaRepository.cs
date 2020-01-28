using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IContactoPtaRepository
    {
        Operation GetContactos();
        Operation Update(ContactoPta cuentaPta);
    }
}
