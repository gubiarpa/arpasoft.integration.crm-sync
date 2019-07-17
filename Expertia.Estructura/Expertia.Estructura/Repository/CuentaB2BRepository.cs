using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository
{
    public class CuentaB2BRepository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public OperationResult Create(CuentaB2B entity)
        {
            /*
             * P_TIPO_DOCUMENTO VARCHAR2,
    P_DOCUMENTO VARCHAR2,
    P_NOMBRE VARCHAR2,
    P_AP_PATERNO VARCHAR2,
    P_AP_MATERNO VARCHAR2,
    P_GENERO VARCHAR2,
    P_ESTADO_CIVIL VARCHAR2,
    P_DIRECCION VARCHAR2
             */


            AddInParameter("IDSalesForce", entity.IdSalesForce);
            ExecuteSPWithoutResults("CONDOR.USP_CREAR_CLIENTE");

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