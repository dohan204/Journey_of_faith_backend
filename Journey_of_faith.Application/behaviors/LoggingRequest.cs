using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Journey_of_faith.Application.behaviors
{
    public class LoggingRequestBehavior<TRequest, TResponse> (ILogger<LoggingRequestBehavior<TRequest, TResponse>> _logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest: class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
        {
            var requestId = Guid.NewGuid();

            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation("Start into request mediatR: {0}, id: {id}", requestId, requestJson);
            var response = await next();

            var responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation("Response for {Correlation}: {Response}", requestId, responseJson);
            return response;
        }
    }
}
