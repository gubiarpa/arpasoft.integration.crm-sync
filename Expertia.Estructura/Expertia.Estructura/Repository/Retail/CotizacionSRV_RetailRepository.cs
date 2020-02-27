using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
namespace Expertia.Estructura.Repository.AppWebs
{
    public class CotizacionSRV_AW_Repository : OracleBase<Pedido>, ICotizacionSRV_Repository
    {
        #region Constructor
        public CotizacionSRV_AW_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        public int ProcesosPostCotizacion(Post_SRV RQ_General_PostSRV)
        {
            int IdPostFin = 0;
            IdPostFin = InsertCotizacionPost(RQ_General_PostSRV);

            if (RQ_General_PostSRV.CambioEstado)
            {
                UpdateEstadoCotVTA(RQ_General_PostSRV);             
            }

            return IdPostFin;
        }

        public int InsertCotizacionPost(Post_SRV RQ_PostSRV)
        {
            int _returnIdPost = 0;
            try
            {
                #region Parameter
                AddParameter("pNumIdCot_in", OracleDbType.Int32, RQ_PostSRV.IdCot, ParameterDirection.Input);
                AddParameter("pChrTipoPost_in", OracleDbType.Char, RQ_PostSRV.TipoPost, ParameterDirection.Input, 1);
                AddParameter("pClbTextoPost_in", OracleDbType.Clob, RQ_PostSRV.TextoPost, ParameterDirection.Input);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, RQ_PostSRV.IPUsuCrea, ParameterDirection.Input, 20);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, RQ_PostSRV.LoginUsuCrea, ParameterDirection.Input, 50);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, RQ_PostSRV.IdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, RQ_PostSRV.IdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, RQ_PostSRV.IdOfi, ParameterDirection.Input);
                
                if (RQ_PostSRV.CambioEstado)
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Char, "S", ParameterDirection.Input, 1);
                    AddParameter("pNumIdEst_in", OracleDbType.Int16, RQ_PostSRV.IdEstado, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Char, "N", ParameterDirection.Input, 1);
                    AddParameter("pNumIdEst_in", OracleDbType.Int16, null, ParameterDirection.Input);
                }
                if (RQ_PostSRV.EsAutomatico)
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                else
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);

                if (RQ_PostSRV.EsUrgenteEmision.HasValue)
                {
                    if (RQ_PostSRV.EsUrgenteEmision.Value)
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                    else
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                }
                else
                    AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, null, ParameterDirection.Input, 1);

                if (RQ_PostSRV.FecPlazoEmision.HasValue)
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Date, RQ_PostSRV.FecPlazoEmision.Value, ParameterDirection.Input);
                else
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Date, null, ParameterDirection.Input);

                AddParameter(OutParameter.NumeroIdPostSRV, OracleDbType.Int32, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Insert_Post_Cotizacion);
                _returnIdPost = Convert.ToInt32((decimal)(Oracle.ManagedDataAccess.Types.OracleDecimal)(GetOutParameter(OutParameter.NumeroIdPostSRV)));                
                #endregion

                return _returnIdPost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEstadoCotVTA(Post_SRV RQ_PostSRV)
        {
            try
            {
                #region Variables - Validaciones
                string DetalleAccion = string.Empty;
                int? IdUsuWebCounterCreaVal = null;

                if (RQ_PostSRV.EsCounterAdmin == true) {
                    IdUsuWebCounterCreaVal = RQ_PostSRV.IdUsuWeb;
                }

                if (RQ_PostSRV.EsAutomatico)
                    DetalleAccion = "Usuario " + (RQ_PostSRV.EsCounterAdmin == true ? RQ_PostSRV.IdUsuWebCounterCrea.Value : RQ_PostSRV.IdUsuWeb) + " cambia estado de cotización '" + RQ_PostSRV.IdCot + "' a " + RQ_PostSRV.IdEstado + "(automático)";
                else if (IdUsuWebCounterCreaVal.HasValue)
                    DetalleAccion = "Usuario " + IdUsuWebCounterCreaVal.Value + " cambia estado de cotización '" + RQ_PostSRV.IdCot + "' a " + RQ_PostSRV.IdEstado;
                else
                    DetalleAccion = "Usuario " + (RQ_PostSRV.EsCounterAdmin == true ? RQ_PostSRV.IdUsuWebCounterCrea.Value : RQ_PostSRV.IdUsuWeb) + " cambia estado de cotización '" + RQ_PostSRV.IdCot + "' a " + RQ_PostSRV.IdEstado;
                #endregion
                
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, RQ_PostSRV.IdCot, ParameterDirection.Input);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, RQ_PostSRV.LoginUsuCrea, ParameterDirection.Input, 50);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, RQ_PostSRV.IPUsuCrea, ParameterDirection.Input, 20);
                AddParameter("pVarDetAccion_in", OracleDbType.Varchar2, DetalleAccion, ParameterDirection.Input, 300);
                AddParameter("pNumIdEstado_in", OracleDbType.Int16, RQ_PostSRV.IdEstado, ParameterDirection.Input);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, (RQ_PostSRV.EsCounterAdmin == true ? RQ_PostSRV.IdUsuWebCounterCrea.Value : RQ_PostSRV.IdUsuWeb), ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, (RQ_PostSRV.EsCounterAdmin == true ? RQ_PostSRV.IdDepCounterCrea.Value : RQ_PostSRV.IdDep), ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, (RQ_PostSRV.EsCounterAdmin == true ? RQ_PostSRV.IdOfiCounterCrea.Value : RQ_PostSRV.IdOfi), ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Estado_Cotizacion);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool _Liberar_UsuWeb_CA(int pIntIdCot)
        {
            throw new NotImplementedException();
        }

        public int _Insert_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost, string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, short pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta, bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, int? pIntIdUsuWebCounterCrea, int? pIntIdOfiCounterCrea, int? pIntIdDepCounterCrea, bool? pBolEsUrgenteEmision, DateTime? pDatFecPlazoEmision, short? pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, double? pDblMontoEstimadoFile)
        {
            int intIdPost = 0;
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pChrTipoPost_in", OracleDbType.Varchar2, pStrTipoPost, ParameterDirection.Input, 50);
                AddParameter("pClbTextoPost_in", OracleDbType.Varchar2, pStrTextoPost, ParameterDirection.Input, 20);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea, ParameterDirection.Input, 300);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Int16, pStrLoginUsuCrea, ParameterDirection.Input);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                if (pBolCambioEstado)
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                    AddParameter("pNumIdEst_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                    AddParameter("pNumIdEst_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                }

                if (pBolEsAutomatico)
                {
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                }

                if (pBolEsUrgenteEmision.HasValue)
                {
                    if (pBolEsUrgenteEmision.Value)
                    {
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "1", ParameterDirection.Input);
                    }
                    else
                    {
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "0", ParameterDirection.Input);
                    }

                }
                else
                {
                    AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, null, ParameterDirection.Input);
                }

                if (pDatFecPlazoEmision.HasValue)
                {
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Char, null, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Char, null, ParameterDirection.Input);
                }

                AddParameter("pNumIdNewPost_out", OracleDbType.Int32, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Insert_Post_Cotizacion);
                intIdPost = (int)GetOutParameter("pNumIdNewPost_out");
                #endregion

                return intIdPost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Update_MotivoNoCompro(int pIntIdCot, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro)
        {
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pNumIdMotivo_in", OracleDbType.Int32, pIntIdMotivoNoCompro, ParameterDirection.Input);
                AddParameter("pVarOtroMotivo_in", OracleDbType.Varchar2, pStrOtroMotivoNoCompro, ParameterDirection.Input, 100);                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Motivo_No_Compra);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FilePTACotVta> _SelectFilesPTABy_IdCot(int pIntIdCotVta, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep)
        {
            List<FilePTACotVta> lstFilesPTA = new List<FilePTACotVta>();
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCotVta, ParameterDirection.Input);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_FilesPtaBy_IdCot);
                
                FilePTACotVta objFilePTACotVta;
                foreach (DataRow row in GetDtParameter("pCurResult_out").Rows)
                {
                    objFilePTACotVta = new FilePTACotVta();
                    objFilePTACotVta.IdCot = pIntIdCotVta;
                    objFilePTACotVta.IdSuc = (Int16)row["SUC_ID"];
                    objFilePTACotVta.IdFilePTA = (int)row["FILE_ID"];
                    objFilePTACotVta.Fecha = (DateTime)row["FPTA_FECHA"];
                    objFilePTACotVta.Moneda = (string)row["FPTA_MONEDA"];
                    objFilePTACotVta.ImporteFacturado = (Double)row["FPTA_IMP_FACT"];
                    objFilePTACotVta.TipoCambio = (Double)row["FPTA_TIPO_CAMBIO"];
                    objFilePTACotVta.NombreSucursal = _Select_NomSucursal((Int16)row["SUC_ID"]);
                    lstFilesPTA.Add(objFilePTACotVta);
                }
                
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstFilesPTA;
        }

        public string _Select_NomSucursal(int pIntIdSuc)
        {
            string strNomSuc = "";
            try
            {
                #region Parameter                
                AddParameter("pIntIdSucursal_in", OracleDbType.Int16, pIntIdSuc, ParameterDirection.Input);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion
                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.IA_Get_SucursalBy_Id);
                
                foreach (DataRow row in GetDtParameter("pCurResult_out").Rows)
                {
                    if (row["DESCRIPCION"] == null)
                        strNomSuc = "";
                    else
                        strNomSuc = row["DESCRIPCION"].ToString();
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strNomSuc;
        }

        public DataTable _Select_InfoFile(int pIntIdSuc, int pIntIdFile)
        {
            try
            {
                #region Parameter                
                AddParameter("v_sucursal", OracleDbType.Int16, pIntIdSuc, ParameterDirection.Input);
                AddParameter("v_file", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("cv_1", OracleDbType.RefCursor, null, ParameterDirection.Output);
                #endregion

                DataTable dtImportesFile = new DataTable();
                dtImportesFile.Columns.Add(new DataColumn("FECHA_EMISION", typeof(string)));
                dtImportesFile.Columns.Add(new DataColumn("ID_CLIENTE", typeof(int)));
                dtImportesFile.Columns.Add(new DataColumn("NOMBRE_CLIENTE", typeof(string)));
                dtImportesFile.Columns.Add(new DataColumn("ID_MONEDA", typeof(string)));
                dtImportesFile.Columns.Add(new DataColumn("IMPORTE_TOTAL", typeof(double)));
                dtImportesFile.Columns.Add(new DataColumn("IMPORTE_AFECTADO", typeof(double)));
                dtImportesFile.Columns.Add(new DataColumn("FLAG", typeof(string)));
                DataRow drImporteFile = null;

                using (OracleDataAdapter objDataAdapter = new OracleDataAdapter())
                {
                    objDataAdapter.Fill(dtImportesFile);

                }
                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Facturacion_os_Tkts_Util);
                #endregion


                return dtImportesFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Actualiza_Imp_File_Cot(int pIntIdCot, int pIntIdSuc, int pIntIdFile, string strIdMoneda, double dblImporteSuma, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep, string pStrEsUpdUsuario)
        {
            int intIdCotNew = 0;
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pNumIdSuc_in", OracleDbType.Int32, pIntIdSuc, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, pIntIdFile, ParameterDirection.Input);
                AddParameter("pChrIdMoneda_in", OracleDbType.Char, strIdMoneda, ParameterDirection.Input,3);
                AddParameter("pDecImpFact_in", OracleDbType.Decimal, dblImporteSuma, ParameterDirection.Input);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input, 100);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pChrEsUpdUsu_in", OracleDbType.Char, pStrEsUpdUsuario, ParameterDirection.Input, 1);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Imp_File_Cot);
                intIdCotNew = 999;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double _Select_TipoCambio(DateTime pDatFecha, string pStrMoneda, short pIntIdEmp, bool pBolEsBDNuevoMundo)
        {
            double dblTipoCambio = 0;
            string strCnx = "";

            try
            {
                #region Parameter                
                AddParameter("pDatFecha_in", OracleDbType.Date, pDatFecha, ParameterDirection.Input);
                AddParameter("pVarIdMoneda_in", OracleDbType.Varchar2, pStrMoneda, ParameterDirection.Input, 3);
                AddParameter("pNumIdEmpresa_in", OracleDbType.Int32, pIntIdEmp, ParameterDirection.Input);
                AddParameter("pNumTipoCambio_out", OracleDbType.Double, null, ParameterDirection.Output);
                
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Tipo_Cambio);
                if (GetOutParameter("pNumIdNewPost_out") == null)
                {
                    dblTipoCambio = 0;
                }
                else
                {
                    dblTipoCambio = (double)GetOutParameter("pNumIdNewPost_out");
                }
                #endregion
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return dblTipoCambio;
        }

        public void _Insert_FilePTA_Cot(int IdCot, int IdSuc, int IdFilePTA, string Moneda, double dblTipoCambio, double ImporteFacturado, int IdUsuWebCounterCrea, int IdOfiCounterCrea, int IdDepCounterCrea)
        {
            int intId = 0;
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, IdCot, ParameterDirection.Input);
                AddParameter("pNumIdSuc_in", OracleDbType.Int32, IdSuc, ParameterDirection.Input);
                AddParameter("pNumIdFile_in", OracleDbType.Int32, IdFilePTA, ParameterDirection.Input);
                AddParameter("pChrMoneda_in", OracleDbType.Char, Moneda, ParameterDirection.Input, 3);
                AddParameter("pDecImpFact_in", OracleDbType.Decimal, ImporteFacturado, ParameterDirection.Input);
                AddParameter("pDecTipoCambio_in", OracleDbType.Decimal, dblTipoCambio, ParameterDirection.Input, 100);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, IdUsuWebCounterCrea, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, IdOfiCounterCrea, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, IdDepCounterCrea, ParameterDirection.Input, 1);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Ins_FilePTA_Cot);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Insert_ArchivoMail_Post_Cot(int pIntIdCot, int intIdPost, string pBytArchivoMail)
        {
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pNumIdPost_in", OracleDbType.Int32, intIdPost, ParameterDirection.Input);
                AddParameter("pBlbArchivoMail_in", OracleDbType.Int32, pBytArchivoMail, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Ins_FilePTA_Cot);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Update_MontoEstimadoFileBy_IdCotVta(int pIntIdCot, double pDblMontoEstimadoFile)
        {
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pNumMonto_in", OracleDbType.Int16, pDblMontoEstimadoFile, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Update_Monto_Estimado_File);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _Insert_FechaSalida_Cot(int pIntIdCot, string pStrFecSalida, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi)
        {
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pVarFecSal_in", OracleDbType.Varchar2, pStrFecSalida, ParameterDirection.Input,10);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Ins_Fec_Salida_Cot);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}