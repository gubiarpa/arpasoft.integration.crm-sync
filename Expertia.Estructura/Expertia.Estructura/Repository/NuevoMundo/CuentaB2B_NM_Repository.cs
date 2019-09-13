using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class CuentaB2B_NM_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_NM_Repository() : base(ConnectionKeys.NuevoMundoConnKey)
        {
        }

        public Operation Create(CuentaB2B entity)
        {
            Operation operation = new Operation();
            object value;

            try
            {
                #region Parameters
                #endregion

                #region Invoke
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new NotImplementedException();
        }

        public Operation Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CuentaB2B> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}