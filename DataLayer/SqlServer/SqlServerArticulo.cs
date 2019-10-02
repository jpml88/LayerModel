using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using iteki.GO.DataLayer.DataObjects;
using iteki.GO.DataLayer.Interfaces;
using iteki.GO.BusinessLayer.ValueObjects;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace iteki.GO.DataLayer.SqlServer
{
    public partial class SqlServerArticulo : IArticulo
    {        
        #region Obtener

        private String Obtener_Consulta_Ejemplos()
        {
            try
            {
                StringBuilder _sql = new StringBuilder();
                _sql.AppendLine("SELECT	Articulos.Id_Articulo");
                _sql.AppendLine("		,Articulos.Descripcion");
                _sql.AppendLine("		,Articulos.FK_Id_Articulo_Categoria");
                _sql.AppendLine("		,Articulos.Ultimo_Precio_Compra");
                _sql.AppendLine("		,Articulos.FK_Id_Ultimo_Proveedor");
                _sql.AppendLine("		,Articulos.FK_Id_Marca");
                _sql.AppendLine("		,Articulos.Porcentaje_Rebaja_Promocion");
                _sql.AppendLine("		,Funciones.ObtenerCadenaCategorias(FK_Id_Articulo_Categoria) AS Cadena_Categorias");
                _sql.AppendLine("		,Proveedores.Nombre  AS Proveedor_Nombre");
                _sql.AppendLine("		,Marcas.Nombre AS Marca_Nombre");
                _sql.AppendLine("FROM	iteki19_go_project.Compras.Articulos WITH(NOLOCK)");
                _sql.AppendLine("		INNER JOIN iteki19_go_project.Compras.Marcas WITH(NOLOCK) ON Articulos.FK_Id_Marca = Marcas.Id_Marca ");
                _sql.AppendLine("		INNER JOIN iteki19_go_project.Compras.Proveedores  WITH(NOLOCK) ON Articulos.FK_Id_Ultimo_Proveedor = Proveedores.Id_Proveedor");
                return _sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OArticulo Obtener_Articulo(Int32 pId_Articulo)
        {
            StringBuilder _sql = new StringBuilder();
            try
            {
                _sql.AppendLine(Obtener_Consulta_Ejemplos());
                _sql.AppendLine("WHERE   Id_Articulo = " + pId_Articulo);
                OArticulo[] _resultado = (OArticulo[])Db.Procesar_DataTable(Db.GetDataTable(_sql.ToString()));
                if (_resultado.Length == 1)
                    return _resultado[0];
                return null;
            }
            catch (SqlException error_sql)
            {
                throw error_sql;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public OArticulo[] Obtener_Articulos()
        {
            StringBuilder _sql = new StringBuilder();
            try
            {
                _sql.AppendLine(Obtener_Consulta_Ejemplos());
                OArticulo[] _resultado = (OArticulo[])Db.Procesar_DataTable(Db.GetDataTable(_sql.ToString()));
                if (_resultado.Length == 0)
                    return null;
                return _resultado;
            }
            catch (SqlException error_sql)
            {
                throw error_sql;
            }
            catch (Exception error)
            {
                throw error;
            }
        }
        
        #endregion        
    }
}
