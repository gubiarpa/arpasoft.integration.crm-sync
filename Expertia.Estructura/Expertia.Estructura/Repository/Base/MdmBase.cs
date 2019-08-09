using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Base
{
    public abstract class MdmBase<T> : OracleBase<T>
    {
        public MdmBase() : base(ConnectionKeys.MDMConnKey)
        {
        }

        protected override IEnumerable<T> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}