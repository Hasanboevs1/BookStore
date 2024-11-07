namespace Book.API.Exceptions;

public class CustomException: Exception
{
    public int Code { get; set; }
    public string Message { get; set; }

    public CustomException(int code, string message) : base(message)
    {
        Code = code;
        Message = message;
    }
}
