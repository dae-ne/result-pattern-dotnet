namespace DaeNe.Result;

public readonly struct ServiceResult<TData>
{
    private readonly TData? _data;
    private readonly Exception? _error;

    private ServiceResult(TData data)
    {
        _data = data;
        _error = default;
        IsSuccess = true;
    }

    private ServiceResult(Exception error)
    {
        _data = default;
        _error = error;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }

    public static implicit operator ServiceResult<TData>(TData data) => new(data);

    public static implicit operator ServiceResult<TData>(Exception error) => new(error);

    public TData GetData() => IsSuccess
        ? _data!
        : throw new InvalidOperationException("The result is not successful.");

    public Exception GetError() => !IsSuccess
        ? _error!
        : throw new InvalidOperationException("The result is successful.");
}

public readonly struct ServiceResult
{
    private readonly Exception? _error;

    private ServiceResult(Exception? error)
    {
        _error = error;
        IsSuccess = error is null;
    }

    public bool IsSuccess { get; }

    public static implicit operator ServiceResult(Exception error) => new(error);

    public static ServiceResult Success() => new(default);
}
