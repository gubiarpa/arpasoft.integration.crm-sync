using System;
using System.Collections.Generic;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System.Data;
using System.Data.OracleClient;

namespace Expertia.Estructura.Repository.Behavior
{
    interface ICotizacionSRV_Repository
    {
        int ProcesosPostCotizacion(Post_SRV RQ_General_PostSRV);
        //int _Insert_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost, string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, short pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta, bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, int? pIntIdUsuWebCounterCrea, int? pIntIdOfiCounterCrea, int? pIntIdDepCounterCrea, bool? pBolEsUrgenteEmision, DateTime? pDatFecPlazoEmision, short? pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, double? pDblMontoEstimadoFile, OracleTransaction pObjTx);
        bool _Liberar_UsuWeb_CA(int pIntIdCot);
        void _UpdateEstadoCotVTA(Post_SRV RQ_General_PostSRV);
        //void _Update_Estado_Cot_VTA(int pIntIdCot, string pStrLoginUsuCrea, string pStrIPUsuCrea, Int16 pIntIdEstado, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, bool pBolEsAutomatico, Nullable<int> pIntIdUsuWebCounterAdmin, OracleTransaction pObjTx);
        //void _Update_MotivoNoCompro(int pIntIdCot, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, OracleTransaction pObjTx);
        List<FilePTACotVta> _SelectFilesPTABy_IdCot(int pIntIdCot, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep);
        DataTable _Select_InfoFile(int pIntIdSuc, int pIntIdFile);
        void _Actualiza_Imp_File_Cot(int pIntIdCot, int pIntIdSuc, int pIntIdFile, string strIdMoneda,double dblImporteSuma, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep, string pStrEsUpdUsuario);
        double _Select_TipoCambio(DateTime pDatFecha, string pStrMoneda, Int16 pIntIdEmp, bool pBolEsBDNuevoMundo);
        //void _Insert_FilePTA_Cot(int IdCot, int IdSuc, int IdFilePTA, string Moneda, double dblTipoCambio, double ImporteFacturado, int IdUsuWebCounterCrea, int IdOfiCounterCrea, int IdDepCounterCrea, OracleTransaction pObjTx);
        //void _Insert_ArchivoMail_Post_Cot(int pIntIdCot, int intIdPost, string pBytArchivoMail, OracleTransaction pObjTx);
        //void _Update_MontoEstimadoFileBy_IdCotVta(int pIntIdCot, double pDblMontoEstimadoFile, OracleTransaction pObjTx);
        //void _Insert_FechaSalida_Cot(int pIntIdCot, string pStrFecSalida, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, OracleTransaction pObjTx);
        CotizacionVta Get_Datos_CotizacionVta(int IdCotSRV);
        int Inserta_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost,
            string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb,
            int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta,
            bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea,
            Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile,int OpcionalInsertPost = 1);

        void _Update_ModalidadCompra(int pIntIdCot, Int16 pIntIdModalidadCompra);        
        void _Update_EsEmitido(int pIntIdCot, bool pBolEsEmitido);
        bool _Update_CounterAdministrativo(int pIntIdCot, Nullable<int> pIntIdUsuWebCA);
        void _Update_Requiere_FirmaCliente_Cot(int pIntIdCot, bool pBolRequiereFirma);
    }
}
