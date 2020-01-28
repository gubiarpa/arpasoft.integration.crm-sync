using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.AppWebs
{
    public class ContactoPta_AW_Repository : OracleBase<ContactoPta>, IContactoPtaRepository
    {
        public ContactoPta_AW_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation GetContactos()
        {
            return new ContactoPta_IA_Repository(UnidadNegocioKeys.AppWebs).GetContactos();
        }

        public Operation Update(ContactoPta cuentaPta)
        {
            return new ContactoPta_IA_Repository(UnidadNegocioKeys.AppWebs).Update(cuentaPta);
        }
    }
}