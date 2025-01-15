using Microsoft.Data.SqlClient;

namespace ColorsManagement.Db
{
    public class DatabaseSetup
    {
        private readonly string _connectionString;

        public DatabaseSetup(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        public void CreateDatabaseAndTables()
        {
            
            var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "" 
            };

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var createDbQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'colors_db')
                    CREATE DATABASE colors_db;
                ";
                using (var command = new SqlCommand(createDbQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

           
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Colors' AND xtype='U')
                    CREATE TABLE Colors (
                        Id UNIQUEIDENTIFIER PRIMARY KEY,
                        ColorName NVARCHAR(100) NOT NULL,
                        Price DECIMAL(18, 2) NOT NULL,
                        DisplayOrder INT NOT NULL UNIQUE,
                        InStock BIT NOT NULL
                    );
                ";
                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
