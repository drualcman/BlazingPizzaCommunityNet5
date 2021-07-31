using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Server.Controllers
{
    public class OidcConfigurationController : Controller
    {
        readonly ILogger<OidcConfigurationController> Logger;
        readonly IClientRequestParametersProvider ClientRequestParametersProvider;

        public OidcConfigurationController(IClientRequestParametersProvider provider, ILogger<OidcConfigurationController> logger)
            => (ClientRequestParametersProvider, Logger) = (provider, logger);

        [HttpGet("_configuration/{clientid}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
