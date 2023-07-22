namespace TemplateFramework.Core.Models;

[DataContract(Name = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/TemplateFramework")]
internal sealed class Contents
{
    [DataMember]
    public string Filename { get; set; } = "";

    [DataMember]
    public List<string> Lines { get; set; } = new();

    [DataMember]
    public bool SkipWhenFileExists { get; set; }
}
