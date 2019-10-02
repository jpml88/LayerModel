/*
(C)2019 
 Autor: Iteki
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using iteki.GO.BusinessLayer.ValueObjects;


namespace iteki.GO.DataLayer.Interfaces
{
    public interface IArticulo
    {
        OArticulo Obtener_Articulo(Int32 pId_Articulo);
        OArticulo[] Obtener_Articulos();
    }
}