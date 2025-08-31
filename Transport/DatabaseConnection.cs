using MySql.Data.MySqlClient;
using System.Data;

namespace Transport
{
    public class DatabaseConnection
    {
        private readonly string connectionString = "server=localhost;user id=root;password=12345;database=TransportDB;";
        private MySqlConnection? connection;

        public DatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        // Open database connection
        public bool OpenConnection()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", "Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Close database connection
        public bool CloseConnection()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error closing connection: {ex.Message}", "Connection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Execute SELECT queries and return DataTable
        public DataTable ExecuteSelect(string query, MySqlParameter[]? parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (OpenConnection())
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    CloseConnection();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error executing SELECT query: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        // Execute INSERT, UPDATE, DELETE queries
        public bool ExecuteNonQuery(string query, MySqlParameter[]? parameters = null)
        {
            try
            {
                if (OpenConnection())
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        CloseConnection();
                        return rowsAffected > 0;
                    }
                }
                return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Execute scalar queries (returns single value)
        public object? ExecuteScalar(string query, MySqlParameter[]? parameters = null)
        {
            try
            {
                if (OpenConnection())
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        object result = command.ExecuteScalar();
                        CloseConnection();
                        return result;
                    }
                }
                return null;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error executing scalar query: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Get connection for advanced operations
        public MySqlConnection? GetConnection()
        {
            return connection;
        }

        // Dispose resources
        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}