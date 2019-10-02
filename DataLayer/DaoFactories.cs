/*
(C)2019 
 Autor: Iteki
*/
using System;
using System.Collections.Generic;
using System.Text;
using iteki.GO.DataLayer.DataObjects;
using iteki.GO.DataLayer.SqlServer;

namespace iteki.GO.DataLayer.DataObjects
{
    /// <summary>
    /// Factory of factories. This class is a factory class that creates
    /// data-base specific factories which in turn create data acces objects.
    /// 
    /// GoF Design Patterns: Factory.
    /// </summary>
    /// <remarks>
    /// This is the abstract factory design pattern applied in a hierarchy
    /// in which there is a factory of factories.
    /// </remarks>
    public class DaoFactories
    {
        /// <summary>
        /// Gets a provider specific (i.e. database specific) factory 
        /// 
        /// GoF Design Pattern: Factory
        /// </summary>
        /// <param name="dataProvider">Database provider.</param>
        /// <returns>Data access object factory.</returns>
        public static DaoFactory GetFactory(string dataProvider)
        {
            // Return the requested DaoFactory
            switch (dataProvider)
            {
                case "System.Data.SqlClient": return new SqlServerDaoFactory();
                default: return new SqlServerDaoFactory();
            }
        }
    }
}
