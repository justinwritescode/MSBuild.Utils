// 
// Constants.cs
// 
//   Created: 2022-10-30-07:33:50
//   Modified: 2022-10-30-07:33:51
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 
namespace MSBuild.StronglyTypedEmbeddedResources;

public static class Constants
{
    public const string ClassNamePlaceholder = "CLASS_NAME";
    public const string NamespacePlaceholder = "NAMESPACE";
    public const string AuthorNamePlaceholder = "AUTHOR_NAME";
    public const string AuthorEmailPlaceholder = "AUTHOR_EMAIL";
    public const string TimestampPlaceholder = "TIMESTAMP";
    public const string YearPlaceholder = "YEAR";
    public const string NamePlaceholder = "NAME";
    public const string LicenseUrlPlaceholder = "LICENSE_URL";
    public const string LicensePlaceholder = "LICENSE";
    public const string FilenamePlaceholder = "FILENAME";

    public static readonly string CodeHeader = 
        new StreamReader(typeof(Constants).Assembly.GetManifestResourceStream("MSBuild.StronglyTypedEmbeddedResourcesGenerator.Resources.CodeHeader.cs")!).ReadToEnd();

    public const string GenerateStronglyTypedResourcesAttributeName = "GenerateStronglyTypedResourcesAttribute";

    public static readonly string StronglyTypedResourceTemplate = 
        new StreamReader(typeof(Constants).Assembly.GetManifestResourceStream("MSBuild.StronglyTypedEmbeddedResourcesGenerator.Resources.StronglyTypedEmbeddedResourceTemplate.cs")!).ReadToEnd();

    public static readonly string GenerateStronglyTypedResourcesAttributeDeclaration = 
        new StreamReader(typeof(Constants).Assembly.GetManifestResourceStream("MSBuild.StronglyTypedEmbeddedResourcesGenerator.Resources.GenerateStronglyTypedResourcesAttribute.cs")!).ReadToEnd();
}
