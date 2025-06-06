namespace Kolokwium1.Services;

using Kolokwium1.Models.DTOs;

    public interface IDbService
    {
        Task<ClientsRentalHistoryDto> GetRentalsForClientByIdAsync(int id);
        Task AddNewRentalAsync(int id, CreateRentalDto rentalRequest);
    }
