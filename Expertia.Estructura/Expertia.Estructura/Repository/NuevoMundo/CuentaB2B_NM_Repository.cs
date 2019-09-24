using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class CuentaB2B_NM_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_NM_Repository() : base(ConnectionKeys.NMConnKey)
        {
        }

        public Operation Create(CuentaB2B entity)
        {
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
    }
}