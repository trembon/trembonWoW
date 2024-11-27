namespace trembonWoW.Core.Connectors.Auth.Models;

public record AccountAuthenticationRecord(string Username, byte[] Salt, byte[] Verifier);