namespace TemplateFramework.Core;

[DataContract(Name = "MultipleContents", Namespace = "http://schemas.datacontract.org/2004/07/TemplateFramework")]
internal sealed class MultipleContents
{
    [DataMember]
    public string BasePath { get; set; } = "";

    [DataMember]
    public List<Contents> Contents { get; set; } = new();

    public const string XmlStringFragment = @"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">";
}
