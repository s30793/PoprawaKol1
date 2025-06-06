using System.Data.Common;
using Kolokwium1.Exceptions;
namespace Kolokwium1.Services;
using Kolokwium1.Models.DTOs;
using Microsoft.Data.SqlClient;

public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<ClientsRentalHistoryDto> GetRentalsForClientByIdAsync(int id)
    {
        var query =
            @"SELECT c.id, c.firstName, c.lastName, c.address, ca.vin, 
       co.name, m.name, cr.dateFrom, cr.dateTo, cr.totalPrice
        from clients c 
        join car_rentals cr on c.id = cr.id
        join cars ca on cr.id = ca.id
        join models m on ca.id = m.id
        join colors co on ca.id = co.id
        where c.id = @id";

        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        com.CommandText = query;
        await conn.OpenAsync();

        com.Parameters.AddWithValue("@id", id);
        var reader = await com.ExecuteReaderAsync();
        ClientsRentalHistoryDto? customerRentalHistoryDto = null;

        while (await reader.ReadAsync())
        {
            if (customerRentalHistoryDto is null)
            {
                customerRentalHistoryDto = new ClientsRentalHistoryDto
                {
                    id = reader.GetInt32(0),
                    firstname = reader.GetString(1),
                    lastname = reader.GetString(2),
                    address = reader.GetString(3),
                    rentals = new List<CarsDto>()
                    {
                        /*vin = reader.GetString(4),
                        color = reader.GetString(5),
                        model = reader.GetString(6),
                        dateFrom = reader.GetDateTime(7),
                        dateTo = reader.GetDateTime(8),
                        totalPrice = reader.GetDecimal(9)*/
                    },
                    //  cars = new List<CarsDto>()
                };
                if (customerRentalHistoryDto is null)
                {
                    throw new NotFoundException("Clients not found");
                }
            }
        }return customerRentalHistoryDto;
    }

    public async Task AddNewRentalAsync(int id, CreateRentalDto rentalRequest)
    {

        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using SqlCommand com = new SqlCommand();

        com.Connection = conn;
        await conn.OpenAsync();

        DbTransaction transaction = await conn.BeginTransactionAsync();
        com.Transaction = transaction as SqlTransaction;

        try
        {
            com.Parameters.Clear();
            com.CommandText = @"SELECT 1 FROM clients where id = @ID";
            com.Parameters.AddWithValue("@id", id);
            
            var clientsIDRes  = await com.ExecuteReaderAsync();
            if (clientsIDRes is null)
            {
                throw new NotFoundException("Client does not exist");
            }
            
            com.Parameters.Clear();
            
            com.CommandText = "Select 1 from cars where id = @ID";
            
            var carsIDRes = await com.ExecuteReaderAsync();
            if (carsIDRes is null) throw new NotFoundException("Cars does not exist");
            
            com.Parameters.Clear();
            
            com.CommandText = @"SELECT 1 from car_rentals where id = @ID";
            
            var rentalsIDRes = await com.ExecuteReaderAsync();
            if (rentalsIDRes is null)
                throw new NotFoundException("Rental does not exist");
            
            com.Parameters.Clear();
            
            com.CommandText = @"SELECT 1 from models where id = @ID";
            
            var modelsIDRes = await com.ExecuteReaderAsync();
            if (modelsIDRes is null)
                throw new NotFoundException("Model does not exist");
            
            com.Parameters.Clear();
            
            com.CommandText = @"SELECT 1 from models where id = @ID";
            
            var colorsIDRes = await com.ExecuteReaderAsync();
            if (colorsIDRes is null)
                throw new NotFoundException("Color does not exist");
            
            com.Parameters.Clear();


            com.CommandText = @"Insert into cars_rental 
                Values(@ID, @ClientID, @CarID )";
            com.Parameters.AddWithValue("@ID", rentalsIDRes);
            com.Parameters.AddWithValue("@ClientID", clientsIDRes);
            com.Parameters.AddWithValue("@CarID", carsIDRes);
            
            
            try
            {
                await com.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw new ConflictException("A rental already exists.");
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
            


