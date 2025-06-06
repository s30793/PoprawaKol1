using Microsoft.VisualBasic;

namespace Kolokwium1.Models.DTOs;

    public class CreateRentalDto
        {
            public int id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string address { get; set; }
            AddCarIdDto carid = new AddCarIdDto();
            AddDatesDto dates = new AddDatesDto();
        }

    public class AddCarIdDto
    {
        public int carId { get; set; }
    }

    public class AddDatesDto
    {
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
    }
    