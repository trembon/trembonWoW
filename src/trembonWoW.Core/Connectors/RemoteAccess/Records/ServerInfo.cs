using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Core.Connectors.RemoteAccess.Records;

public record ServerInfo(string ServerVersion, int PlayersConnected);
