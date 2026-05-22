using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Web.Controllers.Api.Base
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Value is null) return NoContent();
                if (result.Value is bool b && b) return Ok();
                return Ok(result.Value);
            }

            return result.Error.Code switch
            {
                "NotFound" => NotFound(new { error = result.Error.Message }),
                "Forbidden" => StatusCode(403, new { error = result.Error.Message }),
                "Conflict" => Conflict(new { error = result.Error.Message }),
                "Invalid" => BadRequest(new { error = result.Error.Message }),
                _ => StatusCode(500, new { error = result.Error.Message ?? "伺服器內部錯誤" })
            };
        }
    }
}