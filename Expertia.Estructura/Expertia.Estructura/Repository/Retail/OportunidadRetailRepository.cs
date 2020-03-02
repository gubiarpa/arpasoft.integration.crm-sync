using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Retail
{
    public class OportunidadRetailRepository : OracleBase<OportunidadRetailReq>
    {
        public OportunidadRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        #region PublicMethods
        public Operation ObtienePersonalXId_TrustWeb(
            int pIntIdUsuWeb)
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int64, pIntIdUsuWeb);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_PERSONAL.SP_PERS_OBTIENE_TRUSTWEB");

                operation["pCurResult_out"] = ToPersonal(GetDtParameter("pCurResult_out"));

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation ObtieneUsuarioWebXId(
            int pIntIdUsuWeb)
        {
            try
            {
                var operation = new Operation();

                AddParameter("pNumIdUsuWeb", OracleDbType.Int64, pIntIdUsuWeb);
                AddParameter("pCurResult_out", OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_USUARIO_WEB.SP_UW_OBTIENE_X_ID");

                operation["pCurResult_out"] = ToUsuarioWeb(GetDtParameter("pCurResult_out"));

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Operation SelectByDocumento(
            string pStrTipoDoc,
            string pStrNumDoc)
        {
            return null;
        }

        public Operation SelectByEmail(
            string strCorreo)
        {
            return null;
        }

        public Operation InsertaClienteCotizacion(
            string pStrNomCliCot,
            string pStrApePatCliCot,
            string pStrApeMatCliCot,
            string pStrEmailCliCot,
            string pStrEmailAlterCliCot,
            List<TelefonoCliCot> pLstTelefonos,
            bool pBolRecibePromo,
            string pStrDirecCliCot,
            string pStrNumDocCliCot,
            string pStrIdTipDoc,
            int pIntIdUsuWeb,
            int pIntIdWeb,
            List<ArchivoCliCot> pLstArchivos,
            bool pBolEsAdicional,
            int? pIntIdCot,
            DateTime? pDatFecNac)
        {
            try
            {
                var operation = new Operation();

                AddParameter("pVarNomCliCot_in", OracleDbType.Varchar2, pStrNomCliCot);
                AddParameter("pVarApeCliCot_in", OracleDbType.Varchar2, pStrApePatCliCot);
                AddParameter("pVarApeMatCliCot_in", OracleDbType.Varchar2, pStrApeMatCliCot);
                AddParameter("pVarEmailCliCot_in", OracleDbType.Varchar2, pStrEmailCliCot);
                AddParameter("pVarEmailAlterCliCot_in", OracleDbType.Varchar2, pStrEmailAlterCliCot);
                AddParameter("pChrRecibePromo_in", OracleDbType.Varchar2, pBolRecibePromo ? "1" : "0");
                AddParameter("pVarDirecCliCot_in", OracleDbType.Varchar2, pStrDirecCliCot);
                AddParameter("pChrIdTipDoc_in", OracleDbType.Varchar2, pStrIdTipDoc.Trim().Equals("-1") ? null : pStrIdTipDoc);
                AddParameter("pVarNumDocCliCot_in", OracleDbType.Varchar2, pStrNumDocCliCot);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Varchar2, pIntIdUsuWeb);
                AddParameter("pNumIdWeb_in", OracleDbType.Varchar2, pIntIdWeb);
                AddParameter("pDatFecNacCli_in", OracleDbType.Date, pDatFecNac.HasValue ? pDatFecNac : null);
                AddParameter("pNumIdNewCliCot_out", OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_CLI");

                operation["pNumIdNewCliCot_out"] = GetOutParameter("pNumIdNewCliCot_out");

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaTelefClienteCotizacion(
            int pIntIdCliCot,
            string pStrNumTelf,
            string pStrAnexoTelf,
            bool pBolEsTelfPrinc,
            int pIntIdTipoTelf,
            Int16? pIntCodPais,
            Int16? pIntCodArea)
        {
            try
            {
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot);
                AddParameter("pVarNumTelf_in", OracleDbType.Varchar2, pStrNumTelf);
                AddParameter("pVarNumAnexoTelf_in", OracleDbType.Varchar2, pStrAnexoTelf);
                AddParameter("pChrTelfPrinc_in", OracleDbType.Char, pBolEsTelfPrinc ? "1" : "0");
                AddParameter("pNumIdTipoTelf_in", OracleDbType.Int16, pIntIdTipoTelf);
                AddParameter("pNumCodPaisTelf_in", OracleDbType.Int16, pIntCodPais);
                AddParameter("pNumCodAreaTelf_in", OracleDbType.Int16, pIntCodArea);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_TELF_CLI");

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation InsertaArchivoClienteCotizacion(
            int pIntIdCliCot,
            string pStrRutaServArchivo,
            string pStrRutaURLArchivo,
            string pStrNomArchivo,
            string pStrExtArchivo,
            List<Byte> pBytArchivo,
            int pIntIdUsuWeb)
        {
            try
            {
                AddParameter("pNumIdCliCot_in", OracleDbType.Int32, pIntIdCliCot);
                AddParameter("pVarRutaServArchivo_in", OracleDbType.Varchar2, pStrRutaServArchivo);
                AddParameter("pVarRutaURLArchivo_in", OracleDbType.Varchar2, pStrRutaURLArchivo);
                AddParameter("pVarNomArchivo_in", OracleDbType.Varchar2, pStrNomArchivo);
                AddParameter("pVarExtArchivo_in", OracleDbType.Char, pStrExtArchivo);
                AddParameter("pBlbArchivo_in", OracleDbType.Blob, pBytArchivo);
                AddParameter("pNumIdUsuWeb_in", OracleDbType.Int32, pIntIdUsuWeb);

                ExecuteStoredProcedure("APPWEBS.PKG_CLIENTE_COT.SP_INSERTA_ARCHIVO_CLI");
                
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Operation ActualizaClienteCotizacion(
            int pIntIdCliCot,
            string pStrNomCliCot,
            string pStrApeCliCot,
            string pStrApeMatCliCot,
            string pStrEmailCliCot,
            int pIntIdUsuWeb)
        {
            return null;
        }

        public Operation InsertaCotizacionVenta(
            Int16 pIntModoIng,
            string pStrTextoSol,
            string pStrNomUsuCrea,
            string pStrLoginUsuCrea,
            string pStrIPUsuCrea,
            int pIntIdCliCotVta,
            int pIntIdUsuWeb,
            int pIntIdDep,
            int pIntIdOfi,
            int pIntIdWeb,
            int pIntIdLang,
            Int16 pIntIdCanalVta,
            string[] pStrArrayServicios,
            string pStrCodIATAPrinc,
            int? pIntIdEmpCot,
            Int16 pIntIdEstOtro,
            string pStrDestinosPref,
            DateTime? pDatFecSalida,
            DateTime? pDatFecRegreso,
            Int16? pIntCantPaxAdulto,
            Int16? pIntCantPaxNino,
            string pStrPaisResidencia,
            int? pIntIdReservaVuelos,
            short? pIntIdSucursalPaq,
            int? pIntIdWebPaq,
            int? pintIdCotResPaq,
            string pStrTipoPaq,
            int? pintIdReservaAuto,
            int? pintIdReservaSeguro,
            int? pintIdReservaHotel,
            string pStrNomGrupo,
            decimal? pNumMontoDscto,
            int pIntIdOAtencion = 0,
            int pIntIdEvento = 0)
        {
            return null;
        }

        public Operation InsertaIngresoCliente(
            DateTime pDatFecRegistro,
            string pStrNomCli,
            string pStrApeCli,
            string pStrApeMatCli,
            string pStrEmailCli,
            short pIntIdMotivo,
            string pStrOtroMotivo,
            string pStrDestino,
            int pIntIdUsuWebCrea,
            int pIntIdOfiUsuWebCrea,
            int pIntIdDepUsuWebCrea,
            DateTime? pDatFecAtiende,
            int? pIntIdUsuWebAtiende,
            int? pIntIdOfiUsuWebAtiende,
            int? pIntIdDepUsuWebAtiende,
            string pStrComentarios,
            string pStrIP,
            long? pIntIdCotSRV,
            int? pIntIdCliCot,
            string pStrIdTipoDoc,
            string pStrNumDoc,
            DateTime? pDatFecSalida)
        {
            return null;
        }

        public bool ValidarEnvioPromociones(
            int pIntIdOcurrencia,
            string pEnviaPromo_in)
        {
            return true;
        }

        public void EnviarPromociones(
            Personal objPersonal,
            OportunidadRetailReq oportunidadRetailReq,
            bool chEnviarPromociones,
            string strNomCli,
            string strApeCli,
            string strEmailCli
            )
        {
            /*
            UsuarioSession objUsuarioSession = (UsuarioSession)Session(NMConstantesUtility.SES_OBJ_USUARIO);
            NuevoMundoBusiness.SuscripcionBoletinBO objSuscriptBoletinBO = new NuevoMundoBusiness.SuscripcionBoletinBO();
            NMWebUtility objNMWebUtility = new NMWebUtility();
            int intIdOfi = objPersonal.IdOficina; // objUsuarioSession.IdOfi;
            int intIdDep = objPersonal.IdDepartamento;  // objUsuarioSession.IdDep;
            try
            {
                if (Trim(hdRecibeProm.Value) == "0" & chEnviarPromociones)
                {
                    if (intIdDep != NMConstantesUtility.INT_ID_DEP_INTERNO || intIdDep != NMConstantesUtility.INT_ID_DEP_RECEPTIVO)
                        objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_NMV, true);

                    if ((intIdOfi == NMConstantesUtility.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER) | (intIdOfi == NMConstantesUtility.INT_ID_OFI_NMV & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER))
                        objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);
                }
                else if (Trim(hdRecibeProm.Value) == "1" & !chEnviarPromociones)
                {
                    // Estaba suscrito y ahora ya no lo va a estar
                    objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_NMV, false);

                    if ((intIdOfi == NMConstantesUtility.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER) | (intIdOfi == NMConstantesUtility.INT_ID_OFI_NMV & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER))
                        objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, false);
                }
                else if (Trim(hdRecibeProm.Value) == "" & chEnviarPromociones)
                {
                    if (intIdDep != NMConstantesUtility.INT_ID_DEP_INTERNO || intIdDep != NMConstantesUtility.INT_ID_DEP_RECEPTIVO)
                        objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_NMV, true);

                    if ((intIdOfi == NMConstantesUtility.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER) | (intIdOfi == NMConstantesUtility.INT_ID_OFI_NMV & intIdDep == NMConstantesUtility.INT_ID_DEP_COUNTER))
                        objSuscriptBoletinBO.Mail_AgregaEmailListaBoletinNMV(7, 1, objNMWebUtility.ReemplazarTildesVocales(strNomCli + " " + strApeCli), strEmailCli, ConstantesWeb.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);
                }
            }
            catch (Exception ex)
            {
            }
            */
        }
        #endregion

        #region Parse
        private IEnumerable<Personal> ToPersonal(DataTable dt)
        {
            try
            {
                var personalList = new List<Personal>();

                foreach (DataRow row in dt.Rows)
                {
                    personalList.Add(new Personal()
                    {
                        IdUsuWeb = row.IntParse("USUWEB_ID"),
                        LoginPer = row.StringParse("USUWEB_LOGIN"),
                        ApePatPer = row.StringParse("PER_APEPAT"),
                        ApeMatPer = row.StringParse("PER_APEMAT"),
                        NomPer = row.StringParse("PER_NOM"),
                        NomCompletoPer = string.Format("{0} {1} {2}", row.StringParse("PER_NOM"), row.StringParse("PER_APEPAT"), row.StringParse("PER_APEMAT")),
                        DirecPer = row.StringParse("PER_DIREC"),
                        CodPostalPer = row.StringParse("PER_COD_POS"),
                        EmailPer = row.StringParse("PER_EMAIL"),
                        TelfCasaPer = row.StringParse("PER_TELF_CASA"),
                        TelfDirectoPer = row.StringParse("PER_TELF_DIR"),
                        TelfMovilPer = row.StringParse("PER_TELF_MOV"),
                        TelfMovilEmpresaPer = row.StringParse("PER_TELF_MOV_EMP"),
                        TelfNextelPer = row.StringParse("PER_TELF_NEXTEL"),
                        TelfOficinaPer = row.StringParse("PER_TELF_OFI"),
                        AnexoPer = row.StringParse("PER_ANEXO"),
                        NumDocPer = row.StringParse("PER_NRO_DOC"),
                        FechaNacPer = row.StringParse("PER_FEC_NAC"),
                        EstadoPer = row.IntParse("PER_ESTADO"),
                        IdDoc = row.StringParse("DOC_CID"),
                        IdPais = row.StringParse("PAIS_ID"),
                        IdCiudad = row.StringParse("CIU_ID"),
                        IdDistrito = row.IntNullParse("DIS_ID") ?? -1,
                        IdOficina = row.IntParse("OFI_ID"),
                        IdDepartamento = row.IntParse("DEP_ID"),
                        IdEmpresa = row.IntParse("EMP_ID"),
                        IdUsuWebSybase = row.IntNullParse("USUWEB_ID_SYBASE") ?? 0,
                        IdCargo = row.IntNullParse("CAR_ID") ?? 0,
                        EsSupervisorSRV = row.StringParse("PER_ES_SUPERVISOR").Equals("1"),
                        VarDespegar = row.StringParse("PER_VARDESPEGUE").Equals("1"),
                        VarImgDespegar = row.StringParse("PER_VARIMGDESPEGUE").Equals("1"),
                        PCCPer = row.StringParse("PER_PCC"),
                        IdEmpresaPlanilla = row.IntNullParse("ID_EMPRESA_PLANILLA"),
                        IdPlanilla = row.IntNullParse("ID_CODIGO_PLANILLA"),
                        AutoLoginTrp = row.StringNullParse("AUTOLOGIN_TRP"),
                        EsCounterAdmin = row.StringParse("ES_COUNTER_ADMIN").Equals("1"),
                        TieneRpteMetas = row.StringParse("PER_TIENE_RP_METAS").Equals("1")
                    });
                }

                return personalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<UsuarioWeb> ToUsuarioWeb(DataTable dt)
        {
            try
            {
                var usuarioWebList = new List<UsuarioWeb>();

                foreach (DataRow row in dt.Rows)
                {
                    usuarioWebList.Add(new UsuarioWeb()
                    {
                        IdUsuWeb = row.IntParse("USUWEB_ID"),
                        LoginUsuWeb = row.StringParse("USUWEB_LOGIN"),
                        TipoUsuario = row.StringParse("USUWEB_TIPO").Equals("P") ? UsuarioWeb.TIPO_USUARIO.PERSONAL : UsuarioWeb.TIPO_USUARIO.CLIENTE,
                        IdUsuWebSybase = row.IntParse("USUWEB_ID_SYBASE")
                    });
                }

                return usuarioWebList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}