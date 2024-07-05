namespace DaeNe.Result;

/// <summary>
/// Represents the error base used in the result pattern.
/// </summary>
public abstract class ErrorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorBase"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    protected ErrorBase(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value>The error message.</value>
    public string Message { get; }
}
