using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Graphite.WCF
{
    public class OperationTimingEndpointBehavior : IEndpointBehavior
    {
        private readonly StatsClient _client;

        public OperationTimingEndpointBehavior(StatsClient client)
        {
            _client = client;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            foreach(var operation in clientRuntime.Operations)
            {
                operation.ParameterInspectors.Add(
                    new OperationTimingParamaterInspector(_client, endpoint.Contract.Name));
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                operation.CallContextInitializers.Add(
                    new OperationTimingCallContextInitializer(_client, operation.Name, endpoint.Contract.Name));
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}