using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Build.Evaluation;
using Microsoft.CSharp;
using ProjectItem = CodeEffect.Diagnostics.EventSourceGenerator.ProjectItem;

namespace CodeEffect.Diagnostics.EventSourceGenerator
{
    public class EventSourceBuilder
    {
        private readonly Action<string> _logger;

        public EventSourceBuilder(Action<string> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ProjectItem> GetProjectItems(string projectFilePath)
        {
            LogMessage($"Scanning project {projectFilePath} for eventsource definitions");
            var basePath = System.IO.Path.GetDirectoryName(projectFilePath);
            if (basePath == null)
            {
                LogMessage($"Could not find basePath of {projectFilePath}");
            }
            if (basePath != null)
            {
                var projectName = System.IO.Path.GetFileNameWithoutExtension(projectFilePath);
                Project project = null;
                using (var projectFileReader = XmlReader.Create(projectFilePath))
                {
                    project = new Project(projectFileReader);
                    LogMessage($"Loaded project {projectFilePath} from XML with {project.Items.Count} items");
                }

                var hasEventSource = false;
                foreach (
                    var projectItem in project.Items.Where(item => item.EvaluatedInclude.EndsWith(@".eventsource", StringComparison.InvariantCultureIgnoreCase))
                )
                {
                    var rootNamespace = project.Properties.FirstOrDefault(property => property.Name.Equals("RootNamespace"))?.EvaluatedValue ?? projectName;

                    var projectItemFilePath = System.IO.Path.Combine(basePath, projectItem.EvaluatedInclude);
                    yield return new ProjectItem(ProjectItemType.EventSourceDefinition, projectItemFilePath) {Include = projectItem.EvaluatedInclude, RootNamespace = rootNamespace };
                    hasEventSource = true;
                }
                foreach (var projectItem in project.Items.Where(item =>
                    item.EvaluatedInclude.Matches(@"(^|\\)I[^\\]*Logger.cs", StringComparison.InvariantCultureIgnoreCase, useWildcards: false)
                    && item.ItemType == "Compile"))
                {
                    var projectItemFilePath = System.IO.Path.Combine(basePath, projectItem.EvaluatedInclude);
                    yield return new ProjectItem(ProjectItemType.LoggerInterface, projectItemFilePath) {Include = projectItem.EvaluatedInclude};
                }
                foreach (var projectItem in project.Items.Where(item => item.ItemType == "Reference"))
                {
                    var hintPath = projectItem.HasMetadata("HintPath") ? projectItem.GetMetadataValue("HintPath") : null;
                    hintPath = hintPath != null ? PathExtensions.GetAbsolutePath(basePath, hintPath) : null;

                    var projectItemFilePath = hintPath == null ? $"{projectItem.EvaluatedInclude}.dll" : System.IO.Path.Combine(basePath, hintPath);

                    yield return new ProjectItem(ProjectItemType.Reference, projectItemFilePath) { Include = projectItem.EvaluatedInclude };
                }

                if (!hasEventSource)
                {
                    var rootNamespace = project.Properties.FirstOrDefault(property => property.Name.Equals("RootNamespace"))?.EvaluatedValue ?? projectName;

                    var include = $"DefaultEventSource.eventsource";
                    var projectItemFilePath = System.IO.Path.Combine(basePath, include);
                    yield return new ProjectItem(ProjectItemType.DefaultGeneratedEventSource, projectItemFilePath) {Include = include, RootNamespace = rootNamespace };
                }
            }
        }

        public void AddGeneratedOutputsToProject(string projectFilePath, IEnumerable<ProjectItem> includes, bool saveChanges = true)
        {
            LogMessage($"Loading projectfile {projectFilePath} to include new files");
            var basePath = System.IO.Path.GetDirectoryName(projectFilePath);
            Project project = null;
            var updatedProjectFile = false;
            var loadedFromProjectCollection = false;

            foreach (var loadedProject in ProjectCollection.GlobalProjectCollection.LoadedProjects)
            {
                if (loadedProject.FullPath.Equals(projectFilePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    project = loadedProject;
                    LogMessage($"Loaded project {projectFilePath} from GlobalProjectCollection with {project.Items.Count} items");
                    loadedFromProjectCollection = true;
                    break;
                }
            }
            if (project == null)
            {
                using (var projectFileReader = XmlReader.Create(projectFilePath))
                {
                    project = new Project(projectFileReader);
                    LogMessage($"Loaded project {projectFilePath} from XML with {project.Items.Count} items");

                }
            }
            if (project == null)
            {
                throw new NotSupportedException($"Failed to load {projectFilePath} from either XML or GlobalProjectCollection");
            }

            var existingItems = new List<Microsoft.Build.Evaluation.ProjectItem>();
            existingItems.AddRange(project.Items.Where(item => item.HasMetadata("AutoGenerated")));

            // Add or check that it alread exists
            foreach (var include in includes)
            {
                var includeName = include.Name.StartsWith(basePath) ? include.Name.Substring(basePath.Length + 1) : include.Name;

                var alreadyIncluded = false;
                var matchingItems = project.Items.Where(item => item.EvaluatedInclude.Matches($"{includeName}", StringComparison.InvariantCultureIgnoreCase));
                alreadyIncluded = matchingItems.Any();
                if (!alreadyIncluded)
                {
                    var metadata = Enum.GetName(typeof(ProjectItemType), include.ItemType);
                    updatedProjectFile = true;
                    IList<Microsoft.Build.Evaluation.ProjectItem> addedItems = null;
                    if (include.ItemType == ProjectItemType.DefaultGeneratedEventSource)
                    {
                        var hash = include.Content.ToMD5().ToHex();
                        addedItems = project.AddItem("Compile", includeName, new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>("AutoGenerated", metadata),
                            new KeyValuePair<string, string>("Hash", hash), 
                        });
                    }
                    else
                    {
                        addedItems = project.AddItem("Compile", includeName, new KeyValuePair<string, string>[]
                        {
                        new KeyValuePair<string, string>("AutoGen", "true"),
                        new KeyValuePair<string, string>("AutoGenerated", metadata),
                        new KeyValuePair<string, string>("DependentUpon", include.DependentUpon.Include.RemoveCommonPrefix(includeName, System.IO.Path.DirectorySeparatorChar)),
                        });
                    }

                    foreach (var addedItem in addedItems)
                    {
                        LogMessage($"Including project item {addedItem.EvaluatedInclude}");
                        existingItems.Remove(addedItem);
                    }
                }
                else
                {
                    foreach (var matchingItem in matchingItems)
                    {
                        LogMessage($"Matched existing project item {matchingItem.EvaluatedInclude}");
                        existingItems.Remove(matchingItem);
                    }
                }
            }
            // Check if we should remove the AutoGenerated DefaultEventSource.eventsource
            var autoGeneratedDefaultEventSource = existingItems.FirstOrDefault(
                existingItem => existingItem.HasMetadata("AutoGenerated") && existingItem.GetMetadataValue("AutoGenerated") == "DefaultEventSource");
            if (autoGeneratedDefaultEventSource != null)
            {
                if (autoGeneratedDefaultEventSource.EvaluatedInclude != "DefaultEventSource.cs")
                {
                    updatedProjectFile = true;
                    LogMessage($"Updating Project Metadata for {autoGeneratedDefaultEventSource.EvaluatedInclude} as it has been changed from it's original state");
                    autoGeneratedDefaultEventSource.RemoveMetadata("AutoGenerated");
                    existingItems.Remove(autoGeneratedDefaultEventSource);
                }
                else
                {
                    var hash = autoGeneratedDefaultEventSource.HasMetadata("Hash") ? autoGeneratedDefaultEventSource.GetMetadataValue("Hash") : "";

                    var filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(projectFilePath), autoGeneratedDefaultEventSource.EvaluatedInclude);
                    var fileContent = System.IO.File.ReadAllText(filePath);

                    var hashCheck = fileContent.ToMD5().ToHex();

                    if (hash != hashCheck)
                    {
                        updatedProjectFile = true;
                        LogMessage($"Updating Project Metadata for {autoGeneratedDefaultEventSource.EvaluatedInclude} as it's content been changed from it's original state");
                        autoGeneratedDefaultEventSource.RemoveMetadata("AutoGenerated");
                        existingItems.Remove(autoGeneratedDefaultEventSource);
                    }
                }
            }

            // Remove old items that are no longer referenced
            foreach (var existingItem in existingItems)
            {
                LogMessage($"Removing existing project item {existingItem.EvaluatedInclude}");
            }
            if (existingItems.Count > 0)
            {
                updatedProjectFile = true;
                project.RemoveItems(existingItems);
            }

            if (!updatedProjectFile)
            {
                LogMessage($"Igoring to save project file {projectFilePath} - no changes performed");
            }
            else
            {
                if (saveChanges)
                {
                    if (loadedFromProjectCollection)
                    {
                        LogMessage($"Unloading project file {projectFilePath} from Global project collection");
                        ProjectCollection.GlobalProjectCollection.UnloadProject(project);
                    }
                    LogMessage($"Saving project file {projectFilePath}");
                    project.Save(projectFilePath);
                    LogMessage($"Loading project file {projectFilePath} in Global project collection");
                    ProjectCollection.GlobalProjectCollection.LoadProject(projectFilePath);
                }
                else
                {
                    LogMessage($"Project file {projectFilePath} changed");
                    LogMessage(project.Xml.RawXml);
                }
            }
        }

        private void LogMessage(string message)
        {
            _logger(message);
        }
        
        public IEnumerable<ProjectItem> Execute(string projectBasePath, IEnumerable<ProjectItem> projectFiles)
        {
            var files = projectFiles.ToArray();
            LogMessage($"Scanning {files.Length} project file{(files.Length == 1 ? "" : "s")} for eventsources");

            var loggerTemplates = ExecuteLoggers(files);
            var outputs = ExecuteEventSources(projectBasePath, loggerTemplates, files);
            
            LogMessage($"\tGenerated {outputs.Count()} files");
            foreach (var output in outputs)
            {
                LogMessage($"\t{output.ItemType.Name()} {output.Name}");
            }
            return outputs;
        }

        //private bool IsExcludedFolder(string path)
        //{
        //    var name = System.IO.Path.GetFileNameWithoutExtension(path);
        //    if (name.Equals("bin")) return true;
        //    if (name.Equals("obj")) return true;
        //    if (name.StartsWith(".")) return true;

        //    return false;
        //}
        //private IEnumerable<string> GetSubFolders(string path)
        //{
        //    var subFolders = System.IO.Directory.GetDirectories(path);

        //    foreach (var subFolder in subFolders.Where(sf => !IsExcludedFolder(sf)))
        //    {
        //        yield return subFolder;
        //    }
        //}

        private IEnumerable<ProjectItem> ExecuteEventSources(string projectBasePath, EventSourceLoggerTemplate[] loggers, IEnumerable<ProjectItem> projectFiles)
        {
            var files = projectFiles.ToArray();
            LogMessage($"Scanning {files.Length} project file{(files.Length == 1 ? "" : "s")} for eventsources");

            var generatedFiles = new List<ProjectItem>();
            //var eventSourceFiles = files.Where(projectFile => projectFile.Matches("*.eventsource", StringComparison.InvariantCultureIgnoreCase));
            var eventSourceProjectItems = files.OfType(ProjectItemType.EventSourceDefinition);
            IEnumerable<ProjectItem> outputs = null;
            foreach (var eventSourceProjectItem in eventSourceProjectItems)
            {
                LogMessage($"Found EventSource from file {eventSourceProjectItem.Name}");
                outputs = GenerateEventSourceCode(projectBasePath, eventSourceProjectItem, loggers);
                foreach (var output in outputs)
                {
                    generatedFiles.Add(output);
                    LogMessage($"Generated EventSource output {output.Name} from file {eventSourceProjectItem.Name}");
                    output.DependentUpon = eventSourceProjectItem;
                }
            }

            outputs = AddDefaultEventSource(projectBasePath, loggers, projectFiles);
            foreach (var output in outputs)
            {
                generatedFiles.Add(output);
                LogMessage($"Added DefaultEventSource output {output.Name}");
            }
            return generatedFiles;
        }

        private IEnumerable<ProjectItem> AddDefaultEventSource(string projectBasePath, EventSourceLoggerTemplate[] loggers, IEnumerable<ProjectItem> projectFiles)
        {
            var defaultEventSourceFile = projectFiles.GetDefaultEventSourceProjectItem();
            if (defaultEventSourceFile != null)
            {
                var defaultEventSource = new EventSourcePrototype()
                {
                    Namespace = defaultEventSourceFile.RootNamespace,
                    Name = "DefaultEventSource",
                    ClassName = "DefaultEventSource",
                    TypeTemplates = new EventSourceTypeTemplate[]
                    {
                        new EventSourceTypeTemplate()
                        {
                            Name = "Exception",
                            CLRType = "System.Exception" +
                                      "",
                            Arguments = new EventSourceEventCustomArgument[]
                            {
                                new EventSourceEventCustomArgument("message", "string", "$this.Message"),
                                new EventSourceEventCustomArgument("source", "string", "$this.Source"),
                                new EventSourceEventCustomArgument("exceptionTypeName", "string", "$this.GetType().FullName"),
                                new EventSourceEventCustomArgument("exception", "string", "$this.AsJson()"),
                            }
                        }
                    }
                };
                var eventSourceLoggers = new List<EventSourceLogger>();
                var startId = 1000;
                foreach (var logger in loggers)
                {
                    eventSourceLoggers.Add(new EventSourceLogger()
                    {
                        Name = logger.Name,
                        StartId = startId,
                        ImplicitArguments = new EventSourceEventArgument[]
                        {
                            new EventSourceEventArgument() { Name = "autogenerated", Type = "bool"}
                        }
                    });
                    startId += 1000;
                }
                defaultEventSource.Loggers = eventSourceLoggers.ToArray();

                var sourceFileName = System.IO.Path.GetFileName(defaultEventSourceFile.Name);
                var implementationFileName = $"{System.IO.Path.GetFileNameWithoutExtension(defaultEventSourceFile.Name)}.cs";
                var fileRelativePath = defaultEventSourceFile.Name.RemoveFromStart(projectBasePath + System.IO.Path.DirectorySeparatorChar).Replace(sourceFileName, implementationFileName);

                defaultEventSource.Include = fileRelativePath;
                defaultEventSource.SourceFilePath = defaultEventSourceFile.Include;

                var jsonFile = Newtonsoft.Json.JsonConvert.SerializeObject(defaultEventSource, Newtonsoft.Json.Formatting.Indented);
                yield return
                    new ProjectItem(ProjectItemType.DefaultGeneratedEventSource, name: defaultEventSourceFile.Name, content: jsonFile)
                    {
                        Include = defaultEventSourceFile.Include,
                        RootNamespace = defaultEventSourceFile.RootNamespace
                    };

                var outputs = GenerateEventSourceCode(projectBasePath, defaultEventSourceFile, defaultEventSource, loggers);
                foreach (var output in outputs)
                {
                    yield return output;
                }
            }
        }

        private EventSourceLoggerTemplate[] ExecuteLoggers(IEnumerable<ProjectItem> projectFiles)
        {
            var files = projectFiles.ToArray();

            LogMessage($"Scanning {files.Length} project file{(files.Length == 1 ? "": "s")} for loggers");
            var loggerTemplates = new List<EventSourceLoggerTemplate>();
            //var loggerFiles = files.Where(projectFile => projectFile.Matches(@"(^|\\)I[^\\]*Logger.cs", StringComparison.InvariantCultureIgnoreCase, useWildcards: false));
            var loggerFiles = files.OfType(ProjectItemType.LoggerInterface);
            var referenceFiles = files.OfType(ProjectItemType.Reference);
            foreach (var file in loggerFiles)
            {
                LogMessage($"Found Logger file {file.Name}");
                var foundLoggerTemplates = CompileAndEvaluateInterface(file, referenceFiles);
                foreach (var foundLoggerTemplate in foundLoggerTemplates)
                {
                    LogMessage($"Compiled Logger Template {foundLoggerTemplate.Name}");
                    
                }
                loggerTemplates.AddRange(foundLoggerTemplates);
            }
            return loggerTemplates.ToArray();
        }

        private EventSourceLoggerTemplate[] CompileAndEvaluateInterface(ProjectItem projectItem, IEnumerable<ProjectItem> referenceItems)
        {
            LogMessage($"Compiling possible logger file {projectItem.Include}");

            var loggers = new List<EventSourceLoggerTemplate>();
            try
            {
                var parameters = new CompilerParameters();

                foreach (var referenceItem in referenceItems)
                {
                    parameters.ReferencedAssemblies.Add(referenceItem.Name);
                }

                //parameters.ReferencedAssemblies.Add("System.dll");
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;

                parameters.IncludeDebugInformation = false;
                var cSharpCodeProvider = new CSharpCodeProvider();
                //var cSharpCodeProvider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
                var compilerResults = cSharpCodeProvider.CompileAssemblyFromFile(parameters, projectItem.Name);
                foreach (CompilerError compilerResultsError in compilerResults.Errors)
                {
                    LogMessage(compilerResultsError.ToString());
                }

                var types = compilerResults.CompiledAssembly.GetTypes();
                foreach (
                    var type in
                    types.Where(t => t.IsInterface && t.Name.Matches(@"^I[^\\]*Logger", StringComparison.InvariantCultureIgnoreCase, useWildcards: false)))
                {
                    var include = projectItem.Include.Replace(projectItem.Name, type.Name);
                    var eventSourceLogger = new EventSourceLoggerTemplate()
                    {
                        Name = type.Name,
                        Namespace = type.Namespace,
                        Include = include
                    };
                    var eventSourceEvents = new List<EventSourceEvent>();
                    foreach (var methodInfo in type.GetMethods())
                    {
                        var eventSourceEventArguments = new List<EventSourceEventArgument>();
                        var eventSourceEvent = new EventSourceEvent()
                        {
                            Name = methodInfo.Name,
                        };
                        foreach (var parameterInfo in methodInfo.GetParameters())
                        {
                            var typeString = parameterInfo.ParameterType.GetFriendlyName();
                            eventSourceEventArguments.Add(new EventSourceEventArgument()
                            {
                                Name = parameterInfo.Name,
                                Type = typeString,
                            });
                        }
                        eventSourceEvent.Arguments = eventSourceEventArguments.ToArray();
                        eventSourceEvents.Add(eventSourceEvent);
                    }

                    eventSourceLogger.Events = eventSourceEvents.ToArray();
                    loggers.Add(eventSourceLogger);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to compile/evaluate {projectItem.Include} - {ex.Message}\r\n{ex.StackTrace}");
            }
            return loggers.ToArray();
        }

        private IEnumerable<ProjectItem> GenerateEventSourceCode(string projectBasePath, ProjectItem projectItem, EventSourceLoggerTemplate[] loggers)
        {
            var sourceFileName = System.IO.Path.GetFileName(projectItem.Name);
            var implementationFileName = $"{System.IO.Path.GetFileNameWithoutExtension(projectItem.Name)}.cs";
            var fileRelativePath = projectItem.Name.RemoveFromStart(projectBasePath + System.IO.Path.DirectorySeparatorChar).Replace(sourceFileName, implementationFileName);

            var fileRelateiveFolderPath = System.IO.Path.GetDirectoryName(fileRelativePath);
            var eventSourceNamespace = $"{projectItem.RootNamespace}.{fileRelateiveFolderPath.Replace(System.IO.Path.DirectorySeparatorChar, '.')}";

            var content = System.IO.File.ReadAllText(projectItem.Name);
            var eventSourcePrototype = Newtonsoft.Json.JsonConvert.DeserializeObject<EventSourcePrototype>(content);

            var fileName = System.IO.Path.GetFileName(projectItem.Name);
            var className = System.IO.Path.GetFileNameWithoutExtension(fileName);

            eventSourcePrototype.ClassName = className;
            eventSourcePrototype.Include = fileRelativePath;
            eventSourcePrototype.SourceFilePath = projectItem.Include;
            eventSourcePrototype.Namespace = eventSourceNamespace;          
            return GenerateEventSourceCode(projectBasePath, projectItem, eventSourcePrototype, loggers);
        }

        private IEnumerable<ProjectItem> GenerateEventSourceCode(string projectBasePath, ProjectItem projectItem, EventSourcePrototype eventSourcePrototype, EventSourceLoggerTemplate[] loggers)
        {
            eventSourcePrototype.AvailableLoggers = loggers;
            var outputs = eventSourcePrototype.Render(projectBasePath);

            var eventSourceProjectItems = new List<ProjectItem>();
            foreach (var output in outputs)
            {
                output.DependentUpon = projectItem;
                eventSourceProjectItems.Add(output);
            }
            return eventSourceProjectItems;
        }

    }

    public class LoggerEvaluator
    {
        private Action<string> LogMessage { get; }

        public LoggerEvaluator(Action<string> logMessage)
        {
            LogMessage = logMessage;
        }

        private EventSourceLoggerTemplate[] CompileAndEvaluateInterface(ProjectItem projectItem)
        {
            LogMessage($"Compiling possible logger file {projectItem.Include}");

            var loggers = new List<EventSourceLoggerTemplate>();
            try
            {
                var parameters = new CompilerParameters();

                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;

                parameters.IncludeDebugInformation = false;
                var cSharpCodeProvider = new CSharpCodeProvider();
                //var cSharpCodeProvider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
                var compilerResults = cSharpCodeProvider.CompileAssemblyFromFile(parameters, projectItem.Name);
                foreach (CompilerError compilerResultsError in compilerResults.Errors)
                {
                    LogMessage(compilerResultsError.ToString());
                }

                var types = compilerResults.CompiledAssembly.GetTypes();
                foreach (
                    var type in
                    types.Where(t => t.IsInterface && t.Name.Matches(@"^I[^\\]*Logger", StringComparison.InvariantCultureIgnoreCase, useWildcards: false)))
                {
                    var include = projectItem.Include.Replace(projectItem.Name, type.Name);
                    var eventSourceLogger = new EventSourceLoggerTemplate()
                    {
                        Name = type.Name,
                        Namespace = type.Namespace,
                        Include = include
                    };
                    var eventSourceEvents = new List<EventSourceEvent>();
                    foreach (var methodInfo in type.GetMethods())
                    {
                        var eventSourceEventArguments = new List<EventSourceEventArgument>();
                        var eventSourceEvent = new EventSourceEvent()
                        {
                            Name = methodInfo.Name,
                        };
                        foreach (var parameterInfo in methodInfo.GetParameters())
                        {
                            var typeString = parameterInfo.ParameterType.GetFriendlyName();
                            eventSourceEventArguments.Add(new EventSourceEventArgument()
                            {
                                Name = parameterInfo.Name,
                                Type = typeString,
                            });
                        }
                        eventSourceEvent.Arguments = eventSourceEventArguments.ToArray();
                        eventSourceEvents.Add(eventSourceEvent);
                    }

                    eventSourceLogger.Events = eventSourceEvents.ToArray();
                    loggers.Add(eventSourceLogger);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to compile/evaluate {projectItem.Include} - {ex.Message}\r\n{ex.StackTrace}");
            }
            return loggers.ToArray();
        }
    }
}