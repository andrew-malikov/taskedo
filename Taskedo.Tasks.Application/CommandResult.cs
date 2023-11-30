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
    }

    public class ValidationError : ICommandResult
    {
        public required IEnumerable<ValidationFailure> Errors { init; get; }
    }

    public class NotFound : ICommandResult
    {

    }
};
