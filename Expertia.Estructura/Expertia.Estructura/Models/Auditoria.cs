using System;

namespace Expertia.Estructura.Models
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