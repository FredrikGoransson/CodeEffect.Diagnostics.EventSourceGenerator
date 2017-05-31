﻿using System;
using System.Collections.Generic;
using CodeEffect.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

namespace CodeEffect.ServiceFabric.Actors.FabricTransport.Diagnostics

{
    public interface IServiceCommunicationLogger : IServiceClientLogger
    {
        void StartMessageRecieved(string methodName, CustomServiceRequestHeader headers);
        void StopMessageRecieved(string methodName, CustomServiceRequestHeader headers);

        void MessageDispatched(string methodName, CustomServiceRequestHeader headers);

        void MessageFailed(string methodName, CustomServiceRequestHeader headers, Exception ex);

        void MessageHandled(string methodName, CustomServiceRequestHeader headers);

        void FailedToGetServiceMethodName(int interfaceId, int methodId, Exception ex);

        void FailedToReadCustomServiceMessageHeader(ServiceRemotingMessageHeaders serviceRemotingMessageHeaders, Exception ex);
    }


}