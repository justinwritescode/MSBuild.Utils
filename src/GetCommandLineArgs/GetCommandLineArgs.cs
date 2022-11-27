/*
 * GetCommandLineArgs.cs
 *
 *   Created: 2022-11-12-04:15:12
 *   Modified: 2022-11-19-04:04:29
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace MSBuild.Utils;

// Taken from https://stackoverflow.com/questions/3260913/how-to-access-the-msbuild-command-line-parameters-from-within-the-project-file-b
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MSBuild.Extensions;
using static MSBuild.Constants.PropertyNames;

public sealed class GetCommandLineArgs : MSBTask, IEqualityComparer<string>
{
	private static readonly string[] _argsToRemove = { "msbuild", "build", "pack", "restore", "clean", "test", "publish", "run", "dotnet" };

	private string[] PropertiesToRemove = {
		MSBuildToolsPath,
		MSBuildToolsPath32,
		MSBuildThisFileDirectory,
		MSBuildThisFile,
		MSBuildStartupDirectory,
		MSBuildFrameworkToolsPath,
		MSBuildFrameworkToolsPath32,
		MSBuildFrameworkToolsPath64,
		MSBuildExtensionsPath,
		MSBuildExtensionsPath32,
		MSBuildExtensionsPath64,
		MSBuildBinPath,
		FrameworkSDKRoot,
		MSBuildProjectFile,
		MSBuildThisFileFullPath,
		MSBuildProjectFullPath
	};

	private string[] ArgsToRemove => _argsToRemove.Concat(PropertiesToRemove.Select(p => AllEvaluatedProperties[p])).ToArray();

	[Output]
	public ITaskItem[] CommandLineArgs { get; private set; } = Array.Empty<ITaskItem>();
	[Output]
	public string FullCommandLine { get; private set; } = string.Empty;

	private IDictionary<string, string>? _allEvaluatedProperties;
	private IDictionary<string, string> AllEvaluatedProperties => _allEvaluatedProperties ??= this.GetAllEvaluatedProperties();

	public override bool Execute()
	{
		var commandLineArgs = Environment.GetCommandLineArgs().Except(ArgsToRemove, this);//.Except(new[] { "/Users/david/GitHub/justinwritescode/libs/src/MSBuild.Utils/src/GetCommandLineArgs/GetCommandLineArgs.csproj" });
		CommandLineArgs = commandLineArgs.Select(a => new TaskItem(a)).ToArray();
		FullCommandLine = Environment.CommandLine;
		Log.LogMessage(MessageImportance.High, $"Full command line: *{FullCommandLine}*");
		Log.LogMessage(MessageImportance.High, $"Command line args: {string.Join(" ", commandLineArgs)}");
		Log.LogMessage(MessageImportance.High, $"Properties to remove: {string.Join(",\n", PropertiesToRemove.Select(p => $"{p}: {AllEvaluatedProperties[p]}"))}");
		return true;
	}

	public bool Equals(string x, string y)
		=> x.Equals(y, StringComparison.OrdinalIgnoreCase) ||
			x.EndsWith(y + ".exe", StringComparison.OrdinalIgnoreCase) ||
			y.EndsWith(x + ".exe", StringComparison.OrdinalIgnoreCase) ||
			x.StartsWith(y + ".exe", StringComparison.OrdinalIgnoreCase) ||
			y.StartsWith(x + ".exe", StringComparison.OrdinalIgnoreCase) ||
			x.EndsWith(y + ".dll", StringComparison.OrdinalIgnoreCase) ||
			y.EndsWith(x + ".dll", StringComparison.OrdinalIgnoreCase) ||
			x.StartsWith(y + ".dll", StringComparison.OrdinalIgnoreCase) ||
			y.StartsWith(x + ".dll", StringComparison.OrdinalIgnoreCase);

	public int GetHashCode(string obj) => 1;
}
