﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Controllers.Behavior
{
    public interface IClientFeatures
    {
        string Ip { get; }
        string Method { get; }
        string Uri { get; }
    }
}
