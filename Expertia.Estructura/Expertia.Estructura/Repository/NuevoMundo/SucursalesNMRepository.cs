using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class SucursalesNMRepository : OracleBase<object>, ISucursalesNMRepository
    {
        #region Constructor
        public SucursalesNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.NuevoMundo) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation _Select_SucursalesBy_Vendedor(string pStrIdVend)
        {
            var operation = new Operation();

            #region Parameters
            AddParameter("pVarIdVendedor_in", OracleDbType.Varchar2, pStrIdVend, ParameterDirection.Input, 3);
            AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_GetSucursalXVendedor);            
            operation[OutParameter.CursorDtosGenerico] = ToSucursalesNM(GetDtParameter("pCurResult_out"));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<SucursalNM> ToSucursalesNM(DataTable dt)
        {
            try
            {
                var sucursalesNMList = new List<SucursalNM>();

                foreach (DataRow row in dt.Rows)
                {
                    sucursalesNMList.Add(new SucursalNM()
                    {                        
                        IdSucursal = row.IntParse("ID_SUCURSAL"),                        
                        Descripcion = row.StringParse("DESCRIPCION")
                    });
                }
                return sucursalesNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}