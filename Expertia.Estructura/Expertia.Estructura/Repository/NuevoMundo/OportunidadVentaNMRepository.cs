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

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class OportunidadVentaNMRepository : OracleBase, IOportunidadVentaNMRepository
    {
        #region Constructor
        public OportunidadVentaNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region 
        public Operation SelectByCodeSF(string strCodeSF)
        {
            try
            {
                var operation = new Operation();

                AddParameter("pIdCuentaSF_in", OracleDbType.Varchar2, strCodeSF);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure(StoredProcedureName.AW_Get_DatosClienteXIdSF);
                List<ClienteCot> ListClients = (List<ClienteCot>)ToClienteCot(GetDtParameter("pCurResult_out"));
                operation["pCurResult_out"] = (ListClients != null && ListClients.Count > 0 ? ListClients[0] : null);
                                
                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Update(int pIntIdCliCot, string pStrNomCliCot, string pStrApeCliCot, string pStrApeMatCliCot, string pStrEmailCliCot, string pStrEmailAlterCliCot, List<TelefonoCliCot> pLstTelefonos, bool pBolRecibePromo, string pStrDirecCliCot, string pStrNumDocCliCot, string pStrIdTipDoc, int pIntIdUsuWeb, int pIntIdWeb, List<ArchivoCliCot> pLstArchivos, Nullable<DateTime> pDatFecNac)
        {             
            try
            {
                #region Parameter
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot, ParameterDirection.Input);
                AddParameter("pVarNomCliCot_in", OracleDbType.Varchar2, pStrNomCliCot, ParameterDirection.Input, 50);
                AddParameter("pVarApeCliCot_in", OracleDbType.Varchar2, pStrApeCliCot, ParameterDirection.Input, 50);
                AddParameter("pVarApeMatCliCot_in", OracleDbType.Varchar2, pStrApeMatCliCot, ParameterDirection.Input, 100);
                AddParameter("pVarEmailCliCot_in", OracleDbType.Varchar2, pStrEmailCliCot, ParameterDirection.Input, 100);
                AddParameter("pVarEmailAlterCliCot_in", OracleDbType.Varchar2, pStrEmailAlterCliCot, ParameterDirection.Input, 100);
                if (pBolRecibePromo)
                    AddParameter("pChrRecibePromo_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                else
                    AddParameter("pChrRecibePromo_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                AddParameter("pVarDirecCliCot_in", OracleDbType.Varchar2, pStrDirecCliCot, ParameterDirection.Input, 100);
                if (pStrIdTipDoc.Trim() == "-1")
                    AddParameter("pChrIdTipDoc_in", OracleDbType.Char, null, ParameterDirection.Input, 3);
                else
                    AddParameter("pChrIdTipDoc_in", OracleDbType.Char, pStrIdTipDoc, ParameterDirection.Input, 3);
                AddParameter("pVarNumDocCliCot_in", OracleDbType.Varchar2, pStrNumDocCliCot, ParameterDirection.Input, 20);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdWeb_in", OracleDbType.Int32, pIntIdWeb, ParameterDirection.Input);
                if (pDatFecNac.HasValue)
                    AddParameter("pDatFecNacCli_in", OracleDbType.Date, pDatFecNac.Value, ParameterDirection.Input);
                else
                    AddParameter("pDatFecNacCli_in", OracleDbType.Date, null, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Cliente_Cot);
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public int Inserta_Cot_Vta(short pIntModoIng,string pStrTextoSol,string pStrNomUsuCrea,string pStrLoginUsuCrea,
            string pStrIPUsuCrea,int pIntIdCliCotVta,int pIntIdUsuWeb,int pIntIdDep,int pIntIdOfi,int pIntIdWeb,int pIntIdLang,
            short pIntIdCanalVta,string[] pStrArrayServicios,string pStrCodIATAPrinc,int? pIntIdEmpCot,short pIntIdEstOtro, 
            string pStrDestinosPref,DateTime? pDatFecSalida,DateTime? pDatFecRegreso,short? pIntCantPaxAdulto,short? pIntCantPaxNino,
            string pStrPaisResidencia,int? pIntIdReservaVuelos,short? pIntIdSucursalPaq,int? pIntIdWebPaq,int? pintIdCotResPaq,
            string pStrTipoPaq,int? pintIdReservaAuto,int? pintIdReservaSeguro,int? pintIdReservaHotel,string pStrNomGrupo,
            decimal? pNumMontoDscto,int pIntIdOAtencion = 0,int pIntIdEvento = 0)
        {
            int pNumIdNewCliCot_out = 0;
            UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs;
            OracleTransaction objTx = null; OracleConnection objCnx = null;

            try
            {
                ExecuteConexionBegin(unidadNegocio.ToConnectionKey(), ref objTx, ref objCnx);

                AddParameter("pChrModoIng_in", OracleDbType.Int16, pIntModoIng, ParameterDirection.Input);
                AddParameter("pClbTextSol_in", OracleDbType.Clob, pStrTextoSol, ParameterDirection.Input);
                AddParameter("pVarNomUsuCrea_in", OracleDbType.Varchar2, pStrNomUsuCrea, ParameterDirection.Input, 150);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, pStrLoginUsuCrea, ParameterDirection.Input, 50);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea, ParameterDirection.Input, 20);
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCotVta, ParameterDirection.Input);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                AddParameter("pNumIdWeb_in", OracleDbType.Int32, pIntIdWeb, ParameterDirection.Input);
                AddParameter("pNumIdLang_in", OracleDbType.Int32, pIntIdLang, ParameterDirection.Input);
                AddParameter("pNumIdCanalVta_in", OracleDbType.Int16, pIntIdCanalVta, ParameterDirection.Input);
                AddParameter("pChrIATAPrinc_in", OracleDbType.Char, pStrCodIATAPrinc, ParameterDirection.Input, 3);
                if (pIntIdEmpCot.HasValue)
                    AddParameter("pNumIdEmpCot_in", OracleDbType.Int32, pIntIdEmpCot.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdEmpCot_in", OracleDbType.Int32, null, ParameterDirection.Input);
                AddParameter("pNumIdEstOtro_in", OracleDbType.Int16, pIntIdEstOtro, ParameterDirection.Input);
                AddParameter("pVarDestinosPref_in", OracleDbType.Varchar2, pStrDestinosPref, ParameterDirection.Input, 200);
                if (pDatFecSalida.HasValue)
                    AddParameter("pDatFecSalida_in", OracleDbType.Date, pDatFecSalida.Value, ParameterDirection.Input);
                else
                    AddParameter("pDatFecSalida_in", OracleDbType.Date, null, ParameterDirection.Input);
                if (pDatFecRegreso.HasValue)
                    AddParameter("pDatFecRegreso_in", OracleDbType.Date, pDatFecRegreso.Value, ParameterDirection.Input);
                else
                    AddParameter("pDatFecRegreso_in", OracleDbType.Date, null, ParameterDirection.Input);
                if (pIntCantPaxAdulto.HasValue)
                    AddParameter("pNumCantPaxAdulto_in", OracleDbType.Int16, pIntCantPaxAdulto.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumCantPaxAdulto_in", OracleDbType.Int16, null, ParameterDirection.Input);
                if (pIntCantPaxNino.HasValue)
                    AddParameter("pNumCantPaxNino_in", OracleDbType.Int16, pIntCantPaxNino.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumCantPaxNino_in", OracleDbType.Int16, null, ParameterDirection.Input);
                if (string.IsNullOrEmpty(pStrPaisResidencia))
                    AddParameter("pVarPaisResidencia_in", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input, 50);
                else
                    AddParameter("pVarPaisResidencia_in", OracleDbType.Varchar2, pStrPaisResidencia, ParameterDirection.Input, 50);
                if (pIntIdReservaVuelos.HasValue)
                    AddParameter("pNumIdReservaVue_in", OracleDbType.Int32, pIntIdReservaVuelos.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdReservaVue_in", OracleDbType.Int32, null, ParameterDirection.Input);
                if (pIntIdSucursalPaq.HasValue)
                    AddParameter("pNumIdSucPaq_in", OracleDbType.Int16, pIntIdSucursalPaq.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdSucPaq_in", OracleDbType.Int16, null, ParameterDirection.Input);
                if (pIntIdWebPaq.HasValue)
                    AddParameter("pNumIdWebPaq_in", OracleDbType.Int32, pIntIdWebPaq.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdWebPaq_in", OracleDbType.Int32, null, ParameterDirection.Input);
                if (pintIdCotResPaq.HasValue)
                    AddParameter("pNumIdCotResPaq_in", OracleDbType.Int32, pintIdCotResPaq.Value, ParameterDirection.Input);
                else
                    AddParameter("pNumIdCotResPaq_in", OracleDbType.Int32, null, ParameterDirection.Input);
                AddParameter("pVarTipoPaq_in", OracleDbType.Varchar2, pStrTipoPaq, ParameterDirection.Input, 1);
                AddParameter("pVarNomGrupo_in", OracleDbType.Varchar2, pStrNomGrupo, ParameterDirection.Input, 50);
                AddParameter("pNumMontoDscto_in", OracleDbType.Double, pNumMontoDscto, ParameterDirection.Input);
                AddParameter("pNumIdNewCot_out", OracleDbType.Int64, null, ParameterDirection.Output);

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.AW_Insert_CotizacionVta, objTx, null, false);                
                if (!int.TryParse(GetOutParameter("pNumIdNewCot_out").ToString(), out pNumIdNewCliCot_out)) pNumIdNewCliCot_out = 0;                
                #endregion

                #region Servicios
                if (pStrArrayServicios != null)
                {
                    for (Int16 intX = 0; intX <= pStrArrayServicios.Length - 1; intX++)
                        _Insert_ServSol_Cot(pNumIdNewCliCot_out, Convert.ToInt16(pStrArrayServicios[intX]), objTx);
                }
                objTx.Commit();
                #endregion
            }
            catch(Exception ex)
            {
                if (objTx != null)
                    objTx.Rollback();
                throw ex;
            }
            finally
            {
                if (objTx != null)
                    objTx.Dispose();
                objTx = null;
            }

            return pNumIdNewCliCot_out;
        }

        public int _Select_CotId_X_OportunidadSF(string _oportunidadSF)
        {
            int intCotizacion = 0;
            try
            {
                #region Parameter                
                AddParameter("pIdOportunidadSF_in", OracleDbType.Varchar2, _oportunidadSF, ParameterDirection.Input, 18);
                AddParameter("CurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion
                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_DatosCotXIdSF);

                foreach (DataRow row in GetDtParameter("CurResult_out").Rows)
                {
                    if (Convert.IsDBNull(row["COT_ID"]) == false)
                        intCotizacion = Convert.ToInt32(row["COT_ID"]);
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intCotizacion;
        }

        public void RegistraCuenta(string idCuentaSF,int idCuentaNM)
        {
            AddParameter("P_ID_CUENTA_SF", OracleDbType.NVarchar2, idCuentaSF);
            AddParameter("P_ID_CUENTA_NM", OracleDbType.Int32, idCuentaNM);

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Insert_CuentaSF);
            #endregion
        }

        public void RegistraOportunidad(string idOportunidadSF, int idCotizacionNM)
        {
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.NVarchar2, idOportunidadSF);
            AddParameter("P_ID_COTIZACION_NM", OracleDbType.Int32, idCotizacionNM);

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Insert_OportunidadSF);
            #endregion
        }

        public void _Update_DatosReservaVuelo_Manual_Cot(int pIntIdCot, string pStrCodReserva, Int16 pIntIdMoneda, double pDblMontoVta)
        {
            try
            {
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pVarCodResVue_in", OracleDbType.Varchar2, pStrCodReserva, ParameterDirection.Input, 10);
                AddParameter("pNumIdMoneda_in", OracleDbType.Int16, pIntIdMoneda, ParameterDirection.Input);
                AddParameter("pNumMontoVta_in", OracleDbType.Double, pDblMontoVta, ParameterDirection.Input,10);

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_ReservaVueloManual);
                #endregion
                       
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public List<int> _Select_Pedidos_SinBancoBy_IdCot(int pIntIdCot)
        {            
            List<int> lstPedidos = new List<int>();
            try
            {                        
                AddParameter("pNumIdCot_in", OracleDbType.Int32, null, ParameterDirection.Output);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                        
                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_PedidosSinBanco);
                #endregion

                foreach (DataRow row in GetDtParameter("pCurResult_out").Rows)
                {
                    lstPedidos.Add(row.IntParse("NRO_PEDIDO"));
                }
                  
                return lstPedidos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool _Update_Estado_Promociones(int pNumClicot_Id_in, string pEnviaPromo_in)
        {            
            bool valor = false;
            try
            {
                #region Parameter
                AddParameter("pNumClicot_Id_in", OracleDbType.Int32, pNumClicot_Id_in, ParameterDirection.Input);
                AddParameter("pEnviaPromo_in", OracleDbType.Int32, pEnviaPromo_in, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Estado_Promo);
                valor = true;
                #endregion
            }
            catch (Exception ex)
            {                
                throw ex;                
            }
            
            return valor;
        }

        public void _Insert_ServSol_Cot(Int32 pIntIdCot, short pIntIdServSol, OracleTransaction pObjTransaction)
        {   
            AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
            AddParameter("pNumIdServSol_in", OracleDbType.Int16, pIntIdServSol, ParameterDirection.Input);

            ExecuteStorePBeginCommit(StoredProcedureName.AW_Insert_Servicios_Cotizacion, pObjTransaction, null, false);                   
        }

        public bool _EsCounterAdministratiivo(int pIntIdOfi)
        {
            return false;
            /**Programar Funcion**/
            //NMConnection objNMConnection = new NMConnection();
            //OracleCommand objCommand = new OracleCommand();
            //NMOracleParameter objNMOraParam = new NMOracleParameter();
            //try
            //{
            //    objCommand.CommandText = "begin :resultado := " + System.Data.strUsuario_WebsOracle + ".PKG_OFICINA.FN_OFI_ES_CA(:pNumIdOfi_in); end;";
            //    objCommand.CommandType = CommandType.Text;

            //    OracleParameter objParam1 = objCommand.Parameters.Add("rv", OracleDbType.Int16);
            //    objParam1.Direction = ParameterDirection.ReturnValue;

            //    objNMOraParam.AddParameter(objCommand, "pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);

            //    objNMConnection.Oracle_WebsConectar();
            //    objCommand.Connection = objNMConnection.objOracleConexion_Webs;
            //    objCommand.ExecuteNonQuery();
            //    if (objParam1.Value.ToString == "1")
            //        return true;
            //    else
            //        return false;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString());
            //}
            //finally
            //{
            //    objNMConnection.Oracle_WebsDesconectar();
            //    objCommand.Dispose();
            //    objNMConnection = null/* TODO Change to default(_) if this is not a reference type */;
            //    objCommand = null/* TODO Change to default(_) if this is not a reference type */;
            //    objNMOraParam = null/* TODO Change to default(_) if this is not a reference type */;
            //}
        }


        private IEnumerable<ClienteCot> ToClienteCot(DataTable dt)
        {
            try
            {
                var clienteCotList = new List<ClienteCot>();

                foreach (DataRow row in dt.Rows)
                {
                    clienteCotList.Add(new ClienteCot()
                    {
                        IdCliCot = row.IntParse("CLICOT_ID"),
                        NomCliCot = row.StringParse("CLICOT_NOM"),
                        ApeCliCot = row.StringParse("CLICOT_APE"),
                        EmailCliCot = row.StringParse("CLICOT_EMAIL"),
                        ApeMatCliCot = row.StringParse("CLICOT_APE_MAT"),
                        RecibePromo = row.BoolParse("CLICOT_RECIBIR_PROMO")
                    });
                }

                return clienteCotList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Parse
        #endregion
    }
}