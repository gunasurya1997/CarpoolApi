namespace CarPoolService.Models.DBModels;
public partial class Booking
{
    public Guid BookingId { get; set; }

    public Guid RideId { get; set; }

    public int PassengerId { get; set; }

    public int PickupLocationId { get; set; }

    public int DropLocationId { get; set; }

    public string TimeSlot { get; set; } = null!;

    public DateTime Date { get; set; }

    public int ReservedSeats { get; set; }

    public string Fare { get; set; } = null!;
}
