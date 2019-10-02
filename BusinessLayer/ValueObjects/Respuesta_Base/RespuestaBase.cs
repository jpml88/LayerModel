/*
(C)2019
 Autor: Iteki
*/
using System;
using System.Reflection;

namespace iteki.GO
{
	/// <summary>
	/// Esta clase es la respuesta estandar que enviará un webservice, contiene la información basica para hacer el seguimiento y tratamiento del resultado de una operación.
	/// </summary>
	[Serializable]
	public class RespuestaEstandar
	{
		public Resultado Resultado = Resultado.Error;
        public String Mensaje = null;
        public Int32 Id_Error = 0;
        public String Clase_Ejecucion = null;
        public String Metodo_Falla = null;
        public String Metodo_Llamador = null;
	}
}
