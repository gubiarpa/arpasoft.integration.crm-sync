using System;
using System.Collections.Generic;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

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

    }
}
