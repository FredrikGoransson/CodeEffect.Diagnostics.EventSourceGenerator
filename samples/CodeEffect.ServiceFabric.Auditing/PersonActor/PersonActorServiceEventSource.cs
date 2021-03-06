/*******************************************************************************************
*  This class is autogenerated from the class PersonActorServiceEventSource.eventsource.json
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace PersonActor
{
	[EventSource(Name = "CodeEffect-ServiceFabric-Auditing-PersonActorService")]
	internal sealed partial class PersonActorServiceEventSource : EventSource
	{
		public static readonly PersonActorServiceEventSource Current = new PersonActorServiceEventSource();

		static PersonActorServiceEventSource()
		{
			// A workaround for the problem where ETW activities do not 
			// get tracked until Tasks infrastructure is initialized.
			// This problem will be fixed in .NET Framework 4.6.2.
			Task.Run(() => { });
		}

		// Instance constructor is private to enforce singleton semantics
		private PersonActorServiceEventSource() : base() { }

		#region Keywords
		// Event keywords can be used to categorize events. 
		// Each keyword is a bit flag. A single event can be 
		// associated with multiple keywords (via EventAttribute.Keywords property).
		// Keywords must be defined as a public class named 'Keywords' 
		// inside EventSource that uses them.
		public static class Keywords
		{
			public const EventKeywords PersonActor = (EventKeywords)0x1L;
			public const EventKeywords PersonActorService = (EventKeywords)0x2L;
			public const EventKeywords Communication = (EventKeywords)0x4L;

		}
		#endregion Keywords

		#region Events



		#endregion Events
	}


	internal static class PersonActorServiceEventSourceHelpers
	{

            public static string AsJson(this System.Exception that)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(that);
            }


	}

}