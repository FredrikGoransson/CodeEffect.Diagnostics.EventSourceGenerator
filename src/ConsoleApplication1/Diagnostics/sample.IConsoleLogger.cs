/*******************************************************************************************
*  This class is autogenerated from the class ConsoleLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace ConsoleApplication1.Diagnostics
{
	internal sealed partial class Sample
	{

		private const int SayHelloEventId = 1001;

		[Event(SayHelloEventId, Level = EventLevel.LogAlways, Message = "{2}", Keywords = Keywords.Console)]
		public void SayHello(
			int processId, 
			string machineName, 
			string message)
		{
			WriteEvent(
				SayHelloEventId, 
				processId, 
				machineName, 
				message);
		}


		private const int MessageEventId = 2002;

		[Event(MessageEventId, Level = EventLevel.LogAlways, Message = "{2}", Keywords = Keywords.Console)]
		public void Message(
			int processId, 
			string machineName, 
			string message)
		{
			WriteEvent(
				MessageEventId, 
				processId, 
				machineName, 
				message);
		}


		private const int ErrorEventId = 3003;

		[Event(ErrorEventId, Level = EventLevel.LogAlways, Message = "{2}", Keywords = Keywords.Console | Keywords.Error)]
		private void Error(
			int processId, 
			string machineName, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				ErrorEventId, 
				processId, 
				machineName, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void Error(
			int processId, 
			string machineName, 
			System.Exception exception)
		{
			if (this.IsEnabled())
			{
				Error(
					processId, 
					Environment.MachineName, 
					exception.Message, 
					exception.Source, 
					exception.GetType().FullName, 
					exception.AsJson());
			}
		}


		private const int SayGoodbyeEventId = 4004;

		[Event(SayGoodbyeEventId, Level = EventLevel.LogAlways, Message = "Say Goodbye {2} {3}", Keywords = Keywords.Console)]
		private void SayGoodbye(
			int processId, 
			string machineName, 
			string goodbye, 
			string nightTime)
		{
			WriteEvent(
				SayGoodbyeEventId, 
				processId, 
				machineName, 
				goodbye, 
				nightTime);
		}

		[NonEvent]
		public void SayGoodbye(
			int processId, 
			string machineName, 
			string goodbye, 
			System.DateTime nightTime)
		{
			if (this.IsEnabled())
			{
				SayGoodbye(
					processId, 
					Environment.MachineName, 
					goodbye, 
					nightTime.ToString());
			}
		}


		private const int SpecialEventId = 5005;

		[Event(SpecialEventId, Level = EventLevel.LogAlways, Message = "Special {2}", Keywords = Keywords.Console)]
		private void Special(
			int processId, 
			string machineName, 
			string special)
		{
			WriteEvent(
				SpecialEventId, 
				processId, 
				machineName, 
				special);
		}

		[NonEvent]
		public void Special(
			int processId, 
			string machineName, 
			ConsoleApplication1.Loggers.Special special)
		{
			if (this.IsEnabled())
			{
				Special(
					processId, 
					Environment.MachineName, 
					special.ToString());
			}
		}


	}
}