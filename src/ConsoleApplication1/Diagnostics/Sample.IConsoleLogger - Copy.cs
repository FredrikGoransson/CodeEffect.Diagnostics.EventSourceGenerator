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

		[Event(SayHelloEventId, Level = EventLevel.LogAlways, Message = "{4}", Keywords = Keywords.Console)]
		private void SayHello(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName, 
			string message)
		{
			WriteEvent(
				SayHelloEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName, 
				message);
		}

		[NonEvent]
		public void SayHello(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName, 
			string message)
		{
			if (this.IsEnabled())
			{
				SayHello(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName, 
					message);
			}
		}


		private const int MessageEventId = 2002;

		[Event(MessageEventId, Level = EventLevel.LogAlways, Message = "{4}", Keywords = Keywords.Console)]
		private void Message(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName, 
			string message)
		{
			WriteEvent(
				MessageEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName, 
				message);
		}

		[NonEvent]
		public void Message(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName, 
			string message)
		{
			if (this.IsEnabled())
			{
				Message(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName, 
					message);
			}
		}


		private const int ErrorEventId = 3003;

		[Event(ErrorEventId, Level = EventLevel.LogAlways, Message = "{4}", Keywords = Keywords.Console | Keywords.Error)]
		private void Error(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName, 
			string message, 
			string source, 
			string exceptionTypeName, 
			string exception)
		{
			WriteEvent(
				ErrorEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName, 
				message, 
				source, 
				exceptionTypeName, 
				exception);
		}

		[NonEvent]
		public void Error(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName, 
			System.Exception exception)
		{
			if (this.IsEnabled())
			{
				Error(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName, 
					exception.Message, 
					exception.Source, 
					exception.GetType().FullName, 
					exception.AsJson());
			}
		}


		private const int SayGoodbyeEventId = 4004;

		[Event(SayGoodbyeEventId, Level = EventLevel.LogAlways, Message = "Say Goodbye {4} {5}", Keywords = Keywords.Console)]
		private void SayGoodbye(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName, 
			string goodbye, 
			string nightTime)
		{
			WriteEvent(
				SayGoodbyeEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName, 
				goodbye, 
				nightTime);
		}

		[NonEvent]
		public void SayGoodbye(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName, 
			string goodbye, 
			System.DateTime nightTime)
		{
			if (this.IsEnabled())
			{
				SayGoodbye(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName, 
					goodbye, 
					nightTime.ToString());
			}
		}


		private const int SpeciallyEventId = 5005;

		[Event(SpeciallyEventId, Level = EventLevel.LogAlways, Message = "Specially {4}", Keywords = Keywords.Console)]
		private void Specially(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName, 
			string special)
		{
			WriteEvent(
				SpeciallyEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName, 
				special);
		}

		[NonEvent]
		public void Specially(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName, 
			ConsoleApplication1.Loggers.Special special)
		{
			if (this.IsEnabled())
			{
				Specially(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName, 
					special.ToString());
			}
		}


		private const int StartHelloEventId = 6006;

		[Event(StartHelloEventId, Level = EventLevel.LogAlways, Message = "Start Hello", Keywords = Keywords.Console, Opcode = EventOpcode.Start)]
		private void StartHello(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName)
		{
			WriteEvent(
				StartHelloEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName);
		}

		[NonEvent]
		public void StartHello(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName)
		{
			if (this.IsEnabled())
			{
				StartHello(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName);
			}
		}


		private const int StopHelloEventId = 7007;

		[Event(StopHelloEventId, Level = EventLevel.LogAlways, Message = "Stop Hello", Keywords = Keywords.Console, Opcode = EventOpcode.Stop)]
		private void StopHello(
			string actorId, 
			string actorIdType, 
			int processId, 
			string machineName)
		{
			WriteEvent(
				StopHelloEventId, 
				actorId, 
				actorIdType, 
				processId, 
				machineName);
		}

		[NonEvent]
		public void StopHello(
			Microsoft.ServiceFabric.Actors.ActorId actorId, 
			int processId, 
			string machineName)
		{
			if (this.IsEnabled())
			{
				StopHello(
					actorId.ToString(), 
					actorId.Kind.ToString(), 
					processId, 
					Environment.MachineName);
			}
		}


	}
}