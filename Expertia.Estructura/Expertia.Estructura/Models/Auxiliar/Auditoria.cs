using Expertia.Estructura.Models.Foreign;
using System;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Auditoria
    {
        // Create
        public SystemUser CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        // Modify
        public SystemUser ModifyUser { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}