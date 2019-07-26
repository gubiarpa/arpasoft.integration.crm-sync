using Expertia.Estructura.Repository.MDM;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ILookupId
    {
        object LookUpByDescription(CuentaB2B_FK fieldName, object description = null);
    }
}
