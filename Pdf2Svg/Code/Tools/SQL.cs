
using System;


namespace Pdf2Svg
{


    class SQL
    {


        public static string GetConnectionStringOld()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = System.Environment.MachineName;
            csb.InitialCatalog = "COR_Basic_Swisscom";
            csb.IntegratedSecurity = true;

            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "ApertureWebServicesDE";
                csb.Password = "";
            }

            csb.PacketSize = 4096;
            csb.Pooling = false;
            csb.ApplicationName = "";
            csb.ConnectTimeout = 5;
            csb.Encrypt = false;
            csb.MultipleActiveResultSets = false;
            csb.PersistSecurityInfo = false;
            csb.Replication = false;
            csb.WorkstationID = System.Environment.MachineName;

            return csb.ConnectionString;
        }

        private static string m_staticConnectionString;
        private static string m_DataProvider;

        public static string GetConnectionString()
        {
            string strReturnValue = null;

            if (string.IsNullOrEmpty(m_staticConnectionString))
            {
                string strConnectionStringName = System.Environment.MachineName;

                if (string.IsNullOrEmpty(strConnectionStringName))
                {
                    strConnectionStringName = "LocalSqlServer";
                }

                System.Configuration.ConnectionStringSettingsCollection settings = System.Configuration.ConfigurationManager.ConnectionStrings;
                if ((settings != null))
                {
                    foreach (System.Configuration.ConnectionStringSettings cs in settings)
                    {
                        if (System.StringComparer.OrdinalIgnoreCase.Equals(cs.Name, strConnectionStringName))
                        {
                            strReturnValue = cs.ConnectionString;
                            m_staticConnectionString = strReturnValue;
                            m_DataProvider = cs.ProviderName;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    strConnectionStringName = "server";

                    System.Configuration.ConnectionStringSettings conString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionStringName];

                    if (conString != null)
                    {
                        strReturnValue = conString.ConnectionString;
                    }
                }

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    throw new System.ArgumentNullException("ConnectionString \"" + strConnectionStringName + "\" in file web.config.");
                }

                settings = null;
                strConnectionStringName = null;
            }
            else // of if (string.IsNullOrEmpty(strStaticConnectionString))
            {
                return m_staticConnectionString;
            }

            return strReturnValue;
        } // End Function GetConnectionString



        public static System.Data.IDbConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(GetConnectionString());
        }


        public static bool Log(System.Exception ex)
        {
            return Log(null, ex, null);
        }

        public static bool Log(System.Exception ex, System.Data.IDbCommand cmd)
        {
            return Log(null, ex, cmd);
        }

        public static bool Log(string str, System.Exception ex, System.Data.IDbCommand cmd)
        {
            System.Console.WriteLine(str);
            System.Console.WriteLine(ex.Message);

            if(cmd != null)
                System.Console.WriteLine(cmd.CommandText);
            return true;
        }

        // public static DB.Abstraction.cDAL DAL = DB.Abstraction.cDAL.CreateInstance("MS_SQL", GetConnectionString());



        public static System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)cmd))
            {
                cmd.Connection = GetConnection();

                da.Fill(dt);
            }

            return dt;
        } //End Function GetDataTable


        public static System.Data.DataTable GetDataTable(string strSQL)
        {
            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                dt = GetDataTable(cmd);
            }

            return dt;
        } //End Function GetDataTable



        public static System.Data.IDbCommand CreateCommand(string strSQL)
        {
            return new System.Data.SqlClient.SqlCommand(strSQL);
        }


        public static int ExecuteNonQuery(string strSQL)
        {
            int retVal = 0;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                retVal = ExecuteNonQuery(cmd);
            }

            return retVal;
        }


        public static int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;
            using (System.Data.IDbConnection idbConn = GetConnection())
            {

                lock (idbConn)
                {

                    lock (cmd)
                    {

                        cmd.Connection = idbConn;

                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                        {

                            try
                            {
                                cmd.Transaction = idbtTrans;

                                iAffected = cmd.ExecuteNonQuery();
                                idbtTrans.Commit();
                            } // End Try
                            catch (System.Data.Common.DbException ex)
                            {
                                if (idbtTrans != null)
                                    idbtTrans.Rollback();

                                iAffected = -2;

                                if (Log(ex))
                                    throw;
                            } // End catch
                            finally
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                    cmd.Connection.Close();
                            } // End Finally

                        } // End Using idbtTrans

                    } // End lock cmd

                } // End lock idbConn

            } // End Using idbConn

            return iAffected;
        } // End Function Execute


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType

            if (objValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = objValue;

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


        // From Type to DBType
        public static System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType



        public static string GetEmbeddedSqlScript(string strScriptName, ref System.Reflection.Assembly ass)
        {
            string strReturnValue = null;

            bool bNotFound = true;
            foreach (string strThisRessourceName in ass.GetManifestResourceNames())
            {
                if (strThisRessourceName != null && strThisRessourceName.EndsWith(strScriptName, StringComparison.OrdinalIgnoreCase))
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(ass.GetManifestResourceStream(strThisRessourceName)))
                    {
                        bNotFound = false;
                        strReturnValue = sr.ReadToEnd();
                        break;
                    }
                } // End if (strThisRessourceName != null && strThisRessourceName.EndsWith(strScriptName, StringComparison.OrdinalIgnoreCase) )
            }

            if (bNotFound)
            {
                throw new System.Exception("No script called \"" + strScriptName + "\" found in embedded ressources.");
            }

            return strReturnValue;
        } // End Function GetEmbeddedSqlScript


        public static string GetEmbeddedSqlScript(string strScriptName)
        {
            //Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            //Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetCallingAssembly()
            System.Reflection.Assembly ass = typeof(SQL).Assembly;
            return GetEmbeddedSqlScript(strScriptName, ref ass);
        } // End Function GetEmbeddedSqlScript


        public static System.Data.IDbCommand CreateCommandFromFile(string strEmbeddedFileName)
        {
            //Start: Rico Test
            if (!string.IsNullOrEmpty(strEmbeddedFileName) && !strEmbeddedFileName.StartsWith(".")) strEmbeddedFileName = "." + strEmbeddedFileName;
            //End: Rico Test

            string strSQL = GetEmbeddedSqlScript(strEmbeddedFileName);
            return CreateCommand(strSQL);
        }

        public static System.Data.DataTable GetDataTableFromEmbeddedRessource(string strFileName)
        {

            using (System.Data.IDbCommand cmd = CreateCommandFromFile(strFileName))
            {
                return GetDataTable(cmd);
            } // cmd

        } // End Function GetDataTableFromEmbeddedRessource



    } // End Class SQL


} // End Namespace Pdf2Svg
