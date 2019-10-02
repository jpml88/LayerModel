/*
(C)2019
 Autor: Iteki
*/
using System;

namespace iteki.GO
{
	
	/// <summary>
	/// Enumeración de los posibles resultados de una respuesta estandar.
	/// </summary>
	[Serializable]
	public enum Resultado
	{
		Error = 0,
		Ok = 1,
        Advertencia = 2
	}
}
