using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iteki.GO.DataLayer.DataObjects
{
	[Serializable]
    enum Tipos_Dato
    {
        Sin_Especificar = 0,
        Int16 = 1,
        Int32 = 2,
        Int64 = 3,
        String = 4,
        DateTime = 5,
        Float = 6,
        Double = 7,
        Decimal = 8,
        Boolean = 9,
    }

    [Serializable]
    class Atributos_Clase
    {
        private String nombre;
        private Tipos_Dato tipo_dato;
        private Boolean es_nullable;

        #region Get y Set

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public Tipos_Dato Tipo_Dato
        {
            get { return tipo_dato; }
            set { tipo_dato = value; }
        }

        public Boolean Es_Nullable
        {
            get { return es_nullable; }
            set { es_nullable = value; }
        }

        #endregion

        public Atributos_Clase()
        {
            nombre = null;
            tipo_dato = Tipos_Dato.Sin_Especificar;
            es_nullable = false;
        }

        public Atributos_Clase(String pNombre, Tipos_Dato pTipo_Dato, Boolean pEs_Nullable)
        {
            nombre = pNombre;
            tipo_dato = pTipo_Dato;
            es_nullable = pEs_Nullable;
        }
    
    }
}
