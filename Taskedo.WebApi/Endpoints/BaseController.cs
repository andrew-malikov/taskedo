using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Taskedo.Tasks.Application;

namespace Taskedo.WebApi.Endpoints;

public class BaseController : ControllerBase
{
    public ActionResult ToActionResult<TData>(ICommandResult result, ILogger logger)
    {
        if (result is ICommandResult.InternalError internalError)
        {
            foreach (var error in internalError.Errors)
            {
                foreach (ExceptionalError causedByExceptionalError in error.Reasons.OfType<ExceptionalError>())
                {
                    logger.LogError(causedByExceptionalError.Exception, error.Message);
                }
            }
        }

        return result switch
        {
            ICommandResult.Success => Ok(),
            ICommandResult.Success<TData> success => Ok(success.Data),
            ICommandResult.NotFound => NotFound(),
            ICommandResult.ValidationError error => BadRequest(error.Errors),
            ICommandResult.InternalError => StatusCode(StatusCodes.Status500InternalServerError),
            _ => throw new NotImplementedException(),
        };
    }
}
