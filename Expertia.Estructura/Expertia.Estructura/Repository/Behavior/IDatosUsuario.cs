using System;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using System.Collections.Generic;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface IDatosUsuario
    {
        UsuarioLogin Get_Dts_Usuario_Personal(int UsuarioID);
    }
}
