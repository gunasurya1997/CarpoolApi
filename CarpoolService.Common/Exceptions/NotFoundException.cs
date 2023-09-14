namespace CarpoolService.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? customMessage = "User not found") : base(customMessage) { }
    }
}
