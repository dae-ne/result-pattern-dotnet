using DaeNe.Result;

namespace Result.Examples.CQRS.Errors;

public sealed class NotFoundError(string message) : ErrorBase(message);
