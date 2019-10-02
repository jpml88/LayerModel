using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iteki.GO.BusinessLayer.ValueObjects
{
    [Serializable]
    public class OArticulo
    {
        private Int64 id_articulo;
        private String descripcion;
        private Int16 fk_id_articulo_categoria;
        private decimal ultimo_precio_compra;
        private Int32 fk_id_ultimo_proveedor;
        private Int16 fk_id_marca;
        private Int16 porcentaje_rebaja_promocion;
        private String cadena_categorias;
        private String proveedor_nombre;
        private String marca_nombre;


        #region Get y Set
        public Int64 Id_Articulo
        {
            get { return id_articulo; }
            set { id_articulo = value; }
        }

        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public Int16 FK_Id_Articulo_Categoria
        {
            get { return fk_id_articulo_categoria; }
            set { fk_id_articulo_categoria = value; }
        }

        public decimal Ultimo_Precio_Compra
        {
            get { return ultimo_precio_compra; }
            set { ultimo_precio_compra = value; }
        }

        public Int32 FK_Id_Ultimo_Proveedor
        {
            get { return fk_id_ultimo_proveedor; }
            set { fk_id_ultimo_proveedor = value; }
        }

        public Int16 FK_Id_Marca
        {
            get { return fk_id_marca; }
            set { fk_id_marca = value; }
        }

        public Int16 Porcentaje_Rebaja_Promocion
        {
            get { return porcentaje_rebaja_promocion; }
            set { porcentaje_rebaja_promocion = value; }
        }

        public String Cadena_Categorias
        {
            get { return cadena_categorias; }
            set { cadena_categorias = value; }
        }

        public String Proveedor_Nombre
        {
            get { return proveedor_nombre; }
            set { proveedor_nombre = value; }
        }

        public String Marca_Nombre
        {
            get { return marca_nombre; }
            set { marca_nombre = value; }
        }

        #endregion

        public OArticulo()
        {
            id_articulo = 0;
            descripcion = null;
            fk_id_articulo_categoria = 0;
            ultimo_precio_compra = 0;
            fk_id_ultimo_proveedor = 0;
            fk_id_marca = 0;
            porcentaje_rebaja_promocion = 0;
            cadena_categorias = null;
            proveedor_nombre = null;
            marca_nombre = null;
        }
    }



}
