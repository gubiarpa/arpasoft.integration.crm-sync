using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.AppWebs
{
    public class Contacto_AW_Repository : OracleBase<Contacto>, ICrud<Contacto>, ISameSPName<Contacto>
    {
        public Contacto_AW_Repository() : base(ConnectionKeys.AWConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.AW_Create_Contacto, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.AW_Update_Contacto, entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Contacto entity, string SPName, string userName)
        {
            return (new Contacto_DM_Repository(ConnectionKeys.AWConnKey)).ExecuteOperation(entity, SPName, userName);
        }

        public Operation Generate(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Asociate(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}