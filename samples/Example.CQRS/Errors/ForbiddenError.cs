using DaeNe.Result.Errors;

namespace Example.CQRS.Errors;

public sealed class ForbiddenError(string message) : ErrorBase(message);
