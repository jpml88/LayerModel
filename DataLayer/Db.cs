using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;

namespace iteki.GO.DataLayer.DataObjects
{
    /// <summary>
    /// En esta clase se realizan las conexiones a las bases de datos.
    ///
    /// Patrones de diseño aplicados: Singleton, Factory, Proxy.
    /// </summary>
    public static class Db
    {
        private static readonly string Nombre_Assembly_Clases = "ValueObjects";
        private static readonly string Data_Provider = ConfigurationManager.AppSettings.Get("DataProvider");
        private static readonly DbProviderFactory Factory = DbProviderFactories.GetFactory(Data_Provider);
        private static readonly string Connection_String = ConfigurationManager.ConnectionStrings["ConnectionBD"].ConnectionString;
        public static readonly string Entorno = ConfigurationManager.AppSettings.Get("Entorno");

        #region Insert/Update

        /// <summary>
        /// Ejecuta un update en la base a la que se conecta.
        /// </summary>
        /// <param name="pQuery">Consulta a la base de datos para actualización.</param>
        /// <returns>Cantidad de registros afectados.</returns>
        public static int Update(String pQuery)
        {
            using (DbConnection _conexion = Factory.CreateConnection())
            {
                _conexion.ConnectionString = Connection_String;
                using (DbCommand command = Factory.CreateCommand())
                {
                    command.Connection = _conexion;
                    command.CommandText = pQuery;
                    _conexion.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Ejecuta un Insert en la base de datos a la que se conecta, opcionalmente puede devolver un ID si la tabla tiene identity y, tambien opcionalmente, se le pueden pasar parámetros SQL para la inserción.
        /// </summary>
        /// <param name="pQuery">Consulta a la base de datos para inserción.</param>
        /// <param name="pObtener_ID">Indica cuando se debe devolver el id del regsistro insertado.</param>
        /// <param name="pParametro">Parámetro SQL</param>
        /// <returns>Identidad generada (valor numérico).</returns>
        public static int Insert(string pQuery, bool pObtener_ID, SqlParameter pParametro)
        {
            using (DbConnection _conexion = Factory.CreateConnection())            {
                _conexion.ConnectionString = Connection_String;
                using (DbCommand _comando = Factory.CreateCommand())
                {
                    if (pParametro.Value != null)
                        _comando.Parameters.Add(pParametro);
                    _comando.Connection = _conexion;
                    _comando.CommandText = pQuery;
                    _conexion.Open();
                    _comando.ExecuteNonQuery();
                    int id = -1;
                    //Si necesita obtener el ID
                    if (pObtener_ID)
                    {
                        string identitySelect;
                        switch (Data_Provider)
                        {
                            // Access
                            case "System.Data.OleDb":
                                identitySelect = "SELECT @@IDENTITY";
                                break;
                            // Sql Server
                            case "System.Data.SqlClient":
                                identitySelect = "SELECT SCOPE_IDENTITY()";
                                break;
                            // Oracle
                            case "System.Data.OracleClient":
                                identitySelect = "SELECT MySequence.NEXTVAL FROM DUAL";
                                break;
                            default:
                                identitySelect = "SELECT @@IDENTITY";
                                break;
                        }
                        _comando.CommandText = identitySelect;
                        id = int.Parse(_comando.ExecuteScalar().ToString());
                    }
                    return id;
                }
            }
        }

        /// <summary>
        /// Ejecuta un Insert en la base de datos sin devolver ID.
        /// </summary>
        /// <param name="pQuery">Consulta a la base de datos para inserción.</param>
        public static void Insert(string pQuery)
        {
            Insert(pQuery, false, null);
        }

        /// <summary>
        /// Ejecuta un Insert en la base de datos sin devolver ID con la posibilidad de devolver el ID.
        /// </summary>
        /// <param name="pQuery">Consulta a la base de datos para inserción.</param>
        public static void Insert(string pQuery, bool pObtener_ID)
        {
            Insert(pQuery, pObtener_ID, null);
        }

        #endregion

        #region Obtención de datos

        /// <summary>
        /// Llena el dataset en base a la consulta SQL enviada.
        /// </summary>
        /// <param name="pQuery">Consulta en SQL.</param>
        /// <returns>Dataset llenado.</returns>
        public static DataSet GetDataSet(string pQuery)
        {
            using (DbConnection _connection = Factory.CreateConnection())
            {
                _connection.ConnectionString = Connection_String;
                using (DbCommand _command = Factory.CreateCommand())
                {
                    _command.Connection = _connection;
                    _command.CommandType = CommandType.Text;
                    _command.CommandText = pQuery;
                    using (DbDataAdapter _adapter = Factory.CreateDataAdapter())
                    {
                        _adapter.SelectCommand = _command;
                        DataSet ds = new DataSet();
                        _adapter.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        public static object Procesar_DataTable(DataTable dt, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            try
            {
                int i = 0;
                Assembly _asm_sql = typeof(Db).Assembly;
                Type _clase_datos = _asm_sql.GetTypes().First(t => t.Name.EndsWith(callerFilePath.Substring(callerFilePath.LastIndexOf('\\') + 1, callerFilePath.LastIndexOf('.') - callerFilePath.LastIndexOf('\\') - 1)));
                MethodInfo _info_metodo = _clase_datos.GetMethod(callerMember);
                String _nombre_clase = _info_metodo.ReturnType.Name.Replace("[]", "");
                Type _clase;
                PropertyInfo[] _propiedades_clase;
                Assembly _assembly_value = typeof(Resultado).Assembly;
                try
                {
                    _clase = _assembly_value.GetTypes().First(t => t.FullName.EndsWith(_nombre_clase));
                    _propiedades_clase = _clase.GetProperties();
                }
                catch (Exception ex)
                {
                    throw new Exception("La clase '" + _nombre_clase + "' no existe. Contactar a soporte ITEKI." + ex.Message);
                }
                var _resultado = Array.CreateInstance(_clase, dt.Rows.Count);
                IList<Atributos_Clase> _atributos_clase = new List<Atributos_Clase>();
                String _tipo_atributo = null;
                #region Carga de atributos de clase
                foreach (PropertyInfo _propiedad in _propiedades_clase)
                {
                    if (!_propiedad.PropertyType.FullName.Contains(Nombre_Assembly_Clases))
                    {
                        Atributos_Clase _atributo_clase = new Atributos_Clase();
                        _atributo_clase.Nombre = _propiedad.Name;
                        if (_propiedad.PropertyType.Name.Contains("Nullable"))
                        {
                            _tipo_atributo = _propiedad.PropertyType.GenericTypeArguments[0].Name;
                            _atributo_clase.Es_Nullable = true;
                        }
                        else
                            _tipo_atributo = _propiedad.PropertyType.Name;
                        switch (_tipo_atributo)
                        {
                            case "Int16":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Int16;
                                break;
                            case "Int32":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Int32;
                                break;
                            case "Int64":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Int64;
                                break;
                            case "String":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.String;
                                break;
                            case "DateTime":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.DateTime;
                                break;
                            case "Float":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Float;
                                break;
                            case "Double":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Double;
                                break;
                            case "Decimal":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Decimal;
                                break;
                            case "Boolean":
                                _atributo_clase.Tipo_Dato = Tipos_Dato.Boolean;
                                break;
                            default:
                                throw new Exception("Tipo de dato no controlado en cargar dt (" + _tipo_atributo + "). Contactar a soporte ITEKI.");
                        }
                        _atributos_clase.Add(_atributo_clase);
                    }
                }
                #endregion
                foreach (DataRow _fila in dt.Rows)
                {
                    var _objeto = Activator.CreateInstance(_clase);
                    //Type _tipo = _objeto.GetType();
                    #region Carga de atributos
                    foreach (Atributos_Clase _atributo_clase in _atributos_clase)
                    {
                        PropertyInfo prop = _clase.GetProperty(_atributo_clase.Nombre);
                        if (!_atributo_clase.Es_Nullable || (_atributo_clase.Es_Nullable && _fila[_atributo_clase.Nombre].ToString() != ""))
                        {
                            try
                            {
                                switch (_atributo_clase.Tipo_Dato)
                                {
                                    case Tipos_Dato.Int16:
                                        prop.SetValue(_objeto, Int16.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Int32:
                                        prop.SetValue(_objeto, Int32.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Int64:
                                        prop.SetValue(_objeto, Int64.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.String:
                                        prop.SetValue(_objeto, _fila[_atributo_clase.Nombre].ToString(), null);
                                        break;
                                    case Tipos_Dato.DateTime:
                                        prop.SetValue(_objeto, DateTime.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Float:
                                        prop.SetValue(_objeto, float.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Double:
                                        prop.SetValue(_objeto, Double.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Decimal:
                                        prop.SetValue(_objeto, Decimal.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    case Tipos_Dato.Boolean:
                                        prop.SetValue(_objeto, Boolean.Parse(_fila[_atributo_clase.Nombre].ToString()), null);
                                        break;
                                    default:
                                        throw new Exception("Tipo de dato no controlado en cargar dt (" + _atributo_clase.Nombre + "). Contactar a soporte ITEKI.");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Atributo:'" + _atributo_clase.Nombre + "'. Valor:'" + _fila[_atributo_clase.Nombre].ToString() + "'.Error:" + ex.Message);
                            }
                        }
                        else if (_atributo_clase.Es_Nullable && _fila[_atributo_clase.Nombre].ToString() == "")
                        {
                            prop.SetValue(_objeto, null, null);
                        }
                        else
                        {
                            throw new Exception("El campo '" + _atributo_clase.Nombre + "' no es nullable y en la BD está vacío.");
                        }
                    }
                    #endregion
                    _resultado.SetValue(_objeto, i);
                    i++;
                }
                return _resultado;
            }
            catch (Exception error)
            {
                throw new Exception("Ocurrio un error al cargar los datos en SqlServerEjemplo - Cargar_DataTable. " + error.Message);
            }
        }

        /// <summary>
        /// Llena el DataTable en base a la consulta SQL enviada.
        /// </summary>
        /// <param name="pQuery">Consulta en SQL.</param>
        /// <returns>DataTable llenado.</returns>
        public static DataTable GetDataTable(string pQuery)
        {
            return GetDataSet(pQuery).Tables[0];
        }

        /// <summary>
        /// Llena el DataRow en base a la consulta SQL enviada.
        /// </summary>
        /// <param name="pQuery">Sql statement.</param>
        /// <returns>DataRow llenado.</returns>
        public static DataRow GetDataRow(string pQuery)
        {
            DataRow _fila = null;
            DataTable _dt = GetDataTable(pQuery);
            if (_dt.Rows.Count > 0)
            {
                _fila = _dt.Rows[0];
            }
            return _fila;
        }

        /// <summary>
        /// Llena el objecto escalar en base a la consulta SQL enviada.
        /// </summary>L enviada.
        /// </summary>
        /// <param name="pQuery">Sql statement.</param>
        /// <returns>Escalar llenado.</returns>
        public static object GetScalar(string pQuery)
        {
            using (DbConnection _connection = Factory.CreateConnection())            {
                _connection.ConnectionString = Connection_String;
                using (DbCommand _command = Factory.CreateCommand())
                {
                    _command.Connection = _connection;
                    _command.CommandText = pQuery;
                    _connection.Open();
                    return _command.ExecuteScalar();
                }
            }
        }

        #endregion

        #region Metodos utiles para la operación con bases de datos

        /// <summary>
        /// Convierte un dato en formato datetime a un string formateado para su almacenamiento en una base de datos.
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns></returns>
        public static string Castear_Fecha(DateTime pFecha)
        {
            return "convert(datetime, " + Db.Escape(pFecha.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",121)");
        }

        /// <summary>
        /// Convierte un dato en formato boolean a un string formateado para su almacenamiento en una base de datos.
        /// </summary>
        /// <param name="pBoolean"></param>
        /// <returns></returns>
        public static int Castear_Boolean(Boolean pBoolean) 
        {
            if (pBoolean)            
                return 1;            
            else 
                return 0;           

        }

        /// <summary>
        /// Rodea una cadena de texto con comillas simples para su procesamiento a nivel base de datos.
        /// Ademas, se le quitan los espacios en blanco que estén al pcpio o al final de la cadena.
        /// Si la cadena es nula, devuelve el valor NULL.
        /// </summary>
        /// <param name="pString">String de entrada.</param>
        /// <returns>String de salida.</returns>
        public static string Escape(string pString)
        {
            if (String.IsNullOrEmpty(pString))
                return "NULL";
            else if (pString.Trim() == "True")
            {
                return "'1'";
            }
            else if (pString.Trim() == "False")
            {
                return "'0'";
            }
            else
            {
                return "'" + pString.Trim().Replace("'", "''") + "'";
            }
        }

        /// <summary>
        /// Rodea una cadena de texto con comillas simples para su procesamiento a nivel base de datos.
        /// Ademas, se le quitan los espacios en blanco que estén al pcpio o al final de la cadena.
        /// También limita el largo de la cadena.
        /// Si la cadena es nula, devuelve el valor NULL.
        /// </summary>
        /// <param name="pString">Input string.</param>
        /// <param name="pLargo_Maximo">Largo maximo de la cadena de texto.</param>
        /// <returns>String de salida.</returns>
        public static string Escape(string pString, int pLargo_Maximo)
        {
            if (String.IsNullOrEmpty(pString))
                return "NULL";
            else
            {
                pString = pString.Trim();
                if (pString.Length > pLargo_Maximo) pString = pString.Substring(0, pLargo_Maximo - 1);
                return "'" + pString.Trim().Replace("'", "''") + "'";
            }
        }

        /// <summary>
        /// Convierte un dato en formato float a un string formateado para su almacenamiento en una base de datos.
        /// </summary>
        /// <param name="pFloat"></param>
        /// <returns></returns>
        public static string Castear_Flotante(float pFloat)
        {
            return pFloat.ToString().Replace(',', '.');
        }

        /// <summary>
        /// Convierte un dato en formato double a un string formateado para su almacenamiento en una base de datos.
        /// </summary>
        /// <param name="pDouble"></param>
        /// <returns></returns>
        public static string Castear_Double(double pDouble)
        {
            return pDouble.ToString().Replace(',', '.');
        }
        
        /// <summary>
        /// Convierte un dato en formato decimal a un string formateado para su almacenamiento en una base de datos.
        /// Lo redondea a la cantidad de posiciones decimales pasadas como parametro.
        /// </summary>
        /// <param name="pValor"></param>
        /// <param name="pPosiciones_Decimales"></param>
        /// <returns></returns>
        public static string Castear_Decimal(decimal pValor, Int16 pPosiciones_Decimales)
        {
            try
            {
                return Math.Round(pValor, pPosiciones_Decimales).ToString().Replace(',', '.').Replace(';', '.');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Convierte un dato en formato string proveniente de la base de datos a un decimal cuyas posiciones decimales están dadas por el parámetro destinado a tal fin.
        /// </summary>
        /// <param name="pValor"></param>
        /// <param name="pPosiciones_Decimales"></param>
        /// <returns></returns>
        public static decimal Obtener_Decimal(String pValor, Int16 pPosiciones_Decimales)
        {
            try
            {
                decimal _aux = Math.Round(decimal.Parse(pValor), pPosiciones_Decimales);
                return _aux;
            }
            catch
            {
                throw new Exception("El valor indicado no corresponde a un decimal");
            }
        }

        #endregion
    }
}
