using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;

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

        public Operation Delete(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.IA_Update_Contacto, entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Contacto entity, string SPName, string userName)
        {
            return (new DestinosMundiales.Contacto_DM_Repository()).ExecuteOperation(entity, SPName, userName);
        }
        #endregion

        #region NonImplemented
        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Contacto entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}