using DaeNe.Result;

namespace Result.Examples.Services;

public sealed class Service
{
    public ServiceResult<int> Divide(int dividend, int divisor)
    {
        // The divisor cannot be zero. The function returns a failed result with no data,
        // just an exception.
        if (divisor == 0)
        {
            // The exception is wrapped in a ServiceResult using an implicit operator.
            // It makes it more explicit and faster than throwing the exception.
            return new InvalidOperationException("The divisor cannot be zero.");
        }

        // If the divisor is not zero, the function returns a successful result. Again, the
        // result is wrapped in a ServiceResult using an implicit operator.
        return dividend / divisor;
    }
}
