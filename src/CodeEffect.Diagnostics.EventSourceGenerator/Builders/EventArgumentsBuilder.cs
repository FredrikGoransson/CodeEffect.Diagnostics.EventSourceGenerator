using System.Linq;
using CodeEffect.Diagnostics.EventSourceGenerator.Model;
using CodeEffect.Diagnostics.EventSourceGenerator.Utils;

namespace CodeEffect.Diagnostics.EventSourceGenerator.Builders
{
    public class EventArgumentsBuilder : BaseWithLogging, IEventBuilder, ILoggerEventBuilder
    {
        public void Build(Project project, ProjectItem<EventSourceModel> eventSourceProjectItem, EventModel model)
        {
            var eventSourceModel = eventSourceProjectItem.Content;
            if (eventSourceModel == null)
            {
                LogError($"{eventSourceProjectItem.Name} should have a content of type {typeof(EventSourceModel).Name} set but found {eventSourceProjectItem.Content?.GetType().Name ?? "null"}");
                return;
            }

            var eventArgumentBuilders = new IEventArgumentBuilder[]
            {
                new EventArgumentBuilder(),
                new EventArgumentExtensionMethodBuilder(), 
            }.Union(project.GetExtensions<IEventArgumentBuilder>()).ToArray();
            foreach (var argument in model.Arguments ?? new EventArgumentModel[0])
            {
                foreach (var builder in eventArgumentBuilders)
                {
                    PassAlongLoggers(builder as IWithLogging);
                    builder.Build(project, eventSourceProjectItem, argument);
                }
            }
        }

        public void Build(Project project, ProjectItem<EventSourceModel> eventSourceProjectItem, LoggerModel loggerModel, EventModel model)
        {
            Build(project, eventSourceProjectItem, model);
        }
        
    }
}