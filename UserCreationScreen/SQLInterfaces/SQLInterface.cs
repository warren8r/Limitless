using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using UserCreationScreen.SQLInterfaces.Storage;
using UserCreationScreen.SQLInterfaces.Permissions;

namespace UserCreationScreen.SQLInterfaces
{
    public class SQLInterface
    {
        // Public Constants

        // Public Variables
        public string connectionString;

        // Private Constants

        // Private Variables

        /// <summary>
        /// Default constructor for the SQLInterface class. Sets the connection string to the configuration "TestDatabase".
        /// </summary>
        public SQLInterface()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;
        }

        /// <summary>
        /// Constructor for the SQLInterface class. Sets the connection string to the string given or the connection string aligned with the name given in Web.config.
        /// </summary>
        /// <param name="connectionString">The connectionString, or connectionString name</param>
        /// <param name="useConfigManager">When true the given string is treated as the name of a connection string in Web.config</param>
        public SQLInterface(string connectionString, bool useConfigManager = false)
        {
            if (useConfigManager)
                this.connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            else
                this.connectionString = connectionString;
        }

        /// <summary>
        /// Sends a non-query command to the SQL Server that connectionString points to with a default command timeout of 30 seconds.
        /// </summary>
        /// <param name="commandToSend">The command to be sent</param>
        /// <returns>Whether the command is sent properly</returns>
        public bool SendCommand(string commandToSend)
        {
            return SendCommand(commandToSend, 30);
        }

        /// <summary>
        /// Sends a non-query command to the SQL Server that connectionString points to with a default command timeout of 30 seconds.
        /// </summary>
        /// <param name="commandToSend">The command to be sent</param>
        /// <param name="commandTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>Whether the command is sent properly</returns>
        public bool SendCommand(string commandToSend, int commandTimeout)
        {
            return SendCommand(new string[] { commandToSend });
        }

        /// <summary>
        /// Sends a non-query command to the SQL Server that connectionString points to with a default command timeout of 30 seconds.
        /// Everything after element 0 is a parameter. They should be in the format of @[number] starting at zero.
        /// </summary>
        /// <param name="commandsToSend">The command to be sent, as well as the parameters</param>
        /// <param name="commandTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>Whether the command is sent properly</returns>
        public bool SendCommand(string[] commandsToSend, int commandTimeout = 30)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the components of sending requests
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

                // Prepare command
                command.Connection = connection;
                command.Transaction = transaction;

                // Attempt to send the command
                try
                {
                    command.CommandText = commandsToSend[0];
                    command.CommandTimeout = commandTimeout;

                    for (int i = 1; i < commandsToSend.Length; i++)
                    {
                        if (commandsToSend[i] != null)
                            command.Parameters.AddWithValue("@" + (i-1), commandsToSend[i]);
                        else
                            command.Parameters.AddWithValue("@" + (i - 1), System.Data.SqlTypes.SqlString.Null);
                    }

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the results of a query as a object[,].
        /// </summary>
        /// <param name="query">The given query, with parameters</param>
        /// <param name="connectionTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>The resulting table, as objects</returns>
        public object[,] GetQuery(string query, int connectionTimeout = 30)
        {
            return GetQuery(new string[] { query });
        }

        /// <summary>
        /// Get the results of a query as an object[,]
        /// Everything after element 0 is a parameter. They should be in the format of @[number] starting at zero.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionTimeout"></param>
        /// <returns>the resulting table, as objects</returns>
        public object[,] GetQuery(string[] query, int connectionTimeout = 30)
        {
            object[,] toReturn;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the components of sending requests
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

                // Prepare command
                command.Connection = connection;
                command.Transaction = transaction;

                // Attempt to send the query
                try
                {
                    command.CommandText = query[0];
                    command.CommandTimeout = connectionTimeout;

                    // Get parameters
                    for (int i = 1; i < query.Length; i++)
                    {
                        command.Parameters.AddWithValue("@" + (i-1), query[i]);
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    List<object[]> objects = new List<object[]>();
                    int currentPos = 0;

                    // Read the data into the objects list
                    while (reader.Read())
                    {
                        objects.Add(new object[reader.FieldCount]);
                        for (int i = 0; i < objects[0].Length; i++)
                        {
                            objects[currentPos][i] = reader[i];
                        }
                        currentPos++;
                    }

                    toReturn = new object[objects.Count, objects[0].Length];
                    for (int i = 0; i < objects.Count; i++)
                    {
                        for (int j = 0; j < objects[0].Length; j++)
                        {
                            toReturn[i, j] = objects[i][j];
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Gets all the columns of a UserData query
        /// </summary>
        /// <param name="query">The given query</param>
        /// <param name="connectionTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>The table of UserDatas</returns>
        public UserData[] GetUserDataQuery(string query, int connectionTimeout = 30)
        {
            return GetUserDataQuery(new string[] { query });
        }
        
        /// <summary>
        /// Gets all the columns of a UserData query.
        /// Everything after element 0 is a parameter. They should be in the format of @[number] starting at zero.
        /// </summary>
        /// <param name="query">The given query, with parameters</param>
        /// <param name="connectionTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>The table of UserDatas</returns>
        public UserData[] GetUserDataQuery(string[] query, int connectionTimeout = 30)
        {
            List<UserData> toReturn = new List<UserData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the components of sending requests
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

                // Prepare command
                command.Connection = connection;
                command.Transaction = transaction;

                // Attempt to send the command
                try
                {
                    command.CommandText = query[0];
                    command.CommandTimeout = connectionTimeout;

                    // Get parameters
                    for (int i = 1; i < query.Length; i++)
                    {
                        command.Parameters.AddWithValue("@" + (i-1), query[i]);
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserData toAdd = new UserData();
                        toAdd.id = reader.GetInt32(0);
                        toAdd.firstName = reader.GetString(1);
                        toAdd.lastName = reader.GetString(2);
                        toAdd.address1 = reader.GetString(3);
                        toAdd.address2 = reader[4] as String;
                        toAdd.address3 = reader[5] as String;
                        toAdd.phoneNumber = reader.GetString(6);
                        toAdd.emailAddress = reader.GetString(7);
                        toAdd.city = reader.GetString(8);
                        toAdd.state = reader.GetString(9);
                        toAdd.zipCode = reader.GetString(10);
                        toAdd.country = reader.GetString(11);

                        toReturn.Add(toAdd);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return toReturn.ToArray();
        }

        /// <summary>
        /// To be used with the Roles table in the TestDatabase. Returns an array filled with the matching Permissions of the query.
        /// </summary>
        /// <param name="query">The given query, with parameters</param>
        /// <param name="connectionTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>The table of Permissions</returns>
        public Permissions.Permissions[] GetPermissionsQuery(string query, int connectionTimeout = 30)
        {
            return GetPermissionsQuery(new string[] { query } );
        }

        /// <summary>
        /// To be used with the Roles table in the TestDatabase. Returns an array filled with the matching Permissions of the query.
        /// </summary>
        /// <param name="query">The given query, with parameters</param>
        /// <param name="connectionTimeout">The amount of time, in seconds, before the command times out</param>
        /// <returns>The table of Permissions</returns>
        public Permissions.Permissions[] GetPermissionsQuery(string[] query, int connectionTimeout = 30)
        {
            List<Permissions.Permissions> toReturn = new List<Permissions.Permissions>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepare the components of sending requests
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

                // Prepare command
                command.Connection = connection;
                command.Transaction = transaction;

                // Attempt to send the command
                try
                {
                    command.CommandText = query[0];
                    command.CommandTimeout = connectionTimeout;

                    // Get parameters
                    for (int i = 1; i < query.Length; i++)
                    {
                        command.Parameters.AddWithValue("@" + (i - 1), query[i]);
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Permissions.Permissions toAdd = new Permissions.Permissions();
                        toAdd.id = reader.GetInt32(0);
                        toAdd.role_name = reader.GetString(1);
                        toAdd.canCreateUser = reader.GetBoolean(2);
                        toAdd.canEditUser = reader.GetBoolean(3);
                        toAdd.canCreateRole = reader.GetBoolean(4);
                        toAdd.canEditRole = reader.GetBoolean(5);
                        toAdd.canAssignRole = reader.GetBoolean(6);

                        if (reader.FieldCount > 2)
                        {
                            // Get the names of the rest of the fields
                            object[,] itemsToPopulate = GetQuery("EXEC [TestDatabase].[dbo].[GetColumnNames] @TableName = N'Roles'");

                            for (int i = 2; i < reader.FieldCount; i++)
                            {
                                toAdd.pagePermissions.Add(new PagePermission { pageName = (itemsToPopulate[i,0] as string), canAccess = reader.GetBoolean(i) });
                            }
                        }

                        toReturn.Add(toAdd);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return toReturn.ToArray();
        }
    }
}