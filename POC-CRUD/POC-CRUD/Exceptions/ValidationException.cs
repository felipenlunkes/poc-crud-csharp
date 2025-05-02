namespace POC_CRUD.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
        
    }
}