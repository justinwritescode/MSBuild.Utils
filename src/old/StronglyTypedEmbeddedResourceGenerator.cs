/* 
 * StronglyTypedEmbeddedResourceGenerator.cs
 * 
 *   Created: 2022-10-30-10:06:22
 *   Modified: 2022-11-11-10:42:51
 * 
 *   Author: Justin Chase <justin@justinwritescode.com>
 *   
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */ 

// 
// StronglyTypedEmbeddedResourceGenerator.cs
// 
//   Created: 2022-10-30-10:06:22
//   Modified: 2022-11-04-10:17:36
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using Microsoft.Build.Execution;
using Microsoft.Build.Definition;
using static MSBuild.StronglyTypedEmbeddedResources.Constants;

[Generator]
public class StronglyTypedEmbeddedResourceGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator
{
    private IEnumerable<AdditionalText> _additionalFiles = Array.Empty<AdditionalText>();
    private const string License = "MIT";
    public void GenerateSource(SourceProductionContext context, ImmutableArray<AdditionalText> additionalFiles)
    {
        try
        {
            context.AddSource(GenerateStronglyTypedResourcesAttributeName + ".g.cs", GenerateStronglyTypedResourcesAttributeDeclaration);

            context.AddSource("AdditionalFiles.g.cs", $"/* Additional Files ({additionalFiles.Count()}): {string.Join(", ", additionalFiles.Select(af => af.Path))} */");

            additionalFiles.ToList().ForEach(f => {
                var projectFile = new ProjectInstance(f.Path);
                projectFile.EvaluatedItemElements.Where(i => i.ItemType == "EmbeddedResource").ToList().ForEach(i => {
                    var resourceFilePath = i.Include;
                    var resourceFileName = Path.GetFileNameWithoutExtension(resourceFilePath);
                    var resourceClassName = Path.GetExtension(resourceFilePath).Replace(".", "");
                    var resourceFileNamespace = Path.GetDirectoryName(resourceFilePath).Replace("\\", ".") + "." + resourceFileName;
                    var stronglyTypedResourceDefinition =
                        CodeHeader.Replace(YearPlaceholder, DateTime.Now.Year.ToString())
                                //   .Replace(AuthorNamePlaceholder, Author)
                                //   .Replace(AuthorEmailPlaceholder, AuthorEmail)
                                  .Replace(LicensePlaceholder, License)
                                  .Replace(FilenamePlaceholder, resourceFileNamespace + "." + resourceClassName + ".g.cs") + 
                        StronglyTypedResourceTemplate.Replace(NamespacePlaceholder, resourceFileName)
                            .Replace(ClassNamePlaceholder, resourceClassName);
                    context.AddSource(resourceFileNamespace + "." + resourceClassName + ".g.cs", stronglyTypedResourceDefinition);
                });
            });
        }
        catch(Exception ex)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("STRONGLYTYPEDRESOURCEGENERATOR", "Strongly Typed Resource Generator", ex.Message, "STRONGLYTYPEDRESOURCEGENERATOR", DiagnosticSeverity.Error, true),  Location.Create("StronglyTypesEmbeddedResourceGenerator.cs", new TextSpan(0, 0), new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 0)))));
            context.AddSource("Error.g.cs", $@"/* {ex.Source}: {ex.Message}:
{ex.StackTrace} */");
        }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //var configOptionsAndAdditionalFiles = context.AnalyzerConfigOptionsProvider.Combine(context.AdditionalTextsProvider.Collect());
        var additionalFiles = context.AdditionalTextsProvider.Collect();// context.AnalyzerOptions.AdditionalFiles;
        context.RegisterSourceOutput(additionalFiles, GenerateSource);
    }
}
