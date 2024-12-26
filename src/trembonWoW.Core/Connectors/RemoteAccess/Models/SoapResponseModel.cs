using System.Xml.Serialization;

namespace trembonWoW.Core.Connectors.RemoteAccess.Models;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[System.Xml.Serialization.XmlRootAttribute(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class SoapResponse
{
    public SoapResponseBody Body { get; set;}
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class SoapResponseBody
{
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:AC")]
    public ExecuteCommandResponse executeCommandResponse { get; set; }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:AC")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:AC", IsNullable = false)]
public partial class ExecuteCommandResponse
{
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public string result { get; set; }
}

