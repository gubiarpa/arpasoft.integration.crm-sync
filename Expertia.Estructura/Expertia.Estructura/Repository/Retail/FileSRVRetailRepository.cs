using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;
using System.Web;
using Expertia.Estructura.Repository.AppWebs;

namespace Expertia.Estructura.Repository.Retail
{
    public class FileSRVRetailRepository : OracleBase<object>, IFileSRVRetailRepository
    {
        #region Constructor
        public FileSRVRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetFilesAsociadosSRV()
        {
            var operation = new Operation();
            try
            {
                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_FILES_ASOCIADOS_SRV
                AddParameter(OutParameter.CursorFilesAsociadosSRV, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke              
                ExecuteStoredProcedure(StoredProcedureName.AW_Read_FileAsociadosSRV);
                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.CursorFilesAsociadosSRV] = FillFilesAsociadosSRV(GetDtParameter(OutParameter.CursorFilesAsociadosSRV));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operation;
        }

        public Operation Actualizar_EnvioCotRetail(FilesAsociadosSRVResponse FileAsociadosSRVResponse)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (03) P_OPORTUNIDAD_CRM
            AddParameter("P_OPORTUNIDAD_CRM", OracleDbType.Varchar2, FileAsociadosSRVResponse.id_oportunidad_sf);
            /// (04) P_ES_ATENCION
            AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, FileAsociadosSRVResponse.codigo_error);
            /// (05) P_DESCRIPCION
            AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, FileAsociadosSRVResponse.mensaje_error);
            /// (06) P_ACTUALIZADOS
            AddParameter(OutParameter.NumeroActualizados, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_EnvioCotRetail);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.NumeroActualizados] = GetOutParameter(OutParameter.NumeroActualizados);
            #endregion

            return operation;
        }

        private void _Update_RC_Pedido(int pIntIdPedido, int pIntIdSucursal, int pIntIdRecibo)
        {
            try
            {
                #region Parameter                                
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, pIntIdPedido, ParameterDirection.Input);
                AddParameter("pNumIdSucursal_in", OracleDbType.Int32, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdRecibo_in", OracleDbType.Int32, pIntIdRecibo, ParameterDirection.Input);
                AddParameter(OutParameter.NumeroActualizados, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Upd_RC_Pedido);           
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private ArrayList _Insert_ReciboCaja(int pIntIdSucursal, int pIntIdFile, bool pBolEsCCCF, int pIntIdPedido, double pDblMontoPedido, double pDblMontoPedidoRound, string pStrNroTarjeta, string pStrIdForma, string pStrIdValor, string pStrIdUsuBD, DateTime pDatFecPedido, string pStrComentarios, bool pBolEsRutaSelva, bool pBolEsResPub, OracleTransaction pObjTx)
        {   
            ArrayList alstResultado = new ArrayList();
            string strResult = string.Empty; string strComentResult = string.Empty; string strLog = string.Empty;
        
            try
            {               
                AddParameter("pNumIdSucursal_in", OracleDbType.Int32, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pNumEsCCCF_in", OracleDbType.Int32, 0, ParameterDirection.Input);
                AddParameter("pNumIdPedido_in", OracleDbType.Int32, pIntIdPedido, ParameterDirection.Input);
                AddParameter("pNumMontoPedido", OracleDbType.Double, pDblMontoPedido, ParameterDirection.Input, 10);
                AddParameter("pNumMontoPedidoRound", OracleDbType.Double, pDblMontoPedidoRound, ParameterDirection.Input, 10);
                AddParameter("pVarNroTarjeta_in", OracleDbType.Varchar2, pStrNroTarjeta, ParameterDirection.Input, 20);
                AddParameter("pVarIdForma_in", OracleDbType.Varchar2, pStrIdForma, ParameterDirection.Input, 5);
                AddParameter("pChrIdValor_in", OracleDbType.Varchar2, pStrIdValor, ParameterDirection.Input, 5);
                AddParameter("pVarIdUsuBD_in", OracleDbType.Varchar2, pStrIdUsuBD, ParameterDirection.Input, 20);
                AddParameter("pDatFecPedido_in", OracleDbType.Date, Convert.ToDateTime(pDatFecPedido.ToString("dd/MM/yyyy")), ParameterDirection.Input);
                AddParameter("pVarComentarios_in", OracleDbType.Varchar2, pStrComentarios, ParameterDirection.Input, 2000);
                if (pBolEsRutaSelva)
                    AddParameter("pChrEsRutaSelva_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                else
                    AddParameter("pChrEsRutaSelva_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                if (pBolEsResPub)
                    AddParameter("pChrEsReservaPub_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                else
                    AddParameter("pChrEsReservaPub_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);

                AddParameter("pVarResult_out", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                AddParameter("pVarComent_out", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                AddParameter("pVarLog_out", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);

                ExecuteStorePBeginCommit(StoredProcedureName.NM_Ins_ReciboCaja, pObjTx, null, false);

                /*strResult: 1=generó RC, 0=no generó*/
                strResult = Convert.ToString(GetOutParameter("pVarResult_out").ToString());
                strComentResult = Convert.ToString(GetOutParameter("pVarComent_out").ToString());
                strLog = Convert.ToString(GetOutParameter("pVarLog_out").ToString());
                                        
                alstResultado.Add(strResult);
                alstResultado.Add(strComentResult);
                alstResultado.Add(strLog);               
            }
            catch (Exception ex)
            {
                System.Text.StringBuilder sbDatos = new System.Text.StringBuilder();
                sbDatos.Append("pIntIdSucursal: " + pIntIdSucursal + "<br>");
                sbDatos.Append("pIntIdFile: " + pIntIdFile + "<br>");
                sbDatos.Append("pBolEsCCCF: " + pBolEsCCCF + "<br>");
                sbDatos.Append("pIntIdPedido: " + pIntIdPedido + "<br>");
                sbDatos.Append("pDblMontoPedido: " + pDblMontoPedido + "<br>");
                sbDatos.Append("pDblMontoPedidoRound: " + pDblMontoPedidoRound + "<br>");
                sbDatos.Append("pStrNroTarjeta: " + pStrNroTarjeta + "<br>");
                sbDatos.Append("pStrIdForma: " + pStrIdForma + "<br>");
                sbDatos.Append("pStrIdValor: " + pStrIdValor + "<br>");
                sbDatos.Append("pStrIdUsuBD: " + pStrIdUsuBD + "<br>");
                sbDatos.Append("pDatFecPedido: " + pDatFecPedido + "<br>");
                sbDatos.Append("pStrComentarios: " + pStrComentarios + "<br>");
                sbDatos.Append("pStrComentarios: " + pStrComentarios + "<br>");
                sbDatos.Append("strLog: " + strLog + "<br>");
                throw new Exception(ex.ToString() + "<br><br>DATOS:" + sbDatos.ToString());
            }
            return alstResultado;
        }

        public ArrayList Inserta_ReciboCaja(List<Models.FilePTACotVta> pLstFiles, bool pBolEsCCCF, int pIntIdPedido, double pDblMontoPedido, double pDblMontoPedidoRound, string pStrNroTarjeta, string pStrIdForma, string pStrIdValor, string pStrIdUsuBD, DateTime pDatFecPedido, string pStrComentarios, bool pBolEsRutaSelva, bool pBolEsResPub)
        {
            ArrayList alstRpta = new ArrayList();
            bool bolOK = true;
            try
            {
                UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.NuevoMundo;
                OracleTransaction objTx = null; OracleConnection objConn = null;

                ExecuteConexionBegin(unidadNegocio.ToConnectionKey(), ref objTx, ref objConn);
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Alter_SessionDT, objTx, null, false);
                
                foreach (Models.FilePTACotVta objFilePTACotVta in pLstFiles)
                {
                    alstRpta = _Insert_ReciboCaja(objFilePTACotVta.IdSuc,objFilePTACotVta.IdFilePTA,pBolEsCCCF,
                                        pIntIdPedido,pDblMontoPedido,pDblMontoPedidoRound,pStrNroTarjeta,pStrIdForma,
                                        pStrIdValor,pStrIdUsuBD,pDatFecPedido,pStrComentarios,pBolEsRutaSelva,
                                        pBolEsResPub,objTx);
                     
                    if (alstRpta.Count > 0)
                    {
                        if (alstRpta[0].ToString() != "1")
                        {
                            bolOK = false;
                            objTx.Rollback();
                            break;
                        }
                    }
                    else
                    {
                        bolOK = false;
                        objTx.Rollback();
                        break;
                    }

                    try /*Se asigna el RC al pedido generado*/
                    {
                        if (alstRpta.Count > 1)
                        {
                            string strIdsRCTmp = alstRpta[1].ToString();
                            foreach (string strIdRC in strIdsRCTmp.Split(';'))
                            {
                                if (!string.IsNullOrEmpty(strIdRC))
                                {                                    
                                    _Update_RC_Pedido(pIntIdPedido, objFilePTACotVta.IdSuc, System.Convert.ToInt32(strIdRC));
                                }
                            }
                        }
                    }
                    catch (Exception ex){}
                }
                if (bolOK){objTx.Commit(); }

                objTx.Dispose(); objConn.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return alstRpta;
        }

        private void _Insert_HistoriaFile(Int16 pIntIdEmpresa, Int16 pIntIdSucursal, int pIntIdFile, string pStrIdVendedorRegistra, string pStrLoginUsuWebRegistra, string pStrComentario, OracleTransaction pObjTx)
        {            
            try
            {
                #region Parameter                                
                AddParameter("pNumIdEmpresa_in", OracleDbType.Int16, pIntIdEmpresa, ParameterDirection.Input);
                AddParameter("pNumIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pVarIdVend_in", OracleDbType.Varchar2, pStrIdVendedorRegistra, ParameterDirection.Input, 3);
                AddParameter("pVarLoginVend_in", OracleDbType.Varchar2, pStrLoginUsuWebRegistra, ParameterDirection.Input, 30);
                AddParameter("pVarComentario_in", OracleDbType.Varchar2, pStrComentario, ParameterDirection.Input, 255);
                #endregion

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Ins_Historia_File, pObjTx, null,  false);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private DataTable _Select_File(int pIntIdFile, Int16 pIntIdSucursal, OracleConnection pObjCnx)
        {            
            try
            {
                #region Parameter                
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pIntIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Get_Select_File,null, pObjCnx, false);
                #endregion

                return GetDtParameter("pCurResult_out");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }             
        }

        private void _Update_IdVendedor_File(Int16 pIntIdSucursal, int pIntIdFile, string pStrIdVendedor, OracleTransaction pObjTx)
        {          
            try
            {
                #region Parameter                                
                AddParameter("pNumIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pVarIdVendedor_in", OracleDbType.Varchar2, pStrIdVendedor, ParameterDirection.Input, 3);
                #endregion

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Upd_IdVendedor_File, pObjTx, null, false);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private bool _Update_PagoUATP_ExoneradoFirmaCliente_File(Int16 pIntIdSucursal, int pIntIdFile, bool pBolUATPExoneradoFirmaCliente, OracleTransaction pObjTx)
        {            
            bool bolResultado = false;
            try
            {
                #region Parameter                                
                AddParameter("pNumIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                if (pBolUATPExoneradoFirmaCliente)
                {
                    AddParameter("pNumUATPExoneradoFirma_in", OracleDbType.Int16, 1, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pNumUATPExoneradoFirma_in", OracleDbType.Int16, 2, ParameterDirection.Input);
                }
                AddParameter("pChrResult_out", OracleDbType.Char, 1, ParameterDirection.Output, 1);
                #endregion

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Upd_PagoUATP_File, pObjTx, null, false);

                if (Convert.IsDBNull(GetOutParameter("pChrResult_out")))
                {
                    bolResultado = false;
                }
                else if (GetOutParameter("pChrResult_out").ToString() == "1")
                {
                    bolResultado = true;
                }   
                #endregion

                return bolResultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void _update_Vendedor_Pta(Int16 pIntIdSucursal, int pIntIdFile, string pStrIdVendedorRegistra, OracleTransaction pObjTx)
        {            
            try
            {
                #region Parameter                                
                AddParameter("pNumIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pVarIdVend_in", OracleDbType.Varchar2, pStrIdVendedorRegistra, ParameterDirection.Input, 3);
                #endregion

                #region Invoke
                ExecuteStorePBeginCommit(StoredProcedureName.NM_Upd_Vendedor_Pta, pObjTx, null, false);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void _Update_FechaCierreVenta(List<Models.FilePTACotVta> pLstFiles, Int16 pIntIdEmpresa, string pStrIdVendedorRegistra, string pStrIdVendedorCreaCot, string pStrLoginUsuWebRegistra, Nullable<bool> pBolUATPExoneradoFirmaCliente, string pStrNomVendedorCounter, bool pActualizaVendedor)
        {  
            try
            {   
                DataTable dtFile;
                DataRow drFile;
                string strNomVendedor = "";
                string strUATPExoneradoFirmaCliente = "";

                UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.NuevoMundo;
                OracleTransaction objTx = null; OracleConnection objCnx = null;
                ExecuteConexionBegin(unidadNegocio.ToConnectionKey(), ref objTx, ref objCnx);

                foreach (Models.FilePTACotVta objFilePTACotVta in pLstFiles)
                {                               
                    AddParameter("pNumIdSucursal_in", OracleDbType.Int16, objFilePTACotVta.IdSuc, ParameterDirection.Input);
                    AddParameter("pNumIdFile_in", OracleDbType.Int32, objFilePTACotVta.IdFilePTA, ParameterDirection.Input);
                    AddParameter("pVarIdVend_in", OracleDbType.Varchar2, pStrIdVendedorCreaCot, ParameterDirection.Input, 3);
                    AddParameter("pVarLoginVend_in", OracleDbType.Varchar2, pStrLoginUsuWebRegistra, ParameterDirection.Input, 30);
                    ExecuteStorePBeginCommit(StoredProcedureName.NM_Upd_Fecha_Cierre_File, objTx, null, false);                    
                                                    
                    _Insert_HistoriaFile(pIntIdEmpresa, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pStrIdVendedorRegistra, pStrLoginUsuWebRegistra, "CIERRE VENTA DEFINITIVO ACTIVADO DESDE SRV", objTx);                    
                    dtFile = _Select_File(objFilePTACotVta.IdFilePTA, objFilePTACotVta.IdSuc, objCnx);
                    
                    drFile = null;
                    strNomVendedor = "";
                    strUATPExoneradoFirmaCliente = "";
                    if (dtFile.Rows.Count > 0)
                    {
                        drFile = dtFile.Rows[0];
                        if (!Convert.IsDBNull(drFile["NOM_VENDEDOR"]))
                            strNomVendedor = drFile["NOM_VENDEDOR"].ToString();
                        if (!Convert.IsDBNull(drFile["PAGO_UATP_CON_FIRMA"]))
                            strUATPExoneradoFirmaCliente = drFile["PAGO_UATP_CON_FIRMA"].ToString();
                    }

                    _Update_IdVendedor_File(objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pStrIdVendedorCreaCot, objTx);
                    _Insert_HistoriaFile(pIntIdEmpresa, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pStrIdVendedorRegistra, pStrLoginUsuWebRegistra, "MOD COUNTER --> " + strNomVendedor + " A " + pStrNomVendedorCounter + " desde SRV", objTx);

                    if (pBolUATPExoneradoFirmaCliente.HasValue)
                    {
                        if (_Update_PagoUATP_ExoneradoFirmaCliente_File(objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pBolUATPExoneradoFirmaCliente.Value, objTx))
                        {
                            string strValorExonerado = "0";                            
                            if (pBolUATPExoneradoFirmaCliente.Value) /*Si en el SRV es verdadero, en PTA es falso y viceversa*/
                            {
                                strValorExonerado = "1";
                            }
                            _Insert_HistoriaFile(pIntIdEmpresa, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pStrIdVendedorRegistra, pStrLoginUsuWebRegistra, "MOD pago exonerado de firma: de " + Interaction.IIf(strUATPExoneradoFirmaCliente == "1", "SI", "NO") + " a " + Interaction.IIf(strValorExonerado == "1", "SI", "NO") + " desde SRV", objTx);
                        }
                    }

                    if (pActualizaVendedor)
                    {
                        _update_Vendedor_Pta(objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, pStrIdVendedorCreaCot, objTx);
                    }                        
                }
                objTx.Commit();

                objTx.Dispose();
                objCnx.Dispose();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.ToString());
            }            
        }

        public DataTable _Get_ComprobantesBoletosBy_IdFile(int pIntIdFile, Int16 pIntIdSucursal)
        {
            try
            {
                #region Parameter                
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pNumIdSucursal_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output,10);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.NM_Get_ComprobanteXFile);
                #endregion

                return GetDtParameter("pCurResult_out");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally{}
        }

        public bool _Existe_Vendedor_SubArea(Int16 pIntIdArea, Int16 pIntIdSubArea, string pStrIdVendedor)
        {

            bool bolResult = false;
            try
            {
                #region Parameter                
                AddParameter("pNumIdArea_in", OracleDbType.Int16, pIntIdArea, ParameterDirection.Input);
                AddParameter("pNumIdSubArea_in", OracleDbType.Int16, pIntIdSubArea, ParameterDirection.Input);
                AddParameter("pVarIdVend_in", OracleDbType.Varchar2, pStrIdVendedor, ParameterDirection.Input,3);
                AddParameter("pChrResult_out", OracleDbType.Char, null, ParameterDirection.Output,1);                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.NM_Existe_VendedorSubArea);
                if (GetOutParameter("pChrResult_out").ToString().Trim() == "1")
                { 
                    bolResult = true;
                }
                #endregion
            }
            catch (Exception ex)
            {                
                throw ex;
            }

            return bolResult;            
        }

        public void _Insert_TextoFile(int pIntIdFile, Int16 pIntIdSucursal, string pStrTextoFile, string pStrLoginUsuWeb, Int16 pIntIdEmpresa)
        {            
            try
            {
                #region Parameter                                
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pNumIdSuc_in", OracleDbType.Int16, pIntIdSucursal, ParameterDirection.Input);
                AddParameter("pVarRenglon_in", OracleDbType.Varchar2, pStrTextoFile, ParameterDirection.Input, 255);
                AddParameter("pVarLoginUsuWeb_in", OracleDbType.Varchar2, pStrLoginUsuWeb, ParameterDirection.Input, 30);
                AddParameter("pNumIdEmpresa_in", OracleDbType.Int16, pIntIdEmpresa, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.NM_Ins_Text_File);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public DataTable _SelectFilesIdBy_IdCot(int pIntIdCotVta)
        { 
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCotVta, ParameterDirection.Input);                
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_File_Cot_XID);
                return GetDtParameter("pCurResult_out");                
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }                        
        }
                

        public void _Delete_Cot_File(int pIntCotId, int pIntFileId)
        { 
            try
            {
                #region Parameter                                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntCotId, ParameterDirection.Input);
                AddParameter("pFileId_in", OracleDbType.Int32, pIntFileId, ParameterDirection.Input);              
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Del_Cot_File,true);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            } 
        }

        public void _Insert(int pIntIdUsuario, string pStrNomPagina, string pStrComment, int pIntIdLang, int pIntIdWeb, string pStrQuery, string pStrIP)
        {            
            try
            {
                #region Parameter   
                AddParameter("pNumIdUsuario_in", OracleDbType.Int64, pIntIdUsuario, ParameterDirection.Input);
                AddParameter("pVarQuery_in", OracleDbType.Clob, pStrQuery, ParameterDirection.Input);
                AddParameter("pVarNomPagina_in", OracleDbType.Varchar2, pStrNomPagina, ParameterDirection.Input, 100);
                AddParameter("pVarComment_in", OracleDbType.Varchar2, pStrComment, ParameterDirection.Input, 200);
                AddParameter("pNumIdLang_in", OracleDbType.Int64, pIntIdLang, ParameterDirection.Input);
                AddParameter("pNumIdWeb_in", OracleDbType.Int64, pIntIdWeb, ParameterDirection.Input);
                AddParameter("pVarIP_in", OracleDbType.Varchar2, pStrIP, ParameterDirection.Input, 30);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Ins_LogTwo, true);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Auxiliares
        private IEnumerable<FilesAsociadosSRV> FillFilesAsociadosSRV(DataTable dt)
        {
            try
            {
                var FilesAsociadosSRVList = new List<FilesAsociadosSRV>();

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region AddingElement
                        FilesAsociadosSRVList.Add(new FilesAsociadosSRV()
                                                {
                                                    id_oportunidad = row.StringParse("ID_OPORTUNIDAD"),
                                                    cot_id = row.IntParse("COT_ID"),
                                                    suc_id = row.IntParse("SUC_ID"),
                                                    file_id = row.IntParse("FILE_ID"),
                                                    fpta_fecha = row.DateTimeParse("FPTA_FECHA"),
                                                    fpta_imp_fact = (double)row["FPTA_IMP_FACT"]
                                                });
                        #endregion
                    }
                }

                return FilesAsociadosSRVList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}