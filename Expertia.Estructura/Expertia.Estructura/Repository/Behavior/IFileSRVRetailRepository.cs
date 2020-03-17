using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFileSRVRetailRepository
    {
        Operation GetFilesAsociadosSRV();
        Operation Actualizar_EnvioCotRetail(FilesAsociadosSRVResponse FileAsociadosSRVResponse);
        ArrayList Inserta_ReciboCaja(List<Models.FilePTACotVta> pLstFiles, bool pBolEsCCCF, int pIntIdPedido, double pDblMontoPedido, double pDblMontoPedidoRound, string pStrNroTarjeta, string pStrIdForma, string pStrIdValor, string pStrIdUsuBD, DateTime pDatFecPedido, string pStrComentarios, bool pBolEsRutaSelva, bool pBolEsResPub);
        DataTable _Get_ComprobantesBoletosBy_IdFile(int pIntIdFile, Int16 pIntIdSucursal);
        bool _Existe_Vendedor_SubArea(Int16 pIntIdArea, Int16 pIntIdSubArea, string pStrIdVendedor);
        void _Update_FechaCierreVenta(List<Models.FilePTACotVta> pLstFiles, Int16 pIntIdEmpresa, string pStrIdVendedorRegistra, string pStrIdVendedorCreaCot, string pStrLoginUsuWebRegistra, Nullable<bool> pBolUATPExoneradoFirmaCliente, string pStrNomVendedorCounter, bool pActualizaVendedor);
        void _Insert_TextoFile(int pIntIdFile, Int16 pIntIdSucursal, string pStrTextoFile, string pStrLoginUsuWeb, Int16 pIntIdEmpresa);
        DataTable _SelectFilesIdBy_IdCot(int pIntIdCotVta);
        void _Delete_Cot_File(int pIntCotId, int pIntFileId);
        void _Insert(int pIntIdUsuario, string pStrNomPagina, string pStrComment, int pIntIdLang, int pIntIdWeb, string pStrQuery, string pStrIP);
    }
}
