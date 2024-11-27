namespace trembonWoW.Core.Connectors.Auth.Models;

public record Account(string Id, string Username, string? Email, byte[] Salt, byte[] Verifier);