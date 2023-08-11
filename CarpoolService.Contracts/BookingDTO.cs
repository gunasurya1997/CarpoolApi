namespace CarpoolService.Contracts
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }

        public Guid RideId { get; set; }

        public int PassengerId { get; set; }

        public string? PassengerName { get; set; }

        public int PickupLocationId { get; set; }

        public int DropLocationId { get; set; }

        public string TimeSlot { get; set; } = null!;

        public string Date { get; set; } = null!;

        public int ReservedSeats { get; set; }

        public string Fare { get; set; }
    }
}
