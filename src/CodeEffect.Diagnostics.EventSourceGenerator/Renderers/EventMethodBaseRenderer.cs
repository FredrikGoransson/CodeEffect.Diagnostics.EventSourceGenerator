using CodeEffect.Diagnostics.EventSourceGenerator.Model;
using CodeEffect.Diagnostics.EventSourceGenerator.Utils;

namespace CodeEffect.Diagnostics.EventSourceGenerator.Renderers
{
    public abstract class EventMethodBaseRenderer : BaseWithLogging
    {
        protected static string RenderEventSourceType(EventArgumentModel model)
        {
            var type = model.CLRType ?? model.Type;
            switch (type.ToLowerInvariant())
            {
                case ("string"):
                case ("system.string"):
                    return @"string";
                case ("int"):
                case ("system.int32"):
                    return @"int";
                case ("long"):
                case ("system.int64"):
                    return @"long";
                case ("bool"):
                case ("system.boolean"):
                    return @"bool";
                case ("datetime"):
                case ("system.dateTime"):
                    return @"DateTime";
                case ("guid"):
                case ("system.guid"):
                    return @"Guid";
                default:
                    return null;
            }
        }
    }
}