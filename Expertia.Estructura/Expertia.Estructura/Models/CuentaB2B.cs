namespace Expertia.Estructura.Models
{
    public class CuentaB2B : Cuenta
    {
        #region Properties
        public string RazonSocial { get; set; } // Razón Social
        public string Alias { get; set; } // Alias
        public decimal? MontoLineaCredito { get; set; } // Monto de Línea de Crédito        
        #endregion

        #region ForeignKey
        public CondicionPago CondicionPago { get; set; } // Condición de Pago
        public TipoMoneda TipoMonedaDeLineaCredito { get; set; } // Tipo de moneda de Linea de credito
        #endregion
    }
}