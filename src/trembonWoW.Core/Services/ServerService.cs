using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.RemoteAccess;
using trembonWoW.Core.Connectors.RemoteAccess.Records;

namespace trembonWoW.Core.Services;

public interface IServerService
{
    Task<ServerInfo> GetRunningInformation();
}

public class ServerService(IRemoteAccessSoapAPI remoteAccessSoapAPI) : IServerService
{
    public async Task<ServerInfo> GetRunningInformation()
    {
        var result = await remoteAccessSoapAPI.GetServerInfo();
        if (result == null)
            return new ServerInfo("unknown", -1);

        return result;
    }
}
