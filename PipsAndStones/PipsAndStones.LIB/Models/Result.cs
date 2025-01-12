namespace PipsAndStones.LIB.Models;

public record Result<T>
{
    private readonly bool _isSuccess;
    private readonly T? _value;
    private readonly string? _errorMessage;

    private Result(bool isSuccess, T? value, string? errorMessage)
    {
        _isSuccess = isSuccess;
        _value = value;
        _errorMessage = errorMessage;
    }
    
    public bool IsSuccess() => _isSuccess;
    public T? GetValue() => _value;
    public string? GetErrorMessage() => _errorMessage;
    
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
}