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

        public CotizacionVta Get_Datos_CotizacionVta(int IdCotSRV)
        {
            try
            {
                CotizacionVta _CotizacionVta = new CotizacionVta();

                #region Parameter
                AddParameter("pNumIdCot_in", OracleDbType.Int32, IdCotSRV, ParameterDirection.Input);
                AddParameter(OutParameter.CursorDtosCotizacion, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Datos_Cotizacion);
                _CotizacionVta = FillDtsCotizacion(GetDtParameter(OutParameter.CursorDtosCotizacion));                
                #endregion

                return _CotizacionVta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ProcesosPostCotizacion(Post_SRV RQ_General_PostSRV)
        {
            int IdPostFin = 0;
            IdPostFin = InsertCotizacionPost(RQ_General_PostSRV);

            if (RQ_General_PostSRV.CambioEstado)
            {
                UpdateEstadoCotVTA(RQ_General_PostSRV);             
            }

            //if(RQ_General_PostSRV.IdEstado == (Int16)ENUM_ESTADOS_COT_VTA.Facturado)
            //{
            //    foreach (FilePTACotVta objTempFilePTA in RQ_General_PostSRV.LstFilesPTA)
            //    {
            //        DataTable dtImporteFile = _Select_InfoFile(objTempFilePTA.IdSuc, objTempFilePTA.IdFilePTA);
            //        _Actualiza_Imp_File_Cot(,)
            //    }
            //        _Update_Importe_FilePTA(pIntIdCot, objTempFilePTA.IdSucursal, objTempFilePTA.IdFilePTA, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, "1", objTx);
            //}

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
            bool bolAsignado = false;
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Liberar_Usuweb_CA);
                bolAsignado = true;
                #endregion
            }
            catch (Exception ex)
            {
                bolAsignado = false;
                throw ex;
            }

            return bolAsignado;
        }

        public int _Insert_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost, string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, short pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta, bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, int? pIntIdUsuWebCounterCrea, int? pIntIdOfiCounterCrea, int? pIntIdDepCounterCrea, bool? pBolEsUrgenteEmision, DateTime? pDatFecPlazoEmision, short? pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, double? pDblMontoEstimadoFile)
        {
            string valor = "";
            int intIdPost = 0;
            try
            {
                #region Parameter                
                AddParameter("pNumIdCot_in", OracleDbType.Int32, pIntIdCot, ParameterDirection.Input);
                AddParameter("pChrTipoPost_in", OracleDbType.Char, pStrTipoPost, ParameterDirection.Input, 1);
                AddParameter("pClbTextoPost_in", OracleDbType.Clob, pStrTextoPost, ParameterDirection.Input);
                AddParameter("pVarIPUsuCrea_in", OracleDbType.Varchar2, pStrIPUsuCrea, ParameterDirection.Input, 20);
                AddParameter("pVarLoginUsuCrea_in", OracleDbType.Varchar2, pStrLoginUsuCrea, ParameterDirection.Input, 50);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb, ParameterDirection.Input);
                AddParameter("pNumIdDep_in", OracleDbType.Int32, pIntIdDep, ParameterDirection.Input);
                AddParameter("pNumIdOfi_in", OracleDbType.Int32, pIntIdOfi, ParameterDirection.Input);
                if (pBolCambioEstado)
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Char, "S", ParameterDirection.Input, 1);
                    AddParameter("pNumIdEst_in", OracleDbType.Int32, pIntIdEstado, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pChrCambioEst_in", OracleDbType.Char, "N", ParameterDirection.Input, 1);
                    AddParameter("pNumIdEst_in", OracleDbType.Int32, null, ParameterDirection.Input);
                }

                if (pBolEsAutomatico)
                {
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                }
                else
                {
                    AddParameter("pChrEsAutomatico_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                }

                if (pBolEsUrgenteEmision.HasValue)
                {
                    if (pBolEsUrgenteEmision.Value)
                    {
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "1", ParameterDirection.Input, 1);
                    }
                    else
                    {
                        AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, "0", ParameterDirection.Input, 1);
                    }

                }
                else
                {
                    AddParameter("pChrEsUrgenteEmision_in", OracleDbType.Char, null, ParameterDirection.Input, 1);
                }

                if (pDatFecPlazoEmision.HasValue)
                {
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Date, pDatFecPlazoEmision.Value, ParameterDirection.Input);
                }
                else
                {
                    AddParameter("pDatFecPlazoEmision_in", OracleDbType.Date, null, ParameterDirection.Input);
                }

                AddParameter("pNumIdNewPost_out", OracleDbType.Int32, null, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.AW_Insert_Post_Cotizacion);
                valor = GetOutParameter("pNumIdNewPost_out").ToString();
                intIdPost = Convert.ToInt32(valor);
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
                if (GetOutParameter("pNumTipoCambio_out") == null)
                {
                    dblTipoCambio = 0;
                }
                else
                {
                    dblTipoCambio = Convert.ToDouble(GetOutParameter("pNumTipoCambio_out").ToString());
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

        public int Inserta_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost,
            string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb,
            int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta,
            bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea,
            Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile)
        {
            int intIdPost = 0;

            intIdPost = _Insert_Post_Cot(pIntIdCot, pStrTipoPost, pStrTextoPost, pStrIPUsuCrea,
                                    pStrLoginUsuCrea, pIntIdUsuWeb, pIntIdDep, pIntIdOfi, pLstArchivos, pLstFilesPTA, pIntIdEstado,
                                    pBolCambioEstado, pLstFechasCotVta, pBolEsAutomatico, pBytArchivoMail, pBolEsCounterAdmin,
                                    pIntIdUsuWebCounterCrea, pIntIdOfiCounterCrea, pIntIdDepCounterCrea, pBolEsUrgenteEmision,
                                    pDatFecPlazoEmision, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro, pDblMontoEstimadoFile);

            if (pLstArchivos != null)
            {
                foreach (ArchivoPostCot objArchivoPost in pLstArchivos)
                {

                }
            }

            if (pBolCambioEstado)
            {
                Post_SRV RQ_General_PostSRV = new Post_SRV();
                RQ_General_PostSRV.IdCot = pIntIdCot;
                RQ_General_PostSRV.LoginUsuCrea = pStrLoginUsuCrea;
                RQ_General_PostSRV.IPUsuCrea = pStrIPUsuCrea;
                RQ_General_PostSRV.IdEstado = pIntIdEstado;
                if (pBolEsCounterAdmin)
                {
                    RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWebCounterCrea;
                    RQ_General_PostSRV.IdDepCounterCrea = pIntIdDepCounterCrea;
                    RQ_General_PostSRV.IdOfiCounterCrea = pIntIdOfiCounterCrea;
                    RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
                    RQ_General_PostSRV.IdUsuWeb = pIntIdUsuWeb;
                    UpdateEstadoCotVTA(RQ_General_PostSRV);
                }
                else
                {
                    RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWeb;
                    RQ_General_PostSRV.IdDep = pIntIdDep;
                    RQ_General_PostSRV.IdOfi = pIntIdOfi;
                    RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
                    RQ_General_PostSRV.IdUsuWeb = pIntIdUsuWeb;
                    UpdateEstadoCotVTA(RQ_General_PostSRV);

                }
            }

            if (pIntIdEstado == 8 && pIntIdMotivoNoCompro.HasValue) // ESTADO NO COMPRO
            {
                _Update_MotivoNoCompro(pIntIdCot, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro);
            }

            if (pIntIdEstado == 5) //ESTADO FACTURADO
            {
                Update_Importe_FilesPTABy_Cot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);

                if (pLstFilesPTA != null)
                {
                    if (pLstFilesPTA.Count > 0)
                    {
                        bool bolBDNuevoMundo = true;

                        double dblTipoCambio = _Select_TipoCambio(DateTime.Now, "SOL", 1, bolBDNuevoMundo);

                        foreach (FilePTACotVta objFilePTACotVta in pLstFilesPTA)
                        {
                            if (pBolEsCounterAdmin)
                            {
                                _Insert_FilePTA_Cot(objFilePTACotVta.IdCot, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, objFilePTACotVta.Moneda, dblTipoCambio, objFilePTACotVta.ImporteFacturado, pIntIdUsuWebCounterCrea.Value, pIntIdOfiCounterCrea.Value, pIntIdDepCounterCrea.Value);
                            }
                            else
                            {
                                _Insert_FilePTA_Cot(objFilePTACotVta.IdCot, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, objFilePTACotVta.Moneda, dblTipoCambio, objFilePTACotVta.ImporteFacturado, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);
                            }
                        }
                    }
                }
            }
            else if (pIntIdEstado == 4)
            {
                if (pLstFechasCotVta != null)
                {
                    for (int intX = 0; intX <= pLstFechasCotVta.Length - 1; intX++)
                    {
                        if (pBolEsCounterAdmin)
                        {
                            _Insert_FechaSalida_Cot(pIntIdCot, pLstFechasCotVta[intX].ToString(), pIntIdUsuWebCounterCrea.Value, pIntIdDepCounterCrea.Value, pIntIdOfiCounterCrea.Value);

                        }
                        else
                        {

                            _Insert_FechaSalida_Cot(pIntIdCot, pLstFechasCotVta[intX].ToString(), pIntIdUsuWeb, pIntIdDep, pIntIdOfi);

                        }
                    }
                }
            }

            if (pBytArchivoMail != null)//NO ENTRA (SACADO DEL SRV)
            {
                _Insert_ArchivoMail_Post_Cot(pIntIdCot, intIdPost, pBytArchivoMail);
            }

            if (pDblMontoEstimadoFile.HasValue && pDblMontoEstimadoFile.Value > 0)
            {
                _Update_MontoEstimadoFileBy_IdCotVta(pIntIdCot, pDblMontoEstimadoFile.Value);
            }

            return intIdPost;
        }

        private void Update_Importe_FilesPTABy_Cot(int pIntIdCot, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep)
        {
            try
            {
                List<FilePTACotVta> lstFiles = _SelectFilesPTABy_IdCot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);

                foreach (FilePTACotVta objTempFilePTA in lstFiles)
                {
                    _Update_Importe_FilePTA(pIntIdCot, objTempFilePTA.IdSuc, objTempFilePTA.IdFilePTA, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, "1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void _Update_Importe_FilePTA(int pIntIdCot, int pIntIdSuc, int pIntIdFile, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep, string pStrEsUpdUsuario)
        {
            try
            {
                DataTable dtImporteFile = _Select_InfoFile(pIntIdSuc, pIntIdFile);

                if (dtImporteFile.Rows.Count == 0)
                {
                    ///Ssegun el srv no hace nada
                }
                else
                {
                    DataRow drCliente = dtImporteFile.Rows[0];
                    string strIdMoneda = drCliente["ID_MONEDA"].ToString();
                    double dblImporteSuma = 0;

                    foreach (DataRow drImporteFile in dtImporteFile.Rows)
                    {
                        if ((drImporteFile["ID_MONEDA"]) != null)
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                                dblImporteSuma += (double)drImporteFile["IMPORTE_TOTAL"];
                        }
                    }

                    _Actualiza_Imp_File_Cot(pIntIdCot, pIntIdSuc, pIntIdFile, strIdMoneda,
                                                           dblImporteSuma, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, pStrEsUpdUsuario);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Auxiliares
        private CotizacionVta FillDtsCotizacion(DataTable dt = null)
        {
            try
            {
                CotizacionVta cotizacionVta = new CotizacionVta();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Loading
                        if(row != null)
                        {
                            cotizacionVta.IdCot = Convert.ToInt32(row["COT_ID"]);
                            cotizacionVta.FechaCot = (DateTime)row["COT_FECHA"];
                            cotizacionVta.IdModoIng = Convert.ToInt16(row["MODING_ID"]);
                            cotizacionVta.NomModoIngreso = Convert.ToString(row["MODING_NOM"]);
                            cotizacionVta.IdCanalVta = Convert.ToInt16(row["CANV_ID"]);
                            cotizacionVta.NomCanalVta = Convert.ToString(row["CANV_NOM"]);                 
                            cotizacionVta.TextoSolicitud = (row["COT_TEXT_SOL_CLI"] == null ? string.Empty : Convert.ToString(row["COT_TEXT_SOL_CLI"]));
                            cotizacionVta.NomCompletoUsuCrea = Convert.ToString(row["COT_NOMC_USU_CREA"]);
                            cotizacionVta.LoginUsuWeb = Convert.ToString(row["COT_LOGIN_USU_CREA"]);
                            cotizacionVta.IdEstado = Convert.ToInt16(row["EST_ID"]);
                            cotizacionVta.NomEstadoCot = Convert.ToString(row["EST_NOM"]);
                            cotizacionVta.IdCliCot = Convert.ToInt32(row["CLICOT_ID"]);
                            cotizacionVta.IdUsuWeb = Convert.ToInt32(row["USUWEB_ID_CREA"]);
                            cotizacionVta.IdOfi = Convert.ToInt32(row["OFI_ID_USU_CREA"]);
                            cotizacionVta.IdDep = Convert.ToInt32(row["DEP_ID_USU_CREA"]);
                            cotizacionVta.NomOfi = Convert.ToString(row["OFI_NOM"]);
                            cotizacionVta.NomDep = Convert.ToString(row["DEP_NOM"]);
                            cotizacionVta.IdWeb = Convert.ToInt32(row["WEBS_CID"]);
                            cotizacionVta.IdLang = Convert.ToInt32(row["LANG_CID"]);
                            cotizacionVta.CantVecesFact = Convert.ToInt16(row["CANT_FACT"]);
                            cotizacionVta.CodigoIATAPrincipal = (row["COT_IATA_PRINCIPAL"] == null ? string.Empty : Convert.ToString(row["COT_IATA_PRINCIPAL"]));                        
                            if (row["EMPCOT_ID"] == null)
                            {
                                cotizacionVta.IdEmpCot = null;
                                cotizacionVta.RazSocEmpCot = string.Empty;
                            }
                            else
                            {
                                //cotizacionVta.IdEmpCot = Convert.ToInt32(row["EMPCOT_ID"]);
                                cotizacionVta.RazSocEmpCot = Convert.ToString(row["EMPCOT_RAZ_SOC"]);
                            }
                            if (row["COT_DESTINOS_PREF"] != null)
                                cotizacionVta.DestinosPref = Convert.ToString(row["COT_DESTINOS_PREF"]);
                            //if (row["COT_FEC_SAL"] != null)
                                //cotizacionVta.FecSalida = (DateTime)row["COT_FEC_SAL"];
                           // if (row["COT_FEC_REG"] != null)
                                //cotizacionVta.FecRegreso = (DateTime)row["COT_FEC_REG"];
                            //if (row["COT_CANT_ADT"] != null)
                            //    cotizacionVta.CantPaxAdulto = Convert.ToInt16(row["COT_CANT_ADT"]);
                            //if (row["COT_CANT_CHD"] != null)
                            //{
                            //    cotizacionVta.CantPaxNiños = Convert.ToInt16(row["COT_CANT_CHD"]);
                            //}                                
                            //cotizacionVta.EmailUsuWebCrea = Convert.ToString(row["PER_EMAIL"]);
                            //if (row["VUE_RESERVA_ID"] != null)
                            //    cotizacionVta.IdReservaVuelos = Convert.ToInt32(row["VUE_RESERVA_ID"]);
                            //if (row["PAQ_RESERVA_SUC"] != null)
                            //    cotizacionVta.IdSucursalReservaPaquete = Convert.ToInt16(row["PAQ_RESERVA_SUC"]);
                            //if (row["PAQ_RESERVA_ID"] != null)
                            //    cotizacionVta.IdReservaPaquete = Convert.ToInt32(row["PAQ_RESERVA_ID"]);
                            //if (row["PAQ_RESERVA_TIPO"] != null)
                            //    cotizacionVta.TipoPaquete = Convert.ToString(row["PAQ_RESERVA_TIPO"]);
                            //if (row["COT_FIRMA_CLI"] != null)
                            //{
                            //    if (Convert.ToString(row["COT_FIRMA_CLI"]) == "1")
                            //        cotizacionVta.RequiereFirmaCliente = true;
                            //    else
                            //        cotizacionVta.RequiereFirmaCliente = false;
                            //}
                            //if (row["RES_VUE_PNR_MANUAL"] != null)
                            //    cotizacionVta.CodReservaVueManual = Convert.ToString(row["RES_VUE_PNR_MANUAL"]);
                            //if (row["RES_VUE_MONTO_MANUAL"] != null)
                            //    cotizacionVta.MontoReservaVueManual = Convert.ToDouble(row["RES_VUE_MONTO_MANUAL"]);
                            //if (row["VEND_ID"] != null)
                            //    cotizacionVta.IdVendedorPTACrea = Convert.ToString(row["VEND_ID"]);
                            //if (row["ID_MOD_COMPRA"] != null)
                            //    cotizacionVta.IdModalidadCompra = Convert.ToInt16(row["ID_MOD_COMPRA"]);
                            //if (row["ES_URGENTE"] != null)
                            //{
                            //    if (row["ES_URGENTE"].ToString() == "1")
                            //        cotizacionVta.EsUrgenteEmision = true;
                            //    else
                            //        cotizacionVta.EsUrgenteEmision = false;
                            //}
                            //if (row["FECHA_PLAZO_EMISION"] != null)
                            //    cotizacionVta.FechaPlazoEmision = (DateTime)row["FECHA_PLAZO_EMISION"];
                            //if (row["USUWEB_ID_CA"] != null)
                            //    cotizacionVta.IdUsuWebCA = Convert.ToInt32(row["USUWEB_ID_CA"]);
                            //if (row["USUWEB_LOGIN_CA"] != null)
                            //    cotizacionVta.LoginUsuWebCA = Convert.ToString(row["USUWEB_LOGIN_CA"]);
                            //if (row["ES_EMITIDO"] != null)
                            //{
                            //    if (row["ES_EMITIDO"].ToString() == "1")
                            //        cotizacionVta.EsEmitido = true;
                            //}
                            //if (row["COMPRA_ID"] != null)
                            //    cotizacionVta.IdCompra = Convert.ToInt32(row["COMPRA_ID"]);
                            //if (row["AUTO_RES_ID"] != null)
                            //    cotizacionVta.IdReservaAuto = Convert.ToInt32(row["AUTO_RES_ID"]);
                            //if (row["SEGURO_RES_ID"] != null)
                            //    cotizacionVta.IdReservaSeguro = Convert.ToInt32(row["SEGURO_RES_ID"]);
                            //if (row["NOM_GRUPO"] != null)
                            //    cotizacionVta.NomGrupo = Convert.ToString(row["NOM_GRUPO"]);
                            //if (row["MONTO_ESTIMADO_FILE"] != null)
                            //    cotizacionVta.MontoEstimadoFile = Convert.ToDouble(row["MONTO_ESTIMADO_FILE"]);

                            //if (row["HOTEL_RES_ID"] != null)
                            //    cotizacionVta.IdReservaHotel = Convert.ToInt32(row["HOTEL_RES_ID"]);

                            //if (row["ES_AEREO"] != null)
                            //    cotizacionVta.EsAereo = Convert.ToInt32(row["ES_AEREO"]);
                            //if (row["ID_OATENCION"] != null)
                            //    cotizacionVta.IdOAtencion = Convert.ToInt32(row["ID_OATENCION"]);
                            //if (row["ID_EVENTO"] != null)
                            //    cotizacionVta.IdEvento = Convert.ToInt32(row["ID_EVENTO"]);
                            //if (row["IDUSERLOGIN"] != null)
                            //{
                            //    if (row["CLICOT_EMAIL"] != null)
                            //        cotizacionVta.CliCod_Mail = Convert.ToString(row["CLICOT_EMAIL"]);
                            //}
                            //if (row["ID_RESERVAMT"] != null)
                            //    cotizacionVta.IdReserva2MT = Convert.ToInt32(row["ID_RESERVAMT"]);                           
                            //if (row["ES_PAQ_DINAMIC"] != null)
                            //    cotizacionVta.EsPaqDinamico = Convert.ToString(row["ES_PAQ_DINAMIC"]);
                            //if (row["HOTEL_RES_ID"] != null)
                            //    cotizacionVta.HotelResId = Convert.ToInt32(row["HOTEL_RES_ID"]);
                            //if (row["METABUSCADOR"] != null)
                            //    cotizacionVta.Metabuscador = Convert.ToString(row["METABUSCADOR"]);
                        }
                        #endregion                             
                    }
                }
                return cotizacionVta;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}