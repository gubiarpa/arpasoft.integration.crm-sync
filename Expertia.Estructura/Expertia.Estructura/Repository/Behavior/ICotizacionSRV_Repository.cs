using System;
using System.Collections.Generic;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System.Data;

namespace Expertia.Estructura.Repository.Behavior
{
    interface ICotizacionSRV_Repository
    {
        int ProcesosPostCotizacion(Post_SRV RQ_General_PostSRV);
        int _Insert_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost,
            string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb,
            int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta,
            bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea,
            Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile);
        bool _Liberar_UsuWeb_CA(int pIntIdCot);
        void UpdateEstadoCotVTA(Post_SRV RQ_General_PostSRV);
        void _Update_MotivoNoCompro(int pIntIdCot, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro);
        List<FilePTACotVta> _SelectFilesPTABy_IdCot(int pIntIdCot, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep);

        DataTable _Select_InfoFile(int pIntIdSuc, int pIntIdFile);

        void _Actualiza_Imp_File_Cot(int pIntIdCot, int pIntIdSuc, int pIntIdFile, string strIdMoneda,double dblImporteSuma, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep, string pStrEsUpdUsuario);

        double _Select_TipoCambio(DateTime pDatFecha, string pStrMoneda, Int16 pIntIdEmp, bool pBolEsBDNuevoMundo);
        void _Insert_FilePTA_Cot(int IdCot, int IdSuc, int IdFilePTA, string Moneda, double dblTipoCambio, double ImporteFacturado, int IdUsuWebCounterCrea, int IdOfiCounterCrea, int IdDepCounterCrea);
        void _Insert_ArchivoMail_Post_Cot(int pIntIdCot, int intIdPost, string pBytArchivoMail);
        void _Update_MontoEstimadoFileBy_IdCotVta(int pIntIdCot, double pDblMontoEstimadoFile);
        void _Insert_FechaSalida_Cot(int pIntIdCot, string pStrFecSalida, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi);

        CotizacionVta Get_Datos_CotizacionVta(int IdCotSRV);

        int Inserta_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost,
            string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb,
            int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta,
            bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea,
            Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile);
    }
}
