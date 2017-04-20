﻿namespace CodeEffect.Diagnostics.EventSourceGenerator
{
    public partial class Template
    {
        // ReSharper disable InconsistentNaming
        public const string Variable_NAMESPACE_DECLARATION = @"@@NAMESPACE_DECLARATION@@";
        public const string Variable_EVENTSOURCE_CLASS_NAME = @"@@EVENTSOURCE_CLASS_NAME@@";

        public const string Variable_EVENTSOURCE_PARTIAL_FILE_NAME = @"@@EVENTSOURCE_PARTIAL_FILE_NAME@@";
        public const string Variable_LOGGER_EVENTS_DECLARATION = @"@@LOGGER_EVENTS_DECLARATION@@";

        public const string Template_LOGGER_PARTIAL_CLASS_DECLARATION =
            @"/*******************************************************************************************
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

        public const string Variable_LOGGER_SOURCE_FILE_NAME = @"@@LOGGER_SOURCE_FILE_NAME@@";
        public const string Variable_LOGGER_NAME = @"@@LOGGER_NAME@@";
        public const string Variable_LOGGER_CLASS_NAME = @"@@LOGGER_CLASS_NAME@@";
        public const string Variable_EVENTSOURCE_NAMESPACE = @"@@EVENTSOURCE_NAMESPACE@@";

        public const string Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT = @"@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT@@";
        public const string Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT_DELIMITER = @"
			";
        public const string Variable_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION = @"@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION@@";
        public const string Template_LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION_DELIMITER = @"
		";
        public const string Variable_LOGGER_IMPLICIT_ARGUMENTS_CONSTRUCTOR_DECLARATION = @"@@LOGGER_IMPLICIT_ARGUMENTS_CONSTRUCTOR_DECLARATION@@";
        public const string Variable_LOGGER_IMPLICIT_ARGUMENTS_METHOD_CONSTRUCTOR_DELIMITER = @",
			";
        public const string Template_LOGGER_IMPLICIT_ARGUMENTS_METHOD_DECLARATION_DELIMITER = @",
			";
        public const string Variable_LOGGER_IMPLEMENTATION = @"@@LOGGER_IMPLEMENTATION@@";

        public const string Template_LOGGER_CLASS_DECLARATION = @"/*******************************************************************************************
*  This class is autogenerated from the class @@LOGGER_SOURCE_FILE_NAME@@
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using @@NAMESPACE_DECLARATION@@;

namespace @@EVENTSOURCE_NAMESPACE@@
{
	internal sealed class @@LOGGER_CLASS_NAME@@ : @@LOGGER_NAME@@
	{
		@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_DECLARATION@@

		public @@LOGGER_CLASS_NAME@@(
			@@LOGGER_IMPLICIT_ARGUMENTS_CONSTRUCTOR_DECLARATION@@)
		{
			@@LOGGER_IMPLICIT_ARGUMENTS_MEMBER_ASSIGNMENT@@
		}
@@LOGGER_IMPLEMENTATION@@
	}
}
";
        // ReSharper restore InconsistentNaming

    }

    public partial class Template
    {
        // ReSharper disable InconsistentNaming
        public const string Variable_EVENT_NAME = @"@@EVENT_NAME@@";
        public const string Variable_EVENT_ID = @"@@EVENT_ID@@";
        public const string Variable_EVENT_LEVEL = @"@@EVENT_LEVEL@@";
        public const string Variable_EVENT_KEYWORDS_DECLARATION = @"@@EVENT_KEYWORDS_DECLARATION@@";
        public const string Variable_EVENT_MESSAGE_FORMATTER = @"@@EVENT_MESSAGE_FORMATTER@@";
        public const string Variable_EVENT_METHOD_ACCESS = @"@@EVENT_METHOD_ACCESS@@";
        public const string Variable_EVENT_METHOD_ARGUMENTS = @"@@EVENT_METHOD_ARGUMENTS@@";
        public const string Template_EVENT_METHOD_ARGUMENT_DELIMITER = @", 
			";
        public const string Template_EVENT_METHOD_CALL_ARGUMENT_DELIMITER = @", 
				";
        public const string Variable_WRITEEVENT_CALL_ARGUMENTS = @"@@WRITEEVENT_CALL_ARGUMENTS@@";

        public const string Template_EVENT_METHOD = @"
		private const int @@EVENT_NAME@@EventId = @@EVENT_ID@@;

		[Event(@@EVENT_NAME@@EventId, Level = EventLevel.@@EVENT_LEVEL@@, Message = ""@@EVENT_MESSAGE_FORMATTER@@""@@EVENT_KEYWORDS_DECLARATION@@)]
		@@EVENT_METHOD_ACCESS@@ void @@EVENT_NAME@@(
			@@EVENT_METHOD_ARGUMENTS@@)
		{
			WriteEvent(
				@@WRITEEVENT_CALL_ARGUMENTS@@);
		}";

        public const string Variable_NON_EVENT_METHOD_DECLARATION = @"@@NON_EVENT_METHOD_DECLARATION@@";
        public const string Variable_NON_EVENT_METHOD_ARGUMENTS = @"@@NON_EVENT_METHOD_ARGUMENTS@@";
        public const string Template_NONEVENT_METHOD_ARGUMENT_DELIMITER = @", 
			";
        public const string Variable_NON_EVENT_ASSIGNMENT_ARGUMENTS = @"@@NON_EVENT_ASSIGNMENT_ARGUMENTS@@";
        public const string Template_NON_EVENT_ASSIGNMENT_ARGUMENT_DELIMITER = @", 
					";

        public const string Template_NON_EVENT_METHOD = @"
		[NonEvent]
		public void @@EVENT_NAME@@(
			@@NON_EVENT_METHOD_ARGUMENTS@@)
		{
			if (this.IsEnabled())
			{
				@@EVENT_NAME@@(
					@@NON_EVENT_ASSIGNMENT_ARGUMENTS@@);
			}
		}
";

        public const string Variable_LOGGER_METHOD_NAME = @"@@LOGGER_METHOD_NAME@@";
        public const string Variable_LOGGER_METHOD_ARGUMENTS = @"@@LOGGER_METHOD_ARGUMENTS@@";
        public const string Template_LOGGER_METHOD_ARGUMENTS_DELIMITER = @", 
			";
        public const string Variable_LOGGER_METHOD_IMPLEMENTATION_CALL_ARGUMENTS = @"@@LOGGER_METHOD_IMPLEMENTATION_CALL_ARGUMENTS@@";
        public const string Variable_LOGGER_METHOD_IMPLEMENTATION = @"@@LOGGER_METHOD_IMPLEMENTATION@@";

        public const string Template_LOGGER_METHOD_CALL_EVENTSOURCE_EVENT = @"			@@EVENTSOURCE_CLASS_NAME@@.Current.@@LOGGER_METHOD_NAME@@(
				@@LOGGER_METHOD_IMPLEMENTATION_CALL_ARGUMENTS@@
			);
";
        public const string Template_LOGGER_CALL_ARGUMENTS_DELIMITER = @", 
				";

        public const string Template_LOGGER_METHOD = @"
		public void @@LOGGER_METHOD_NAME@@(
			@@LOGGER_METHOD_ARGUMENTS@@)
		{
@@LOGGER_METHOD_IMPLEMENTATION@@    
		}
";
        // ReSharper restore InconsistentNaming
    }

    public partial class Template
    {
        // ReSharper disable InconsistentNaming

        public const string Variable_SOURCE_FILE_NAME = @"@@SOURCE_FILE_NAME@@";
        public const string Variable_EVENTSOURCE_NAME = @"@@EVENTSOURCE_NAME@@";
        public const string Variable_KEYWORDS_DECLARATION = @"@@KEYWORDS_DECLARATION@@";
        public const string Variable_EVENTS_DECLARATION = @"@@EVENTS_DECLARATION@@";
        public const string Variable_EXTENSIONS_DECLARATION = @"@@EXTENSIONS_DECLARATION@@";
        public const string Template_EXTENSIONS_DECLARATION = @"
	internal static class @@EVENTSOURCE_CLASS_NAME@@Helpers
	{
@@EXTENSION_METHODS_DECLARATION@@
	}";
        public const string Variable_EXTENSION_METHODS_DECLARATION = @"@@EXTENSION_METHODS_DECLARATION@@";


        public const string Template_EVENTSOURCE_CLASS_DECLARATION = @"/*******************************************************************************************
*  This class is autogenerated from the class @@SOURCE_FILE_NAME@@
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace @@NAMESPACE_DECLARATION@@
{
	[EventSource(Name = ""@@EVENTSOURCE_NAME@@"")]
	internal sealed partial class @@EVENTSOURCE_CLASS_NAME@@ : EventSource
	{
		public static readonly @@EVENTSOURCE_CLASS_NAME@@ Current = new @@EVENTSOURCE_CLASS_NAME@@();

		static @@EVENTSOURCE_CLASS_NAME@@()
		{
			// A workaround for the problem where ETW activities do not 
			// get tracked until Tasks infrastructure is initialized.
			// This problem will be fixed in .NET Framework 4.6.2.
			Task.Run(() => { });
		}

		// Instance constructor is private to enforce singleton semantics
		private @@EVENTSOURCE_CLASS_NAME@@() : base() { }

		#region Keywords
		// Event keywords can be used to categorize events. 
		// Each keyword is a bit flag. A single event can be 
		// associated with multiple keywords (via EventAttribute.Keywords property).
		// Keywords must be defined as a public class named 'Keywords' 
		// inside EventSource that uses them.
		public static class Keywords
		{
@@KEYWORDS_DECLARATION@@
		}
		#endregion Keywords

		#region Events

@@EVENTS_DECLARATION@@

		#endregion Events
	}

@@EXTENSIONS_DECLARATION@@

}";
        // ReSharper restore InconsistentNaming
    }

    public partial class Template
    {
        // ReSharper disable InconsistentNaming
        public const string Template_ARGUMENT_CLR_TYPE = @"@@ARGUMENT_CLR_TYPE@@";
        public const string Template_ARGUMENT_NAME = @"@@ARGUMENT_NAME@@";

        public const string Template_METHOD_ARGUMENT_DECLARATION = @"@@ARGUMENT_CLR_TYPE@@ @@ARGUMENT_NAME@@";
        public const string Template_PRIVATE_MEMBER_DECLARATION = @"private readonly @@ARGUMENT_CLR_TYPE@@ _@@ARGUMENT_NAME@@;";
        public const string Template_PRIVATE_MEMBER_ASSIGNMENT = @"_@@ARGUMENT_NAME@@ = @@ARGUMENT_NAME@@;";

        public const string Template_METHOD_CALL_PASSTHROUGH_ARGUMENT = @"@@ARGUMENT_NAME@@";
        public const string Template_METHOD_CALL_PRIVATE_MEMBER_ARGUMENT = @"_@@ARGUMENT_NAME@@";
        // ReSharper restore InconsistentNaming
    }

    public partial class Template
    {
        // ReSharper disable InconsistentNaming
        public const string Template_KEYWORD = @"			public const EventKeywords @@KEYWORD_NAME@@ = (EventKeywords)0x@@KEYWORD_INDEX@@L;";
        public const string Template_KEYWORD_NAME = @"@@KEYWORD_NAME@@";
        public const string Template_KEYWORD_INDEX = @"@@KEYWORD_INDEX@@";
        // ReSharper restore InconsistentNaming
    }

    public partial class Template
    {
        // ReSharper disable InconsistentNaming
        public const string Template_EXTENSION_CLRTYPE = @"@@EXTENSION_CLRTYPE@@";

        public const string Template_EXTENSION_ASJSON_DECLARATION = @"
            public static string AsJson(this @@EXTENSION_CLRTYPE@@ that)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(that);
            }
";

        public const string Template_EXTENSION_GETREPLICAORINSTANCEID_DECLARATION = @"
            public static long GetReplicaOrInstanceId(this System.Fabric.ServiceContext context)
            {
                var stateless = context as System.Fabric.StatelessServiceContext;
                if (stateless != null)
                {
                    return stateless.InstanceId;
                }

                var stateful = context as System.Fabric.StatefulServiceContext;
                if (stateful != null)
                {
                    return stateful.ReplicaId;
                }

                throw new NotSupportedException(""Context type not supported."");
            }
";

        public const string Template_EXTENSION_GETCONTENTDIGEST_DECLARATION = @"
            public static long GetContentDigest(this string content)
            {
                var contentDigest = """";
                try
                {
    				var hash = content.GetMD5Hash();
                    var length = content?.Length ?? 0;
                    contentDigest = $""{content?.Substring(0, 30)?.Replace(""\r"", """")?.Replace(""\n"", """")}... ({length}) [{hash}]"";
                }
                catch (Exception ex)
                {
                    contentDigest = $""Failed to generate digest {ex.Message}"";
                }
                return contentDigest;
            }
";

        public const string Template_EXTENSION_GETMD5HASH_DECLARATION = @"
		    public static string GetMD5Hash(this string input)
		    {
			    var md5Hasher = MD5.Create();
			    var data = md5Hasher?.ComputeHash(Encoding.UTF8.GetBytes(input));
			    var hexStringBuilder = new StringBuilder();
			    for (var i = 0; i < (data?.Length); i++)
			    {
				    hexStringBuilder.Append(data[i].ToString(""x2""));
			    }
			    return hexStringBuilder.ToString();
		    }
";
        // ReSharper restore InconsistentNaming
    }
}