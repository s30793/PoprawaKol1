namespace Kolokwium1.Models.DTOs;

public class ClientsRentalHistoryDto
{
    public int id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string address { get; set; }
    public List<CarsDto> rentals = new();
}

    public class CarsDto
    {
        public string vin { get; set; }

    }

    public class ModelsDto
    {
        public string name { get; set; }
    }

    public class CarRentalsDto
    {
        
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
        public int totalprice { get; set; }
        
    }

    public class ColorsDto
    {
        public string name { get; set; }
    }
