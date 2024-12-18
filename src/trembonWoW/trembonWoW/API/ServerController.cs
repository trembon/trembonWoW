using Microsoft.AspNetCore.Mvc;
using trembonWoW.Core.Authorization;
using trembonWoW.Core.Connectors.RemoteAccess;

namespace trembonWoW.API
{
    [ApiController]
    [Route("/api/server")]
    [ApiKeyAuthorization]
    public class ServerController(IRemoteAccessSoapAPI remoteAccessSoapAPI) : ControllerBase
    {
        [HttpGet("info")]
        public async Task<IActionResult> GetServerInformationAsync()
        {
            var result = await remoteAccessSoapAPI.GetServerInfo();
            return Ok(result);
        }
    }
}
