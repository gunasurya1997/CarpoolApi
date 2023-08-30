namespace CarPoolService.Models.DBModels;
public partial class CarPoolRide
{
    public Guid RideId { get; set; }

    public int DriverId { get; set; }

    public int DepartureCityId { get; set; }

    public int DestinationCityId { get; set; }

    public string Stops { get; set; } = null!;

    public string TimeSlot { get; set; } = null!;

    public DateTime Date { get; set; }

    public int AvailableSeats { get; set; }

    public bool RideStatus { get; set; }

    public string Fare { get; set; } = null!;
}
