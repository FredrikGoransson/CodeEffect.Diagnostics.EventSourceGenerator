/*******************************************************************************************
*  This class is autogenerated from the class ConsoleRunnerLogger
*  Do not directly update this class as changes will be lost on rebuild.
*******************************************************************************************/
using System;
using ConsoleApplication1.Loggers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using CodeEffect.Diagnostics.EventSourceGenerator.AI;


namespace ConsoleApplication1.Diagnostics
{
	internal sealed class ConsoleRunnerLogger : IConsoleRunnerLogger
	{
		// Hello from extension
		private readonly Microsoft.ApplicationInsights.TelemetryClient _telemetryClient;

		public ConsoleRunnerLogger(
			)
		{
			// Do stuff in the constructor
			
            _telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();
            _telemetryClient.Context.User.Id = Environment.UserName;
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

		}

		public void RunnerCreated(
			)
		{
			Sample.Current.RunnerCreated(
				
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: RunnerCreated");
           
			_telemetryClient.TrackEvent(
	            nameof(RunnerCreated),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                
	            });
    
		}




		public void RunnerDestroyed(
			)
		{
			Sample.Current.RunnerDestroyed(
				
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: RunnerDestroyed");
           
			_telemetryClient.TrackEvent(
	            nameof(RunnerDestroyed),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                
	            });
    
		}




		public void WaitingForKeyPress(
			)
		{
			Sample.Current.WaitingForKeyPress(
				
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: WaitingForKeyPress");
           
			_telemetryClient.TrackEvent(
	            nameof(WaitingForKeyPress),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                
	            });
    
		}




		public void KeyPressed(
			System.ConsoleKey key)
		{
			Sample.Current.KeyPressed(
				key
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: KeyPressed");
           
			System.Diagnostics.Debug.WriteLine($"\tkey.ToString():\t{key.ToString()}");
			_telemetryClient.TrackEvent(
	            nameof(KeyPressed),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"Key", key.ToString()}
	            });
    
		}




		public void UnsupportedKeyError(
			System.Exception ex)
		{
			Sample.Current.UnsupportedKeyError(
				ex
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner, Error] ERR: UnsupportedKeyError");
           
			System.Diagnostics.Debug.WriteLine($"\tex.Message:\t{ex.Message}");
			System.Diagnostics.Debug.WriteLine($"\tex.Source:\t{ex.Source}");
			System.Diagnostics.Debug.WriteLine($"\tex.GetType().FullName:\t{ex.GetType().FullName}");
			System.Diagnostics.Debug.WriteLine($"\tex.AsJson():\t{ex.AsJson()}");
			_telemetryClient.TrackException(
	            ex,
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
                    { "Name", "UnsupportedKeyError" },
	                {"Message", ex.Message},
                    {"Source", ex.Source},
                    {"ExceptionTypeName", ex.GetType().FullName},
                    {"Exception", ex.AsJson()}
	            });
    
		}




		public void StartLoop(
			)
		{
			Sample.Current.StartLoop(
				
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: StartLoop");
           
			_loopStopwatch.Restart();
            var loopOperationHolder = _telemetryClient.StartOperation<RequestTelemetry>("loop");
	       
	       OperationHolder.StartOperation(loopOperationHolder);
    
		}

		private System.Diagnostics.Stopwatch _loopStopwatch = new System.Diagnostics.Stopwatch();



		public void StopLoop(
			)
		{
			Sample.Current.StopLoop(
				
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: StopLoop");
           
			_loopStopwatch.Stop();
	        var loopOperationHolder = OperationHolder.StopOperation();
	        _telemetryClient.StopOperation(loopOperationHolder);
	        loopOperationHolder.Dispose();
    
		}




		public void RandomIntsGenerated(
			int[] values)
		{
			Sample.Current.RandomIntsGenerated(
				values
			);

			System.Diagnostics.Debug.WriteLine($"[ConsoleRunner] ERR: RandomIntsGenerated");
           
			System.Diagnostics.Debug.WriteLine($"\tvalues.ToString():\t{values.ToString()}");
			_telemetryClient.TrackEvent(
	            nameof(RandomIntsGenerated),
	            new System.Collections.Generic.Dictionary<string, string>()
	            {
	                {"Values", values.ToString()}
	            });
    
		}




	}
}
