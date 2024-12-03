using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Core.Connectors.Characters.Records;

public record Character(uint Guid, uint Account, string Name, byte Level, byte Race, byte Class);