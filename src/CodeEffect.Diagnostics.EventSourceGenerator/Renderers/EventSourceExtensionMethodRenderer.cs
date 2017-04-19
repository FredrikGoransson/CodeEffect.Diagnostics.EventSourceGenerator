using CodeEffect.Diagnostics.EventSourceGenerator.Model;
using CodeEffect.Diagnostics.EventSourceGenerator.Utils;

namespace CodeEffect.Diagnostics.EventSourceGenerator.Renderers
{
    public class EventSourceExtensionMethodRenderer : BaseWithLogging, IExtensionsMethodRenderer
    {
        public string Render(Project project, EventSourceModel eventSource, ExtensionsMethodModel model)
        {
            if (model.Type == "AsJson")
            {
                var output = Template.Template_EXTENSION_ASJSON_DECLARATION;
                output = output.Replace(Template.Template_EXTENSION_CLRTYPE, model.CLRType);

                return output.ToString();
            }
            else if (model.Type == "GetReplicaOrInstanceId")
            {
                var output = Template.Template_EXTENSION_GETREPLICAORINSTANCEID_DECLARATION;
                return output.ToString();
            }
            else if (model.Type == "GetContentDigest")
            {
                var output = Template.Template_EXTENSION_GETCONTENTDIGEST_DECLARATION;
                return output.ToString();
            }
            else if (model.Type == "GetMD5Hash")
            {
                var output = Template.Template_EXTENSION_GETMD5HASH_DECLARATION;
                return output.ToString();
            }
            return null;

        }
    }
}