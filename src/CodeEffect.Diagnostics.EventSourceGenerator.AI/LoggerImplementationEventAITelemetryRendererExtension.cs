﻿using System.Diagnostics.Tracing;
using System.Linq;
using CodeEffect.Diagnostics.EventSourceGenerator.Model;

namespace CodeEffect.Diagnostics.EventSourceGenerator.AI
{
    public class LoggerImplementationEventAITelemetryRendererExtension : AITelemetryRendererExtensionBase, ILoggerImplementationEventRenderer
    {
        public string Render(Project project, ProjectItem<LoggerModel> loggerProjectItem, EventModel model)
        {
//            if (model.OpCode == EventOpcode.Start)
//            {
//                var eventOperationName = GetEventOperationName(model);
//                var output = @"		private CodeEffect.Diagnostics.EventSourceGenerator.AI.OperationHolder _@@LOGGER_EVENT_OPERATION_NAME@@OperationHolder;
//";
//                output = output.Replace("@@LOGGER_EVENT_OPERATION_NAME@@", eventOperationName);

//                return output;
//            }
            return "";
        }
    }
}