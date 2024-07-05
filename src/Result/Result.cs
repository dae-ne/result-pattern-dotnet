namespace DaeNe.Result;

/// <summary>
/// Represents the result returned from an operation.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public readonly struct Result<TData>
{
    private readonly TData? _data;
    private readonly ErrorBase? _error;

    private Result(TData data)
    {
        _data = data;
        _error = default;
        IsFailure = false;
    }

    private Result(ErrorBase error)
    {
        _data = default;
        _error = error;
        IsFailure = true;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is failure.
    /// </summary>
    /// <value><c>true</c> if operation failed; otherwise, <c>false</c>.</value>
    private bool IsFailure { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is success.
    /// </summary>
    /// <value><c>true</c> if operation succeeded; otherwise, <c>false</c>.</value>
    private bool IsSuccess => !IsFailure;

    public static implicit operator Result<TData>(TData data) => new(data);

    public static implicit operator Result<TData>(ErrorBase error) => new(error);

    /// <summary>
    /// Matches the result and returns the appropriate value.
    /// </summary>
    /// <param name="onSuccess">The data returned when the operation is successful.</param>
    /// <param name="onFailure">The error returned when the operation is unsuccessful.</param>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>The result.</returns>
    public TResult Match<TResult>(
        Func<TData, TResult> onSuccess,
        Func<ErrorBase, TResult> onFailure)
        => IsSuccess ? onSuccess(_data!) : onFailure(_error!);
}
