using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.MDM
{
    public class CuentaB2B_MDMRepo : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_MDMRepo() : base(ConnectionKeys.MDMConnKey)
        {
        }

        public OperationResult Create(CuentaB2B entity)
        {

            return new OperationResult();
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