using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Condor
{
    public class CuentaB2B_RbRepository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_RbRepository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        public OperationResult Create(CuentaB2B entity)
        {
            AddInParameter("PX_ID_CUENTA", entity.IdSalesForce);
            throw new NotImplementedException();
        }

        public OperationResult Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CuentaB2B> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}