using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IPedidoRepository
    {
        Operation Create(Pedido pedido);
        RptaPagoSafetyPay Get_Rpta_SagetyPay(int IdPedido);
        void InsertFormaPagoPedido(Pedido pedidoRQ, PedidoRS pedidoRS);
        void Update_FechaExpira_Pedido(Pedido pedidoRQ, PedidoRS pedidoRS);
    }
}
