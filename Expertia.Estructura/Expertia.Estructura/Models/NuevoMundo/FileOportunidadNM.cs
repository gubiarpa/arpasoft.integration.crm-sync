using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    /**Send**/
    public class FileOportunidadNM
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }
        public int idCotSrv_SF { get; set; }
        public int numeroFile { get; set; }
        public string importe { get; set; }
        public short sucursal { get; set; }
        public string fecha { get; set; }
        public string accion_SF { get; set; }
    }

    public class RptaFileNM_SF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }        
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    /**Asociate**/
    public class Oportunidad_FileNMRQ
    {
        public string accion_SF { get; set; }
        public List<FileNM> lstFiles { get; set; }
        public int idUsuario { get; set; }
        public int idCotSRV { get; set; }
        public string idoportunidad_SF { get; set; }
    }

    public class AssociateNMFileRS
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public List<FileNMRS> lstFiles { get; set; }
    }

    public class FileNM
    {
        public int? idFilePTA { get; set; }
        public short? Sucursal { get; set; }
    }

    public class FileNMRS
    {
        public int idFilePTA { get; set; }
        public short sucursal { get; set; }
        public string importe { get; set; }        
        public string fecha { get; set; }        
    }
}