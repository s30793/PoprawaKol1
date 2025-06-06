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

    public async Task<CustomerRentalHistoryDto> GetRentalsForCustomerByIdAsync(int id)
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
        CustomerRentalHistoryDto? customerRentalHistoryDto = null;

        while (await reader.ReadAsync())
        {
            if (customerRentalHistoryDto is null)
            {
                customerRentalHistoryDto = new CustomerRentalHistoryDto
                {
                    id = reader.GetInt32(0),
                    firstname = reader.GetString(1),
                    lastname = reader.GetString(2),
                    address = reader.GetString(3),
                    rentals = new List<CarsDto>(){
                        /*vin = reader.GetString(4),
                        color = reader.GetString(5),
                        model = reader.GetString(6),
                        dateFrom = reader.GetDateTime(7),
                        dateTo = reader.GetDateTime(8),
                        totalPrice = reader.GetDecimal(9)*/
                        };
                if         
                        
                        
                        
                        
                };
            }
            }
        
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
            com.CommandText = @"SELECT 1 FROM clients where id = @id";
            com.Parameters.AddWithValue("@id", id);
            
            var clientsIDRes  = await com.ExecuteReaderAsync();
            if (clientsIDRes is not null)
            {
                throw new NotFoundException("Client does exist");
            }
            
            com.Parameters.Clear();
            
            com.CommandText = "Select 1 from "
                
                
                
                
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
            


