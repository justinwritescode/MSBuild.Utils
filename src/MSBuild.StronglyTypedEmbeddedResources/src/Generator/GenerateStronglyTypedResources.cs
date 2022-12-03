/* 
 * GenerateStronglyTypedResources.cs
 * 
 *   Created: 2022-11-12-03:15:31
 *   Modified: 2022-11-12-03:15:31
 * 
 *   Author: Justin Chase <justin@justinwritescode.com>
 *   
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */ 
namespace StronglyTypedEmbeddedResourcesGenerator;

public class GenerateStronglyTypedResources : MSBTask
{
    [Required]
    public ITaskItem[] EmbeddedResources { get; set; }

    [Required]
    public string OutputDirectory { get; set; }

    [Required]
    public string OutputNamespace { get; set; }

    [Required]
    public string OutputClassName { get; set; }

    public override bool Execute()
    {
        var outputDirectory = new DirectoryInfo(OutputDirectory);
        if (!outputDirectory.Exists)
        {
            outputDirectory.Create();
        }

        var outputFilePath = Path.Combine(OutputDirectory, OutputClassName + ".cs");
        var outputResourceFilePath = Path.Combine(OutputDirectory, OutputClassName + ".resources");
        var outputResourceFile = new FileInfo(outputResourceFilePath);
        if (outputResourceFile.Exists)
        {
            outputResourceFile.Delete();
        }

        var resourceManager = new ResourceManager(OutputNamespace + "." + OutputClassName, Assembly.GetExecutingAssembly());
        var resourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        var resourceWriter = new ResourceWriter(outputResourceFilePath);
        foreach (DictionaryEntry resource in resourceSet)
        {
            resourceWriter.AddResource(resource.Key.ToString(), resource.Value);
        }
        resourceWriter.Generate();
        resourceWriter.Close();

        var resourceNames = resourceSet.Cast<DictionaryEntry>().Select(r => r.Key.ToString()).ToArray();
        var resourceValues = resourceSet.Cast<DictionaryEntry>().Select(r => r.Value.ToString()).ToArray();

        var output = new StringBuilder();
        for (var i = 0; i < resourceNames.Length; i++)
        {
            output.AppendLine($"        public const string {resourceNames[i]} = \"{resourceValues[i]}\";");
        }
        output.AppendLine("    }");
        output.AppendLine("}");

        File.WriteAllText(outputFilePath, output.ToString());

        return true;
    }
}
