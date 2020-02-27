using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oficina = Expertia.Estructura.Models.Retail.Oficina;

namespace Expertia.Estructura.Repository.General
{
    public class DatosOficina : OracleBase<Models.Retail.Oficina>, IDatosOficina
    {
        #region Constructor
        public DatosOficina(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion
        public Models.Retail.Oficina ObtieneOficinaXId(int pIntIdOfi)
        {
            Oficina objOficina = null;
            try
            {
                #region Parameter
                AddParameter("pNumIdPedido_in", OracleDbType.Int64, pIntIdOfi, ParameterDirection.Input);
                AddParameter(OutParameter.CursorDtosOficina, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion
                #region Invoke   
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Datos_Oficina);
                objOficina = new Oficina();
                objOficina = FillRptaOficina(GetDtParameter(OutParameter.CursorDtosOficina));
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objOficina;
        }

        private Oficina FillRptaOficina(DataTable dt = null)
        {
            try
            {
                Oficina oficina = new Oficina();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Loading
                        oficina.intIdOfi = System.Convert.ToInt32(row["OFI_ID"]);
                        if (row["OFI_NOM"] != null)
                            oficina.strNomOfi = row["OFI_NOM"].ToString();
                        if (row["OFI_DIREC"] != null)
                            oficina.strDirecOfi = row["OFI_DIREC"].ToString();
                        if (row["OFI_ESTADO"] != null)
                            oficina.intEstadoOfi = System.Convert.ToInt32(row["OFI_ESTADO"]);
                        if (row["OFI_COD_POS"] != null)
                            oficina.strCodPostalOfi = row["OFI_COD_POS"].ToString();
                        if (row["OFI_TELF"] != null)
                            oficina.strTelfOfi = row["OFI_TELF"].ToString();
                        if (row["OFI_FAX"] != null)
                            oficina.strFaxOfi = row["OFI_FAX"].ToString();
                        #endregion     
                        break;
                    }
                }
                return oficina;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}