using Microsoft.AspNetCore.Mvc;
using trembonWoW.Core.Authorization;
using trembonWoW.Core.Connectors.RemoteAccess;
using trembonWoW.Core.Services;

namespace trembonWoW.API;

[ApiController]
[Route("/api/server")]
[ApiKeyAuthorization]
public class ServerController(IServerService serverService) : ControllerBase
{
    [HttpGet("info")]
    public async Task<IActionResult> GetServerInformationAsync()
    {
        var result = await serverService.GetRunningInformation();
        return Ok(result);
    }
}
