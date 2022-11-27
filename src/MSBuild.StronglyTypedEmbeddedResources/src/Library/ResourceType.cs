/* 
 * ResourceType.cs
 * 
 *   Created: 2022-10-30-10:39:56
 *   Modified: 2022-11-11-10:41:09
 * 
 *   Author: Justin Chase <justin@justinwritescode.com>
 *   
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */ 

// 
// ResourceType.cs
// 
//   Created: 2022-10-30-10:39:56
//   Modified: 2022-11-02-05:05:10
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

using System.Collections.Generic;
// 
// ResourceType.cs
// 
//   Created: 2022-10-30-07:39:56
//   Modified: 2022-10-30-11:38:46
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

namespace MSBuild.StronglyTypedEmbeddedResources;

using System;
using JustinWritesCode.ComponentModel;


[GenerateEnumerationClass("ResourceType")]
public enum ResourceTypeEnum
{
    [FileExtension(".txt")]
    String = 0,
    [FileExtension(".cs", ".vb")]
    SourceCode,
    [FileExtension(".*")]
    Binary,
    [FileExtension(".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff", ".ico", ".svg")]
    Image,
    [FileExtension(".png")]
    ImagePng,
    [FileExtension(".jpg", ".jpeg")]
    ImageJpeg,
    [FileExtension(".gif")]
    ImageGif,
    [FileExtension(".bmp")]
    ImageBmp,
    [FileExtension(".svg")]
    ImageSVG,
    [FileExtension(".tif", ".tiff")]
    ImageTiff,
    [FileExtension(".ico")]
    ImageIcon,
    [FileExtension(".dll", ".exe")]
    Assembly,
    [FileExtension(".json")]
    Json,
    [FileExtension(".xml")]
    Xml,
    [FileExtension(".yml", ".yaml")]
    Yaml,
    [FileExtension(".csv")]
    Csv,
    [FileExtension(".htm", "html")]
    Html,
    [FileExtension(".resx")]
    ResX
}

public partial class ResourceType
{
    public static ResourceType FromFileExtension(string fileExtension)
    {
        if (fileExtension == null)
            throw new ArgumentNullException(nameof(fileExtension));

        if (fileExtension.StartsWith("."))
            fileExtension = fileExtension.Substring(1);

        foreach (var resourceType in ResourceType.Values)
        {
            if (resourceType.GetCustomAttribute<FileExtensionAttribute>().Value.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                return resourceType;
        }

        return ResourceType.Binary.Instance;
    }
}

// internal class GenerateEnumerationClassAttribute : Attribute
// {
// }
