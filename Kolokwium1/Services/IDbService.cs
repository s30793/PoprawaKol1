namespace Kolokwium1.Services;

using Kolokwium1.Models.DTOs;

    public interface IDbService
    {
        Task<CustomerRentalHistoryDto> GetRentalsForCustomerByIdAsync(int id);
        Task AddNewRentalAsync(int id, CreateRentalDto rentalRequest);
    }
