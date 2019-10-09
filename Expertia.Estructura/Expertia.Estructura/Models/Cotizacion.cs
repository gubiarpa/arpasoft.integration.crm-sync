using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class Cotizacion : IUnidadNegocio, IAuditable
    {
        #region Properties
        public string IdCuentaSalesforce { get; set; }
        public string IdOportunidadSalesforce { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRetorno { get; set; }
        #endregion

        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }
        public SimpleDesc Grupo { get; set; }
        public SimpleDesc Origen { get; set; }
        public SimpleDesc Ciudad { get; set; }
        public SimpleDesc VendedorCounter { get; set; }
        public SimpleDesc VendedorCotizador { get; set; }
        #endregion

        #region MultipleKey
        #endregion

        #region Audit
        public Auditoria Auditoria { get; set; }
        #endregion

        /*
            [x]  P_CODIGO_ERROR                 → 
            [x]  P_MENSAJE_ERROR                → 
            [ ]  P_NOMBRE_USUARIO               → Auditoria
            [ ]  P_ID_CUENTA_SALESFORCE         → IdCuentaSalesForce
            [ ]  P_ID_OPORTUNIDAD_SALESFORCE    → IdOportunidadSalesforce
            [ ]  P_NOMBRE_GRUPO                 → Grupo
            [ ]  P_FECHA_SALIDA                 → FechaSalida
            [ ]  P_FECHA_RETORNO                → FechaRetorno
            [ ]  P_NOMBRE_ORIGEN                → Origen
            [ ]  P_NOMBRE_CIUDAD                → Ciudad
            [ ]  P_NOMBRE_VENDEDOR_COUNTER      → Vendedor
            [ ]  P_NOMBRE_VENDEDOR_COTIZADOR    → VendedorCotiza
            [x]  P_ID_CUENTA                    → 
            [x]  P_ID_COTIZACION                → 
         */

    }
}