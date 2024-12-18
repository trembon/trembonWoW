using System.Xml.Serialization;

namespace trembonWoW.Core.Connectors.RemoteAccess.Models;

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[System.Xml.Serialization.XmlRootAttribute(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class SoapRequest
{
    public SoapRequestBody Body { get; set; }

    public SoapRequest()
    {
        Body = new SoapRequestBody();
    }

    public SoapRequest(string command)
    {
        Body = new SoapRequestBody(command);
    }

    public static SoapRequest Create(string command) => new(command);
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class SoapRequestBody
{
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:AC")]
    public SoapRequestExecuteCommand executeCommand { get; set; }

    public SoapRequestBody()
    {
        executeCommand = new SoapRequestExecuteCommand();
    }

    public SoapRequestBody(string command)
    {
        executeCommand = new SoapRequestExecuteCommand(command);
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:AC")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:AC", IsNullable = false)]
public partial class SoapRequestExecuteCommand
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public string command { get; set; }

    public SoapRequestExecuteCommand()
    {
        command = "";
    }

    public SoapRequestExecuteCommand(string command)
    {
        this.command = command;
    }
}

