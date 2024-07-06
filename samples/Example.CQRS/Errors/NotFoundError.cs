using DaeNe.Result.Errors;

namespace Example.CQRS.Errors;

public sealed class NotFoundError(string message) : ErrorBase(message);
