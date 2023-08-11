namespace CarpoolService.Contracts
{
    public class CarPoolRideDTO
    {
        public string? DriverName { get; set; }
        public int DriverId { get; set; }

        public Guid RideId { get; set; }

        public int DepartureCityId { get; set; }

        public int DestinationCityId { get; set; }

        public string Stops { get; set; } = null!;

        public string TimeSlot { get; set; } = null!;

        public string Date { get; set; } = null!;

        public int AvailableSeats { get; set; }

        public bool RideStatus { get; set; }

        public string Fare { get; set; }
    }
}
