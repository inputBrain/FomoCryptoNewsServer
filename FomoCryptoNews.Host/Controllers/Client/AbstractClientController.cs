using FomoCryptoNews.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FomoCryptoNews.Host.Controllers.Client;


[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]/[action]")]
public abstract class AbstractClientController<T> : AbstractController<T> where T : ControllerBase
{
    protected AbstractClientController(PostgreSqlContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
}