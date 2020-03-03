using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IPedidoRepository
    {
        Operation Create(Pedido pedido);
        Operation GetPedidosProcesados();
        RptaPagoSafetyPay Get_Rpta_SagetyPay(int IdPedido);
        void InsertFormaPagoPedido(Pedido pedidoRQ, PedidoRS pedidoRS);
        void Update_FechaExpira_Pedido(Pedido pedidoRQ, PedidoRS pedidoRS);
        void Update_Pedido_Process(PedidosProcesados PedidosProccess);
        void Update_Pedido_SolicitudPago_SF(int idPedido, int idSrv, string IdOportunidad_SF, string IdSolicitudpago_SF);
        List<PasarelaPago_Pedido> Get_Pedido_XSolicitud(Nullable<int> pIntIdResVue, Nullable<int> pIntIdResPaq, string pStrTipoPaq, Nullable<int> pIntIdWeb, Nullable<int> pIntIdCotSRV, Nullable<int> pIntIdResAuto, Nullable<int> pIntIdResSeguro);
        FormaPagoPedido Get_FormaPagoBy_IdPedido(int IdPedido);
        List<AmountType> Get_Monedas_PedidoSafetyPay(int IdPedido);
    }
}
