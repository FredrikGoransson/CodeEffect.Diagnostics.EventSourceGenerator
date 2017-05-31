﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using CodeEffect.ServiceFabric.Actors.FabricTransport.Diagnostics;
using CodeEffect.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace CodeEffect.ServiceFabric.Actors.Remoting.FabricTransport.Client
{
    public class FabricTransportActorRemotingClientFactory : IServiceRemotingClientFactory, ICommunicationClientFactory<IServiceRemotingClient>
    {
        private readonly ICommunicationClientFactory<IServiceRemotingClient> _innerClientFactory;
        private readonly IServiceCommunicationLogger _logger;

        public FabricTransportActorRemotingClientFactory(ICommunicationClientFactory<IServiceRemotingClient> innerClientFactory, IServiceCommunicationLogger logger)
        {
            _innerClientFactory = innerClientFactory;
            _logger = logger;
            _innerClientFactory.ClientConnected += OnClientConnected;
            _innerClientFactory.ClientDisconnected += OnClientDisconnected;
        }

        public async Task<IServiceRemotingClient> GetClientAsync(Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName,
            OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            var client = await _innerClientFactory.GetClientAsync(serviceUri, partitionKey, targetReplicaSelector, listenerName, retrySettings, cancellationToken);
            return new FabricTransportActorRemotingClient(client, serviceUri, _logger);
        }

        public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings,
            CancellationToken cancellationToken)
        {
            var client = await _innerClientFactory.GetClientAsync(previousRsp, targetReplicaSelector, listenerName, retrySettings, cancellationToken);
            return new FabricTransportActorRemotingClient(client, previousRsp.ServiceName, _logger);
        }

        public Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings,
            CancellationToken cancellationToken)
        {
            return _innerClientFactory.ReportOperationExceptionAsync(client, exceptionInformation, retrySettings, cancellationToken);
        }

        private void OnClientConnected(object sender, CommunicationClientEventArgs<IServiceRemotingClient> e)
        {
            this.ClientConnected?.Invoke((object)this, new CommunicationClientEventArgs<IServiceRemotingClient>()
            {
                Client = e.Client
            });
        }

        private void OnClientDisconnected(object sender, CommunicationClientEventArgs<IServiceRemotingClient> e)
        {
            this.ClientDisconnected?.Invoke((object)this, new CommunicationClientEventArgs<IServiceRemotingClient>()
            {
                Client = e.Client
            });
        }

        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;
        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;
    }
}