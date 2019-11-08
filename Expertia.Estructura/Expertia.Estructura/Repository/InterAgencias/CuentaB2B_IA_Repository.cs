using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class CuentaB2B_IA_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>, ISameSPName<CuentaB2B>
    {
        public CuentaB2B_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(ConnectionKeys.IAConnKey, unidadNegocio)
        {
        }

        #region PublicMethods
        public Operation Create(CuentaB2B entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.IA_Create_CuentaB2B, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(CuentaB2B entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.IA_Update_CuentaB2B, entity.Auditoria.CreateUser.Descripcion);
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(CuentaB2B entity, string SPName, string userName)
        {
            return (new DestinosMundiales.CuentaB2B_DM_Repository(UnidadNegocioKeys.InterAgencias)).ExecuteOperation(entity, SPName, userName);
        }

        public Operation Generate(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Asociate(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}