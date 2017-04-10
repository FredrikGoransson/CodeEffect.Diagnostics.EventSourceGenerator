﻿using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CodeEffect.Diagnostics.EventSourceGenerator
{
    public class EventSourceLogger
    {
        // ReSharper disable InconsistentNaming
        private const string Variable_NAMESPACE_DECLARATION = @"@@NAMESPACE_DECLARATION@@";
        private const string Variable_EVENTSOURCE_CLASS_NAME = @"@@EVENTSOURCE_CLASS_NAME@@";

        private const string Variable_EVENTSOURCE_PARTIAL_FILE_NAME = @"@@EVENTSOURCE_PARTIAL_FILE_NAME@@";
        private const string Variable_LOGGER_EVENTS_DECLARATION = @"@@LOGGER_EVENTS_DECLARATION@@";

        private const string Template_LOGGER_PARTIAL_CLASS_DELCARATION = @"/*******************************************************************************************
*  This class is autogenerated from the class @@LOGGER_SOURCE_FILE_NAME@@
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace @@NAMESPACE_DECLARATION@@
{
	internal sealed partial class @@EVENTSOURCE_CLASS_NAME@@
	{
@@LOGGER_EVENTS_DECLARATION@@
	}
}";

        private const string Variable_LOGGER_SOURCE_FILE_NAME = @"@@LOGGER_SOURCE_FILE_NAME@@";
        private const string Variable_LOGGER_NAME = @"@@LOGGER_NAME@@";
        private const string Variable_LOGGER_NAMESPACE = @"@@LOGGER_NAMESPACE@@";
        private const string Variable_LOGGER_CLASS_NAME = @"@@LOGGER_CLASS_NAME@@";

        private const string Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT = @"@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT@@";
        private const string Template_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT_DELIMITER = @"
			";
        private const string Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION = @"@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION@@";
        private const string Template_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION_DELIMITER = @"
		";
        private const string Variable_LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION = @"@@LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION@@";
        private const string Template_LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION_DELIMITER = @",
			";
        private const string Variable_LOGGER_IMPLEMENTATION = @"@@LOGGER_IMPLEMENTATION@@";

        private const string Template_LOGGER_CLASS_DECLARATION = @"/*******************************************************************************************
*  This class is autogenerated from the class @@LOGGER_SOURCE_FILE_NAME@@
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;

namespace @@NAMESPACE_DECLARATION@@
{
	internal sealed class @@LOGGER_CLASS_NAME@@ : @@LOGGER_NAME@@
	{
		@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION@@

		public @@LOGGER_CLASS_NAME@@(
			@@LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION@@)
		{
			@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT@@
		}
@@LOGGER_IMPLEMENTATION@@
	}
}
";
        // ReSharper restore InconsistentNaming

        public string GetImplementationName()
        {
            return this.Name.Substring(1);
        }
        public string GetKeyword()
        {
            return this.Name.Substring(1).Replace("Logger", "");
        }
        public string Name { get; set; }
        [JsonIgnore]
        public string Include { get; set; }
        public string LoggerNamespace { get; set; }
        public int? StartId { get; set; }
        public EventSourceEventArgument[] ImplicitArguments { get; set; }
        public EventSourceEventCustomArgument[] OverrideArguments { get; set; }

        [JsonIgnore]
        private EventSourceEvent[] Events { get; set; }

        public string RenderImplementation(EventSourcePrototype eventSource, int index, string fileName)
        {
            var className = GetImplementationName();

            var output = Template_LOGGER_CLASS_DECLARATION;
            output = output.Replace(Variable_LOGGER_SOURCE_FILE_NAME, fileName);
            output = output.Replace(Variable_NAMESPACE_DECLARATION, this.LoggerNamespace);

            output = output.Replace(Variable_LOGGER_NAME, this.Name);
            output = output.Replace(Variable_LOGGER_NAMESPACE, this.LoggerNamespace);
            output = output.Replace(Variable_LOGGER_CLASS_NAME, className);

            var memberDeclarations = new StringBuilder();
            var memberDeclarationsDelimiter = "";

            var memberAssignments = new StringBuilder();
            var memberAssignmentsDelimiter = "";

            var methodArguments = new StringBuilder();
            var methodArgumentsDelimiter = "";

            var next = 0;
            foreach (var argument in this?.ImplicitArguments ?? new EventSourceEventArgument[0])
            {
                argument.SetCLRType(eventSource);

                var methodArgument = argument.RenderMethodArgument();
                methodArguments.Append($"{methodArgumentsDelimiter}{methodArgument}");
                methodArgumentsDelimiter = Template_LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION_DELIMITER;

                var memberDeclaration = argument.RenderPrivateDeclaration();
                memberDeclarations.Append($"{memberDeclarationsDelimiter}{memberDeclaration}");
                memberDeclarationsDelimiter = Template_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION_DELIMITER;

                var memberAssignment = argument.RenderPrivateAssignment();
                memberAssignments.Append($"{memberAssignmentsDelimiter}{memberAssignment}");
                memberAssignmentsDelimiter = Template_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT_DELIMITER;

                next++;
            }
            output = output.Replace(Variable_LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION, methodArguments.ToString());
            output = output.Replace(Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION, memberDeclarations.ToString());
            output = output.Replace(Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT, memberAssignments.ToString());

            var implementation = new StringBuilder();
            next = index;
            foreach (var loggerEvent in this.Events)
            {
                if (ImplicitArguments != null && ImplicitArguments.Length > 0)
                {
                    loggerEvent.InsertImplicitArguments(ImplicitArguments);
                }
                if (OverrideArguments != null && OverrideArguments.Length > 0)
                {
                    loggerEvent.OverrideArguments(OverrideArguments);
                }
                loggerEvent.Keywords = new string[] {this.GetKeyword()};
                implementation.AppendLine(loggerEvent.RenderLogger(next, eventSource));
                next += 1;
            }

            output = output.Replace(Variable_LOGGER_IMPLEMENTATION, implementation.ToString());

            return output;

        }

        public string RenderPartial(EventSourcePrototype eventSource,  int index, string fileName)
        {
            var output = Template_LOGGER_PARTIAL_CLASS_DELCARATION;
            output = output.Replace(Variable_EVENTSOURCE_CLASS_NAME, eventSource.ClassName);
            output = output.Replace(Variable_NAMESPACE_DECLARATION, eventSource.Namespace);
            output = output.Replace(Variable_EVENTSOURCE_PARTIAL_FILE_NAME, fileName);
            output = output.Replace(Variable_LOGGER_SOURCE_FILE_NAME, this.Name);
            

            var logger = new StringBuilder();
            var next = index;
            foreach (var loggerEvent in this.Events)
            {
                if (ImplicitArguments != null && ImplicitArguments.Length > 0)
                {
                    loggerEvent.InsertImplicitArguments(ImplicitArguments);
                }
                if (OverrideArguments != null && OverrideArguments.Length > 0)
                {
                    loggerEvent.OverrideArguments(OverrideArguments);
                }
                loggerEvent.Keywords = new string[] { this.GetKeyword() };
                logger.AppendLine(loggerEvent.Render(next, eventSource));
                next += 1;
            }

            output = output.Replace(Variable_LOGGER_EVENTS_DECLARATION, logger.ToString());

            return output;
        }

        public void AddTemplate(EventSourceLoggerTemplate loggerTemplate)
        {
            var events = new List<EventSourceEvent>();
            this.LoggerNamespace = loggerTemplate.Namespace;
            foreach (var templateEvent in loggerTemplate.Events)
            {
                events.Add(templateEvent);
            }
            this.Events = events.ToArray();
            this.Include = loggerTemplate.Include;
        }

        public override string ToString()
        {
            return $"{nameof(EventSourceLogger)} {this.Name}";
        }
    }
}