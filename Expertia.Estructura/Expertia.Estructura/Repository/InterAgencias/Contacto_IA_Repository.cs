using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Contacto_IA_Repository : OracleBase<Contacto>, ICrud<Contacto>, ISameSPName<Contacto>
    {
        public Contacto_IA_Repository() : base(ConnectionKeys.IAConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.IA_Create_Contacto, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.IA_Update_Contacto, entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Contacto entity, string SPName, string userName)
        {
            return (new Contacto_DM_Repository(ConnectionKeys.IAConnKey)).ExecuteOperation(entity, SPName, userName);
        }

        public Operation Generate(Contacto entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Asociate(Contacto entity)
        {
            throw new System.NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}