﻿namespace trembonWoW.Core.Connectors.Auth.Records;

public record Account(uint Id, string Username, string? Email, byte[] Salt, byte[] Verifier);