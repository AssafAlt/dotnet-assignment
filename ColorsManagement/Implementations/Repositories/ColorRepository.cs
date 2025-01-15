using ColorsManagement.Dtos;
using ColorsManagement.Interfaces.Repositories;
using ColorsManagement.Models;
using Microsoft.Data.SqlClient;

namespace ColorsManagement.Implementations.Repositories
{
    public class ColorRepository : IColorRepository
    {

        private readonly string _connectionString;

        public ColorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddColorAsync(Color color)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "INSERT INTO Colors (Id,ColorName, Price, DisplayOrder, InStock) VALUES (@Id,@ColorName, @Price, @DisplayOrder, @InStock)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", color.Id);
                    cmd.Parameters.AddWithValue("@ColorName", color.ColorName);
                    cmd.Parameters.AddWithValue("@Price", color.Price);
                    cmd.Parameters.AddWithValue("@DisplayOrder", color.DisplayOrder);
                    cmd.Parameters.AddWithValue("@InStock", color.InStock);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Color>> GetAllAsync()
        {
            var colors = new List<Color>();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT * FROM Colors";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var color = new Color
                            {

                                Id = Guid.TryParse(reader["Id"].ToString(), out var id) ? id : Guid.NewGuid(),
                                ColorName = reader["ColorName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                                InStock = Convert.ToBoolean(reader["InStock"])
                            };

                            colors.Add(color);
                        }
                    }
                }
            }

            return colors;
        }

        public async Task<bool> IsColorExistByIdAsync(Guid colorId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT COUNT(1) FROM Colors WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", colorId);

                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        public async Task RemoveColorByIdAsync(Guid colorId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();


                string query = "DELETE FROM Colors WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@Id", colorId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateColorAsync(Color color)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();


                string query = @"
            UPDATE Colors
            SET 
                ColorName = @ColorName,
                Price = @Price,
                DisplayOrder = @DisplayOrder,
                InStock = @InStock
            WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@Id", color.Id);
                    cmd.Parameters.AddWithValue("@ColorName", color.ColorName);
                    cmd.Parameters.AddWithValue("@Price", color.Price);
                    cmd.Parameters.AddWithValue("@DisplayOrder", color.DisplayOrder);
                    cmd.Parameters.AddWithValue("@InStock", color.InStock);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateDisplayOrdersAsync(List<UpdateDisplayOrderDto> listDto)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var transaction = conn.BeginTransaction();

                try
                {
                    
                    string randomUpdateQuery = "UPDATE Colors SET DisplayOrder = ABS(CHECKSUM(NEWID()))";

                    using (var cmd = new SqlCommand(randomUpdateQuery, conn, transaction))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    
                    foreach (var item in listDto)
                    {
                        string updateQuery = "UPDATE Colors SET DisplayOrder = @DisplayOrder WHERE Id = @Id";

                        using (var cmd = new SqlCommand(updateQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", item.Id);
                            cmd.Parameters.AddWithValue("@DisplayOrder", item.DisplayOrder);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    
                    throw new Exception("Error occurred while updating the display order.", ex);
                }
            }

        }
    }
}
