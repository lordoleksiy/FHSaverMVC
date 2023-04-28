public class CustomException : Exception
{
    public int StatusCode { get; private set; }
    public string ErrorMessage { get; private set; }

    public CustomException(int statusCode, string errorMessage)
        : base(errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    public CustomException(int statusCode, string errorMessage, Exception innerException)
        : base(errorMessage, innerException)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}
