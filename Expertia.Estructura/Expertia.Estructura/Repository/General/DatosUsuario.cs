using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace Expertia.Estructura.Repository.General
{
    public class DatosUsuario : OracleBase<VentasRequest>, IDatosUsuario
    {
        #region Constructor
        public DatosUsuario(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        public UsuarioLogin Get_Dts_Usuario_Personal(int UsuarioID)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, UsuarioID, ParameterDirection.Input);
                AddParameter(OutParameter.CursorDtosPersonal, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Datos_Usuario);
                UsuarioLogin _DtosUsuario = FillDatosUsuarioPersonal(GetDtParameter(OutParameter.CursorDtosPersonal));                
                #endregion

                return _DtosUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioLogin Get_Dts_Usuario_Personal_NM(int UsuarioID)
        {
            try
            {
                #region Parameter
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, UsuarioID, ParameterDirection.Input);
                AddParameter(OutParameter.CursorDtosPersonal, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Datos_Usuario_NM);
                UsuarioLogin _DtosUsuario = FillDatosUsuarioPersonal(GetDtParameter(OutParameter.CursorDtosPersonal));
                #endregion

                return _DtosUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private UsuarioLogin FillDatosUsuarioPersonal(DataTable dt = null)
        {
            try
            {
                UsuarioLogin DatosUsuario = null;
                if (dt != null)
                {
                    DatosUsuario = new UsuarioLogin();
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Loading                        
                        if (row["USUWEB_ID"] != null)
                            DatosUsuario.IdUsuario = Convert.ToInt32(row["USUWEB_ID"]);                        
                        if (row["PER_NOM"] != null)
                            DatosUsuario.NomUsuario = row["PER_NOM"].ToString();
                        if (row["PER_APEPAT"] != null)
                            DatosUsuario.ApePatUsuario = row["PER_APEPAT"].ToString();
                        if (row["PER_APEMAT"] != null)
                            DatosUsuario.ApeMatUsuario = row["PER_APEMAT"].ToString();
                        if (row["USUWEB_LOGIN"] != null)
                            DatosUsuario.LoginUsuario = row["USUWEB_LOGIN"].ToString();
                        if (row["PER_EMAIL"] != null)
                            DatosUsuario.EmailUsuario = row["PER_EMAIL"].ToString();                        
                        if (row["PER_ESTADO"] != null)
                            DatosUsuario.EstadoUsuario = row["PER_ESTADO"].ToString();
                        if (row["DEP_ID"] != null)
                            DatosUsuario.IdDep = Convert.ToInt32(row["DEP_ID"]);
                        if (row["OFI_ID"] != null)
                            DatosUsuario.IdOfi = Convert.ToInt32(row["OFI_ID"]);
                        if (row["EMP_ID"] != null) { }
                            DatosUsuario.IdEmp = Convert.ToInt32(row["EMP_ID"]);                                                                      
                        if (row["ES_COUNTER_ADMIN"] != null && string.IsNullOrEmpty(row["ES_COUNTER_ADMIN"].ToString()) == false)
                            DatosUsuario.EsCounterAdminSRV = (row["ES_COUNTER_ADMIN"].ToString() == "1" ? true : false);
                        if (row["PER_ES_SUPERVISOR"] != null && string.IsNullOrEmpty(row["PER_ES_SUPERVISOR"].ToString()) == false)
                            DatosUsuario.EsSupervisorSRV = (row["PER_ES_SUPERVISOR"].ToString() == "1" ? true : false);

                        DatosUsuario.TipoUsuario = TIPO_USUARIO.PERSONAL;
                        DatosUsuario.NomCompletoUsuario = (DatosUsuario.NomUsuario != null ? DatosUsuario.NomUsuario : "") + (DatosUsuario.ApePatUsuario != null ? DatosUsuario.ApePatUsuario : "") + (DatosUsuario.ApeMatUsuario != null ? DatosUsuario.ApeMatUsuario : "");
                        #endregion     
                        break;
                    }
                }
                return DatosUsuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}