namespace DaeNe.Result;

public abstract class ErrorBase(string message)
{
    public string Message { get; } = message;
}
