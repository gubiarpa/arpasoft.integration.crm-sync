using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System.Collections.Generic;
using System;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IEnviarCorreo
    {
        bool Enviar_SolicitudPagoServicioSafetyPay(string IdUsuario, int pIntIdWeb, int pIntIdLang, int pIntIdCotVta, string pStrEmailTO, string pStrEMailCC, string pStrNomCli, string pStrApeCli, string pStrURLPago, string pStrNomCompletoUsuWeb, string pStrEmailUsuWeb, Int16 pIntIdFormaPago, string pStrIdTransaction, int pIntIdPedido, double pDblMontoPagar, string pStrFechaExpiraPago, List<AmountType> pLstAmountSafetyPay, List<PaymentLocationType> pLstPaymentLocationSafetyPay);
    }
}
