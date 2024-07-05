namespace DaeNe.Result;

/// <summary>
/// Represents the result returned from an operation.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
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

    /// <summary>
    /// Gets a value indicating whether this instance is success.
    /// </summary>
    /// <value><c>true</c> if operation succeeded; otherwise, <c>false</c>.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is failure.
    /// </summary>
    /// <value><c>true</c> if operation failed; otherwise, <c>false</c>.</value>
    public bool IsFailure => !IsSuccess;

    public TData Data => _data!;

    public Exception Error => _error!;

    public static implicit operator ServiceResult<TData>(TData data) => new(data);

    public static implicit operator ServiceResult<TData>(Exception error) => new(error);

    /// <summary>
    /// Matches the result and returns the appropriate value.
    /// </summary>
    /// <param name="onSuccess">The data returned when the operation is successful.</param>
    /// <param name="onFailure">The error returned when the operation is unsuccessful.</param>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>The result.</returns>
    public TResult Match<TResult>(
        Func<TData, TResult> onSuccess,
        Func<Exception, TResult> onFailure)
        => IsSuccess ? onSuccess(_data!) : onFailure(_error!);
}
