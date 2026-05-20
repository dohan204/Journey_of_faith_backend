using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace Journey_of_faith.Api.middlewares;

public class CustomHttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, OperationRequestBuilder requestBuilder, CancellationToken cancellationToken)
    {
        if(context.Request.Headers.ContainsKey("X-Developer"))
        {
            requestBuilder.AllowNonPersistedOperation();
        }
        return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}