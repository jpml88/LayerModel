/*
(C)2019
 Autor: Iteki
*/
using System;
using System.Reflection;

namespace iteki.GO
{
	/// <summary>
	/// Esta clase es la respuesta estandar que enviar� un webservice, contiene la informaci�n basica para hacer el seguimiento y tratamiento del resultado de una operaci�n.
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
