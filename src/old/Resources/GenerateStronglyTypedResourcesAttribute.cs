// 
#nullable enable
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
internal class GenerateStronglyTypedResourcesAttribute : Attribute
{
    public GenerateStronglyTypedResourcesAttribute(string? namespaceName = null, string? className = null, string? authorName = null, string ?authorEmail = null)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        AuthorName = authorName;
        AuthorEmail = authorEmail;
    }

    public string? NamespaceName { get; }
    public string? ClassName { get; }
    public string? AuthorName { get; }
    public string? AuthorEmail { get; }
}
