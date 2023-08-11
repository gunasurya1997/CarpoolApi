namespace CarPoolService.Models.DBModels;

public partial class CarPoolRide
{
    public Guid RideId { get; set; }

    public int? DriverId { get; set; }

    public int? DepartureCityId { get; set; }

    public int? DestinationCityId { get; set; }

    public string? Stops { get; set; }

    public string? TimeSlot { get; set; }

    public string? Date { get; set; }

    public int? AvailableSeats { get; set; }

    public bool? RideStatus { get; set; }

    public string? Fare { get; set; }
}
