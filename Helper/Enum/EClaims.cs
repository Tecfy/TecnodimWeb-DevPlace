﻿using System.ComponentModel;

namespace Helper.Enum
{
    public enum EClaims
    {
        [Description("Recortar")]
        Recortar = 1,
        [Description("Classificar")]
        Classificar = 2,
        [Description("Digitalizar")]
        Digitalizar = 3,
        [Description("Reenviar")]
        Reenviar = 4,
    }
}
