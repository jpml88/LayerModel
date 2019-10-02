/*
(C)2012 
 Autor: Iteki
*/ 
using System;
using System.Collections.Generic;
using System.Text;
using iteki.GO.DataLayer.Interfaces;
using iteki.GO.DataLayer;

namespace iteki.GO.DataLayer.SqlServer
{
    /// <summary>
    /// Factory espec?fico para SQL-Server que crea los DAO para cada entidad.
    /// 
    /// GoF Design Pattern: Factory.
    /// </summary>
    public class SqlServerDaoFactory : DaoFactory
    {
        public override IArticulo Articulo
        {
            get { return new SqlServerArticulo(); }
        }
   }
}