using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace iteki.GO.BusinessLayer.ValueObjects
{
    [Serializable]
    public class RS_Articulo : RespuestaEstandar
    {
        public OArticulo Articulo;
    }

    [Serializable]
    public class RS_Articulos : RespuestaEstandar
    {
        public OArticulo[] Articulos;
    }
}
