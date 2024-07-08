namespace DaeNe.Result;

public readonly struct ServiceResult<TData>
{
    private ServiceResult(TData data)
    {
        Data = data;
        Error = default;
    }

    private ServiceResult(Exception error)
    {
        Data = default;
        Error = error;
    }

    public TData? Data { get; }

    public Exception? Error { get; }

    public bool IsSuccess => Error is null;

    public static implicit operator ServiceResult<TData>(TData data) => new(data);

    public static implicit operator ServiceResult<TData>(Exception error) => new(error);
}

public readonly struct ServiceResult
{
    private ServiceResult(Exception? error)
    {
        Error = error;
    }

    public Exception? Error { get; }

    public bool IsSuccess => Error is null;

    public static implicit operator ServiceResult(Exception error) => new(error);

    public static ServiceResult Success() => new(null);
}
