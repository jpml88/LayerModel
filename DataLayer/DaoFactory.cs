/*
(C)2019
 Autor: Iteki
*/ 
using System;
using System.Collections.Generic;
using System.Text;
using iteki.GO.DataLayer.Interfaces;

namespace iteki.GO.DataLayer
{

    public abstract class DaoFactory
    {
        public abstract IArticulo Articulo { get; }
    }
}

