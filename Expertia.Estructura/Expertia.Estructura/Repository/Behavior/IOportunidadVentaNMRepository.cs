using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IOportunidadVentaNMRepository
    {
        Operation SelectByCodeSF(string strCodeSF);
        void _Update(int pIntIdCliCot, string pStrNomCliCot, string pStrApeCliCot, string pStrApeMatCliCot, string pStrEmailCliCot, string pStrEmailAlterCliCot, List<TelefonoCliCot> pLstTelefonos, bool pBolRecibePromo, string pStrDirecCliCot, string pStrNumDocCliCot, string pStrIdTipDoc, int pIntIdUsuWeb, int pIntIdWeb, List<ArchivoCliCot> pLstArchivos, Nullable<DateTime> pDatFecNac);
        bool _Update_Estado_Promociones(int pNumClicot_Id_in, string pEnviaPromo_in);
        int Inserta_Cot_Vta(short pIntModoIng, string pStrTextoSol, string pStrNomUsuCrea, string pStrLoginUsuCrea,
            string pStrIPUsuCrea, int pIntIdCliCotVta, int pIntIdUsuWeb, int pIntIdDep, int pIntIdOfi, int pIntIdWeb, int pIntIdLang,
            short pIntIdCanalVta, string[] pStrArrayServicios, string pStrCodIATAPrinc, int? pIntIdEmpCot, short pIntIdEstOtro,
            string pStrDestinosPref, DateTime? pDatFecSalida, DateTime? pDatFecRegreso, short? pIntCantPaxAdulto, short? pIntCantPaxNino,
            string pStrPaisResidencia, int? pIntIdReservaVuelos, short? pIntIdSucursalPaq, int? pIntIdWebPaq, int? pintIdCotResPaq,
            string pStrTipoPaq, int? pintIdReservaAuto, int? pintIdReservaSeguro, int? pintIdReservaHotel, string pStrNomGrupo,
            decimal? pNumMontoDscto, int pIntIdOAtencion = 0, int pIntIdEvento = 0);
        void RegistraCuenta(string idCuentaSF, int idCuentaNM);
        bool _EsCounterAdministratiivo(int pIntIdOfi);
        void _Update_DatosReservaVuelo_Manual_Cot(int pIntIdCot, string pStrCodReserva, Int16 pIntIdMoneda, double pDblMontoVta);
        List<int> _Select_Pedidos_SinBancoBy_IdCot(int pIntIdCot);
        int _Select_CotId_X_OportunidadSF(string _oportunidadSF);
        void RegistraOportunidad(string idOportunidadSF, int idCotizacionNM);
    }
}
