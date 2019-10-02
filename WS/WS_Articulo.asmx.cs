using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iteki.GO.BusinessLayer.ValueObjects;
using iteki.GO.DataLayer.Interfaces;
using iteki.GO.DataLayer.DataObjects;
using iteki.GO.Web_Service;

namespace iteki.GO.Web_Service
{
    /// <summary>
    /// Summary description for WS_Ejemplo
    /// </summary>
    [WebService(Namespace = "Web_Service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WS_Articulo : System.Web.Services.WebService
    {
        #region Obtener

        [WebMethod(EnableSession = true)]
        public RS_Articulo Obtener_Articulo(Int32 pId_Articulo)
        {
            RS_Articulo _respuesta = new RS_Articulo();
            try
            {
                _respuesta.Articulo = DataAccess.Articulo.Obtener_Articulo(pId_Articulo);
                _respuesta.Resultado = Resultado.Ok;
            }
            catch (Exception ex)
            {
                _respuesta.Resultado = Resultado.Error;
                _respuesta.Mensaje = ex.Message;
                _respuesta.Id_Error = 1;
            }
            return _respuesta;
        }

        [WebMethod(EnableSession = true)]
        public RS_Articulos Obtener_Articulos()
        {
            RS_Articulos _respuesta = new RS_Articulos();
            try
            {
                _respuesta.Articulos = DataAccess.Articulo.Obtener_Articulos();
                _respuesta.Resultado = Resultado.Ok;
            }
            catch (Exception ex)
            {
                _respuesta.Resultado = Resultado.Error;
                _respuesta.Mensaje = ex.Message;
                _respuesta.Id_Error = 1;
            }
            return _respuesta;
        }
        
        #endregion
    }
}
