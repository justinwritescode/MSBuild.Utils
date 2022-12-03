namespace MSBuild.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

public static class TaskExtensions
{
    /// <summary>
    /// Attempts to get the current <see cref="ProjectInstance"/> of the executing task via reflection.
    /// </summary>
    /// <returns>A <see cref="ProjectInstance"/> object if one could be determined, otherwise null..</returns>
    public static ProjectInstance? TryGetProjectInstance(this ITask task)
    {
        return task.TryGetProject().CreateProjectInstance();
    }

    public static Project TryGetProject(this ITask task)
    {
        using(XmlReader projectFileReader = XmlReader.Create(task.BuildEngine.ProjectFileOfTaskNode))
        {
            return new Project(projectFileReader);
        }
    }

    public static IDictionary<string, string> GetAllEvaluatedProperties(this ITask task)
    {
        var projectXml = File.ReadAllText(task.BuildEngine.ProjectFileOfTaskNode);
        using(var xmlReader = new System.Xml.XmlTextReader(new StringReader(projectXml)))
        {
            var project = new Project(xmlReader);
            return new ProjectPropertiesDictionary(project);
        }
        // var _allEvaluatedProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        // // using(XmlReader projectFileReader = XmlReader.Create(task.BuildEngine.ProjectFileOfTaskNode))
        // // {
        //     Project project = new Project(task.BuildEngine.ProjectFileOfTaskNode);

        //     foreach(ProjectProperty property in project.AllEvaluatedProperties)
        //     {
        //         string propertyName = property.Name;
        //         string propertyValue = property.EvaluatedValue;

        //         _allEvaluatedProperties[propertyName] = propertyValue;
        //     }
        // // }
        // return _allEvaluatedProperties;
    }

    private class ProjectPropertiesDictionary : IDictionary<string, string>
    {
        private IDictionary<string, string> _dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public ProjectPropertiesDictionary(Project project)
        {
            foreach(var property in project.AllEvaluatedProperties)
            {
                var propertyName = property.Name;
                var propertyValue = property.EvaluatedValue;

                this[propertyName] = propertyValue;
            }
        }

        public virtual string this[string key]
        {
            get => _dictionary.ContainsKey(key) ? _dictionary[key] : string.Empty;
            set => _dictionary[key] = value;
        }

        public ICollection<string> Keys => this._dictionary.Keys;

        public ICollection<string> Values => this._dictionary.Values;

        public int Count => this._dictionary.Count;

        public bool IsReadOnly => this._dictionary.IsReadOnly;

        public void Add(string key, string value) => this._dictionary.Add(key, value);
        public void Add(KeyValuePair<string, string> item) => this._dictionary.Add(item);
        public void Clear() => this._dictionary.Clear();
        public bool Contains(KeyValuePair<string, string> item) => this._dictionary.Contains(item);
        public bool ContainsKey(string key) => this._dictionary.ContainsKey(key);
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => this._dictionary.CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => this._dictionary.GetEnumerator();
        public bool Remove(string key) => this._dictionary.Remove(key);
        public bool Remove(KeyValuePair<string, string> item) => this._dictionary.Remove(item);
        public bool TryGetValue(string key, out string value) => this._dictionary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this._dictionary).GetEnumerator();
    }
}
