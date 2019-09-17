using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Contacto_DM_Repository : OracleBase<Contacto>, ICrud<Contacto>
    {
        public Contacto_DM_Repository() : base(ConnectionKeys.DMConnKey)
        {
        }

        public Operation Create(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Delete(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Contacto entity)
        {
            throw new NotImplementedException();
        }
    }
}