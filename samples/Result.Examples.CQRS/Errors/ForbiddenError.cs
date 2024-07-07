using DaeNe.Result;

namespace Result.Examples.CQRS.Errors;

public sealed class ForbiddenError(string message) : ErrorBase(message);
