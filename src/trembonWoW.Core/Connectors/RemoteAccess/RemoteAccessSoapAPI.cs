using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using trembonWoW.Core.Connectors.RemoteAccess.Models;
using trembonWoW.Core.Connectors.RemoteAccess.Records;

namespace trembonWoW.Core.Connectors.RemoteAccess
{
    public interface IRemoteAccessSoapAPI
    {
        Task<ServerInfo?> GetServerInfo();

        Task<bool> ChangeAccountPassword(string username, string newPassword);

        Task<bool> CharacterLevel(string characterName, int level);

        Task<bool> TeleportCharacter(string characterName, string location);

        Task<bool> SendMoney(string characterName, int amount, string subject, string text);
    }

    public partial class RemoteAccessSoapAPI(IHttpClientFactory httpClientFactory) : IRemoteAccessSoapAPI
    {
        private const string HTTP_CLIENT_NAME = "soap";

        [GeneratedRegex("rev\\. [a-z0-9+]* (\\d{4}-\\d{2}-\\d{2})", RegexOptions.IgnoreCase | RegexOptions.Multiline, "sv-SE")]
        private static partial Regex RevisionRegex();

        [GeneratedRegex("Connected players: ([0-9]*)\\.", RegexOptions.IgnoreCase | RegexOptions.Multiline, "sv-SE")]
        private static partial Regex ConnectedRegex();

        public async Task<ServerInfo?> GetServerInfo()
        {
            string? data = await ExecuteCommand("server info");
            if (data == null)
                return null;

            var revMatch = RevisionRegex().Match(data);
            var onlineMatch = ConnectedRegex().Match(data);

            return new ServerInfo(revMatch.Groups[1].Value, int.Parse(onlineMatch.Groups[1].Value));
        }

        // AC> account set password dev password password
        // AC> The password was changed

        public async Task<bool> ChangeAccountPassword(string username, string newPassword)
        {
            string? data = await ExecuteCommand($"account set password {username} {newPassword} {newPassword}");
            return data?.StartsWith("The password was changed", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        //AC> character level Brock 58
        //AC> You changed level of Brock to 58.

        public async Task<bool> CharacterLevel(string characterName, int level)
        {
            string? data = await ExecuteCommand($"character level {characterName} {level}");
            return data?.Contains($"You changed level of {characterName} to {level}", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }


        //AC> teleport name Brock stormwind
        //AC> You are teleporting Brock(offline) to Stormwind.
        public async Task<bool> TeleportCharacter(string characterName, string location)
        {
            string? data = await ExecuteCommand($"teleport name {characterName} {location}");
            return data?.Contains($"You are teleporting {characterName}", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }


        // AC> send money Brock "Boost money" "Money to get you started. Should cover class spell, mount and some gear on the auction house." 15000000
        // AC> Mail sent to Brock

        public async Task<bool> SendMoney(string characterName, int amount, string subject, string text)
        {
            string? data = await ExecuteCommand($"send money {characterName} \"{subject}\" \"{text}\" {amount}");
            return data?.Contains($"Mail sent to {characterName}", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }


        private async Task<string?> ExecuteCommand(string command)
        {
            var client = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);

            var request = SoapRequest.Create(command);
            string requestString = SerializeToXml(request);

            var httpContent = new StringContent(requestString, Encoding.UTF8, "application/xml");
            var httpResponse = await client.PostAsync("/", httpContent);

            string responseString = await httpResponse.Content.ReadAsStringAsync();
            var result = SerializeFromXml(responseString);

            return result?.Body?.executeCommandResponse?.result;
        }

        private static string SerializeToXml<T>(T request)
        {
            XmlSerializer serializer = new(typeof(T));

            using StringWriter textWriter = new();
            using XmlWriter writer = XmlWriter.Create(textWriter);
            serializer.Serialize(writer, request);

            return textWriter.ToString();
        }

        private static SoapResponse? SerializeFromXml(string responseText)
        {
            XmlSerializer serializer = new(typeof(SoapResponse));

            using StringReader reader = new(responseText);
            var response = serializer.Deserialize(reader) as SoapResponse;

            return response;
        }
    }
}