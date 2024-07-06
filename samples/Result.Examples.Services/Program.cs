using Result.Examples.Services;

const int dividend = 10;
const int divisor = 0;

var service = new Service();

// The result is a ServiceResult<int>. In this case, the divisor is zero, so the method
// returns a failed result with no data. It returns an exception that can be accessed using
// the GetError method.
var result = service.Divide(dividend, divisor);

Console.WriteLine(result.IsSuccess
    ? $"The result is {result.GetData()}."
    : $"An error occurred: {result.GetError().Message}");
