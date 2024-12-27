using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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