using HSX.Contract.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSX.API.Controllers;

[EnableCors(Constants.CorsPolicyNames.HSXPolicy)]
[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<T> : ControllerBase where T : class
{
    public ILogger<T> _logger { get; set; }
    public BaseController(ILogger<T> logger)
    {
        _logger = logger;
    }
}
