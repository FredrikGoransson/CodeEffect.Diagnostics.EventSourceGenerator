using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeEffect.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Remoting;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Builder;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;

namespace CodeEffect.ServiceFabric.Actors.Remoting.FabricTransport
{
    public static class FabricTransportActorRemotingHelpers
    {
        internal static ActorRemotingProviderAttribute GetProvider(IEnumerable<Type> types = null)
        {
            if (types != null)
            {
                foreach (var t in types)
                {
                    var attribute = t.GetTypeInfo().Assembly.GetCustomAttribute<ActorRemotingProviderAttribute>();
                    if (attribute != null)
                    {
                        return attribute;
                    }
                }
            }
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                var attribute = assembly.GetCustomAttribute<ActorRemotingProviderAttribute>();
                if (attribute != null)
                {
                    return attribute;
                }
            }
            return new FabricTransport.FabricTransportActorRemotingProviderAttribute();
        }

        public static IEnumerable<IExceptionHandler> GetExceptionHandlers(Type actorInterfaceType, params Type[] additionalTypes)
        {
            var types = new[] {actorInterfaceType}.Union(additionalTypes);
            foreach (var t in types)
            {
                var attribute = t.GetTypeInfo().Assembly.GetCustomAttribute<FabricTransportRemotingExceptionHandlerAttribute>();
                IExceptionHandler instance = null;
                if (attribute != null)
                {
                    var exceptionHandlerType = attribute.ExceptionHandlerType;
                    if (exceptionHandlerType.GetInterface(nameof(IExceptionHandler), false) != null)
                    {
                        try
                        {
                            instance = Activator.CreateInstance(exceptionHandlerType) as IExceptionHandler;
                            
                        }
                        catch (Exception ex)
                        {
                            // TODO: Log this?
                        }
                    }
                }
                if (instance != null)
                {
                    yield return instance;
                }
            }            
        }

        public static IServiceRemotingClientFactory CreateServiceRemotingClientFactory(Type interfaceType, IServiceRemotingCallbackClient callbackClient, IServiceClientLogger logger, string correlationId, MethodDispatcherBase actorMethodDispatcher, MethodDispatcherBase serviceMethodDispatcher)
        {
            var fabricTransportSettings = GetDefaultFabricTransportSettings("TransportSettings");
            var exceptionHandlers = GetExceptionHandlers(interfaceType);
            return
                (IServiceRemotingClientFactory) new CodeEffect.ServiceFabric.Actors.Remoting.FabricTransport.Client.FabricTransportActorRemotingClientFactory(
                    new FabricTransportActorRemotingClientFactory(
                        fabricTransportSettings,
                        callbackClient,
                        (IServicePartitionResolver) null,
                        exceptionHandlers,
                        traceId: correlationId), 
                    logger, 
                    actorMethodDispatcher,
                    serviceMethodDispatcher);
        }
        

        /// <summary>
        ///  FabricTransportSettings returns the default Settings .Loads the configuration file from default Config Package"Config" , if not found then try to load from  default config file "ClientExeName.Settings.xml"  from Client Exe directory.
        /// </summary>
        /// <param name="sectionName">Name of the section within the configuration file. If not found section in configuration file, it will return the default Settings</param>
        /// <returns></returns>
        private static FabricTransportRemotingSettings GetDefaultFabricTransportSettings(string sectionName = "TransportSettings")
        {
            FabricTransportRemotingSettings settings = (FabricTransportRemotingSettings)null;
            if (!FabricTransportRemotingSettings.TryLoadFrom(sectionName, out settings, (string)null, (string)null))
            {
                settings = new FabricTransportRemotingSettings();
            }
            return settings;
        }

        internal static FabricTransportRemotingListenerSettings GetActorListenerSettings(ActorService actorService)
        {
            FabricTransportRemotingListenerSettings listenerSettings;
            if (!FabricTransportRemotingListenerSettings.TryLoadFrom(ActorNameFormat.GetFabricServiceTransportSettingsSectionName(actorService.ActorTypeInformation.ImplementationType), out listenerSettings, (string)null))
                listenerSettings = GetDefaultFabricTransportListenerSettings("TransportSettings");
            return listenerSettings;
        }

        internal static FabricTransportRemotingListenerSettings GetDefaultFabricTransportListenerSettings(string sectionName = "TransportSettings")
        {
            FabricTransportRemotingListenerSettings listenerSettings = (FabricTransportRemotingListenerSettings)null;
            if (!FabricTransportRemotingListenerSettings.TryLoadFrom(sectionName, out listenerSettings, (string)null))
            {
                listenerSettings = new FabricTransportRemotingListenerSettings();
            }
            return listenerSettings;
        }
    }
}