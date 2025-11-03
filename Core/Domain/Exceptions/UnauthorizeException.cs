namespace Domain.Exceptions
{
    public sealed class UnauthorizeException(string message = "Invalid Email OR Password.") : Exception(message)
    {
    }
}
