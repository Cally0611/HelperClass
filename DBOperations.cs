
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrlofOeev5.HelperClasses
{
    public class DBOperations
    {
        public string DBcommand
        {
            set
            {
                _dbcommand = value;
            }
        }

        private string _dbcommand;

        public string SPcommand
        {
            set
            {
                _spcommand = value;
            }
        }


        //private int _countofparam;

        //public int Countofparam
        //{
        //    set
        //    {
        //        _countofparam = value;
        //    }
        //    get
        //    {
        //        return _countofparam;
        //    }
        //}

        private string _paramname;

        public string Paramname
        {
            set
            {
                _paramname = value;
            }
            get
            {
                return _paramname;
            }
        }

        private string _spcommand;

        public List<object> Sourcelist
        {
            get
            {
                return _sourcelist;
            }
            set
            {
                _sourcelist = value;
            }
        }

        private List<object> _sourcelist;

        public SqlDbType DbType
        {
            get
            {
                return _dbtype;
            }
            set
            {
                _dbtype = value;
            }

        }

        private SqlDbType _dbtype;

        public object ParamValue
        {
            get
            {
                return _paramvalue;
            }
            set
            {
                _paramvalue = value;
            }
        }

        private object _paramvalue;


        public SqlCommand SQLCommand
        {
            get
            {
                return _sqlcommand;
            }
            set
            {
                _sqlcommand = value;
            }
        }

        private SqlCommand _sqlcommand;

        public SqlDataReader Sqldreader
        {
            get
            {
                return _sqldreader;
            }
            set
            {
                _sqldreader = value;
            }
        }

        private SqlDataReader _sqldreader;

        public SqlParameter[] Sqlparam
        {
            get
            {
                return _sqlparam;
            }
            set
            {
                _sqlparam = value;
            }
        }

        private SqlParameter[] _sqlparam;

        //public SqlParameter Sqlparamitem
        //{
        //    get
        //    {
        //        return _sqlparamitem;
        //    }
        //    set
        //    {
        //        _sqlparamitem = value;
        //    }
        //}

        //private SqlParameter _sqlparamitem;


        //public static SqlConnection Conn = new SqlConnection(ConnectionClass.consString);
        public SqlConnection Conn
        {
            get
            {
                return new SqlConnection(ConnectionClass.consString);
            }
        }


        protected internal void OpenConnection()
        {
            Conn.Open();
        }



        protected internal void CloseConnection()
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }

        protected internal void DisposeConnection()
        {
            try
            {
                //Clean Up Connection Object
                if (Conn != null)
                {
                    if (Conn.State != ConnectionState.Closed)
                    {
                        Conn.Close();
                    }
                    Conn.Dispose();
                }

                //Clean Up Command Object
                if (_sqlcommand != null)
                {
                    _sqlcommand.Dispose();
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Error disposing data class." + Environment.NewLine + ex.Message);
            }
        }

        protected internal bool ExecuteSP()
        {
            try
            {
                using (Conn)
                {
                    //Conn.Open();
                    using (_sqlcommand = new SqlCommand())
                    {
                        _sqlcommand.Connection = Conn;
                        _sqlcommand.Connection.Open();

                        _sqlcommand.CommandType = CommandType.StoredProcedure;
                        _sqlcommand.CommandText = _spcommand;
                        _sqlcommand.Parameters.AddRange(_sqlparam);
                        _sqlcommand.ExecuteNonQuery();
                        _sqlcommand.Connection.Close();
                    }

                }
                //Conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                //System.Web.HttpContext.Current.Server.Transfer("../ErrorFinePlan.aspx", true);
                return false;
            }
            finally
            {
                try
                {
                    CloseConnection();
                    DisposeConnection();
                }
                catch (Exception)
                {
                }
            }

        }

        protected internal bool ExecuteSQLText()
        {
            try
            {
                using (Conn)
                {
                    //Conn.Open();
                    using (_sqlcommand = new SqlCommand())
                    {
                        _sqlcommand.Connection = Conn;
                        _sqlcommand.Connection.Open();
                        _sqlcommand.CommandType = CommandType.Text;
                        _sqlcommand.CommandText = _spcommand;
                        _sqlcommand.Parameters.AddRange(_sqlparam);
                        _sqlcommand.ExecuteNonQuery();
                        _sqlcommand.Connection.Close();
                    }

                }
                //Conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                try
                {
                    CloseConnection();
                    DisposeConnection();
                }
                catch (Exception)
                {
                }
            }
        }

        protected internal int ExecuteSPSelect1()
        {
            Int32 count = 0;
            try
            {
                using (Conn)
                {
                    //Conn.Open();
                    using (_sqlcommand = new SqlCommand())
                    {
                        _sqlcommand.Connection = Conn;
                        _sqlcommand.Connection.Open();
                        _sqlcommand.CommandType = CommandType.Text;
                        _sqlcommand.CommandText = _spcommand;
                        _sqlcommand.Parameters.AddRange(_sqlparam);

                        _sqlcommand.ExecuteScalar();
                        count = (Int32)_sqlcommand.ExecuteScalar();
                        _sqlcommand.Connection.Close();

                    }
                    //Conn.Close();
                    return count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                try
                {
                    CloseConnection();
                    DisposeConnection();
                }
                catch (Exception)
                {
                }
            }

        }

        //protected internal List<object> ExecuteSPSelectMorethan1()
        protected internal List<T> ExecuteSPSelectMorethan1<T>() where T : new()
        {
            List<T> readdata = new List<T>();
            try
            {
                using (Conn)
                {
                    // Conn.Open();
                    using (_sqlcommand = new SqlCommand())
                    {
                        _sqlcommand.Connection = Conn;
                        _sqlcommand.Connection.Open();
                        _sqlcommand.CommandType = CommandType.Text;
                        _sqlcommand.CommandText = _spcommand;
                        _sqlcommand.Parameters.AddRange(_sqlparam);

                        //set this sqldatareader
                        _sqldreader = _sqlcommand.ExecuteReader();

                        while (_sqldreader.Read())
                        {
                            T t = new T();

                            for (int inc = 0; inc < _sqldreader.FieldCount; inc++)
                            {
                                Type type = t.GetType();
                                string name = _sqldreader.GetName(inc);
                                PropertyInfo prop = type.GetProperty(name);
                                if (prop != null)
                                {
                                    if (name == prop.Name)
                                    {
                                        object value = _sqldreader.GetValue(inc);
                                        if (value != DBNull.Value)
                                        {
                                            prop.SetValue(t, ChangeType(value, prop.PropertyType), null);
                                        }
                                        else
                                        {
                                            value = null;
                                            //prop.SetValue(t, value, null);
                                        }

                                        //var value = _sqldreader.GetValue(inc);

                                        //if (value != DBNull.Value)
                                        //{
                                        //    prop.SetValue(t, ChangeType(value, prop.PropertyType), null);
                                        //    //prop.SetValue(t, Convert.ChangeType(value, prop.PropertyType), null);

                                        //}
                                        ////else
                                        ////{
                                        //    prop.SetValue(t, value, null);
                                        ////}
                                    }
                                }
                            }

                            readdata.Add(t);
                        }



                        _sqldreader.Close();
                        _sqlcommand.Connection.Close();
                    }
                    //Conn.Close();
                    return readdata;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                try
                {
                    CloseConnection();
                    DisposeConnection();
                }
                catch (Exception)
                {
                }
            }
        }


        protected internal SqlDataReader ExecuteSPSelect()
        {
            try
            {
                using (Conn)
                {
                    //Conn.Open();
                    using (_sqlcommand = new SqlCommand())
                    {
                        _sqlcommand.Connection = Conn;
                        _sqlcommand.Connection.Open();
                        _sqlcommand.CommandType = CommandType.Text;
                        _sqlcommand.CommandText = _spcommand;
                        _sqlcommand.Parameters.AddRange(_sqlparam);

                        _sqlcommand.ExecuteReader();
                        _sqldreader = (SqlDataReader)_sqlcommand.ExecuteReader();
                        _sqlcommand.Connection.Close();

                    }
                    //Conn.Close();
                    return _sqldreader;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                try
                {
                    CloseConnection();
                    DisposeConnection();
                }
                catch (Exception)
                {
                }
            }
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }


    }
}
