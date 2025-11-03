using Microsoft.AspNetCore.Http;
using Shared.ErrorDetails;
using Shared.ErrorModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]

    public class ApiController : ControllerBase
    {
    }
}
