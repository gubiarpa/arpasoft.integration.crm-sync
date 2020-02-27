using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IFactFileRepository
    {
        Operation GuardarDatosFacturacion(FactFileRetailReq model);
        void EliminarDetalleTarifa(int IdDatosFacturacion);
        void EliminarDetalleNoRecibos(int IdDatosFacturacion);
        void GuardarDetalleTarifa(FactFileRetailReq model, int IdDatosFacturacion);
        void GuardarDetalleNoRecibo(FactFileRetailReq model, int IdDatosFacturacion);
    }
}