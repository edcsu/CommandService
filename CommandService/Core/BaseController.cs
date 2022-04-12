using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CommandService.Core
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {
    }
}
