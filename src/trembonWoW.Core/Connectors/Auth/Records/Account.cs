namespace trembonWoW.Core.Connectors.Auth.Models;

public record Account(uint Id, string Username, string? Email, byte[] Salt, byte[] Verifier);