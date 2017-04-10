using System;
using CodeEffect.Diagnostics.EventSourceGenerator.Model;

namespace CodeEffect.Diagnostics.EventSourceGenerator.Builders
{
    public class EventArgumentBuilder : IEventArgumentBuilder
    {
        public void Build(Project project, EventSourceModel eventSource, EventArgumentModel model)
        {
            var type = model.Type;
            if (IsComplexType(model.Type))
            {
                model.TypeTemplate =  eventSource.TypeTemplates.GetTypeTemplate(model.Type);
                if (model.TypeTemplate != null)
                {
                    var parsedType = ParseType(model.TypeTemplate.CLRType);
                    var renderedType = RenderCLRType(parsedType);
                    if (renderedType == "string" && (parsedType != typeof(string)))
                    {
                        type = model.TypeTemplate.CLRType;
                    }
                    else
                    {
                        type = renderedType;
                    }
                    model.IsTemplated = true;
                }    
            }
            model.CLRType = type;
        }

        private static Type ParseType(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case ("string"):
                case ("system.string"):
                    return typeof(string);
                case ("int"):
                case ("system.int32"):
                    return typeof(int);
                case ("long"):
                case ("system.int64"):
                    return typeof(long);
                case ("bool"):
                case ("system.boolean"):
                    return typeof(bool);
                case ("datetime"):
                case ("system.dateTime"):
                    return typeof(System.DateTime);
                case ("guid"):
                case ("system.guid"):
                    return typeof(Guid);
                default:
                    return typeof(object);
            }
        }        

        private static string RenderCLRType(Type type)
        {
            if (type == typeof(string))
                return @"string";
            if (type == typeof(int))
                return @"int";
            if (type == typeof(long))
                return @"long";
            if (type == typeof(bool))
                return @"bool";
            if (type == typeof(DateTime))
                return @"DateTime";
            if (type == typeof(Guid))
                return @"Guid";

            return @"string";
        }

        private static bool IsComplexType(string type)
        {
            var parsedType = ParseType(type);
            return parsedType == typeof(object);
        }
    }
}