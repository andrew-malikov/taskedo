using FluentResults;
using FluentValidation.Results;

namespace Taskedo.Tasks.Application;

public interface ICommandResult
{
    public class Success : ICommandResult
    {

    }

    public class Success<TData> : ICommandResult
    {
        public required TData Data { init; get; }
    }

    public class InternalError : ICommandResult
    {
        public required IEnumerable<IError> Errors { init; get; }

        public static InternalError Of(string message) => new() { Errors = new[] { new Error(message) } };
    }

    public class ValidationError : ICommandResult
    {
        public required IEnumerable<ValidationFailure> Errors { init; get; }

        public static ValidationError Of(ValidationFailure error) => new() { Errors = new List<ValidationFailure> { error } };
    }

    public class NotFound : ICommandResult
    {

    }
};
