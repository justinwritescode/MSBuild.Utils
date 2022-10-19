namespace JustinWritesCode.MSBuild.Utils.Extensions;
using System.Reflection;

public class TaskExtensions
{
    /// <summary>
    /// Attempts to get the current <see cref="ProjectInstance"/> of the executing task via reflection.
    /// </summary>
    /// <returns>A <see cref="ProjectInstance"/> object if one could be determined, otherwise null..</returns>
    public static ProjectInstance? TryGetProjectInstance(this ITask task)
    {
        try
        {
            FieldInfo requestEntryFieldInfo = BuildEngine.GetType().GetField("_requestEntry", BindingFlags.Instance | BindingFlags.NonPublic);

            if (requestEntryFieldInfo != null && BuildRequestEntryTypeLazy.Value != null && BuildRequestConfigurationTypeLazy.Value != null)
            {
                object requestEntry = requestEntryFieldInfo.GetValue(BuildEngine);

                if (requestEntry != null && BuildRequestEntryRequestConfigurationPropertyInfo.Value != null)
                {
                    object requestConfiguration = BuildRequestEntryRequestConfigurationPropertyInfo.Value.GetValue(requestEntry);

                    if (requestConfiguration != null && BuildRequestConfigurationProjectPropertyInfo.Value != null)
                    {
                        return BuildRequestConfigurationProjectPropertyInfo.Value.GetValue(requestConfiguration) as ProjectInstance;
                    }
                }
            }
        }
        catch
        {
            // Ignored because we never want this method to throw since its using reflection to access internal members that could go away with any future release of MSBuild
        }

        return null;
    }
}
