using System.Net;
using HotChocolate.AspNetCore.Formatters;
using HotChocolate.Execution;

namespace Journey_of_faith.Api.middlewares;


public class CustomHttpResponseFormatter: DefaultHttpResponseFormatter
{
    public CustomHttpResponseFormatter() : base(new HttpResponseFormatterOptions
    {
        Json = new HotChocolate.Transport.Formatters.JsonResultFormatterOptions
        {
            // NullIgnoreCondition = HotChocolate.Text.Json.JsonNullIgnoreCondition.FieldsAndLists,
            Indented = true
        }
    })
    {
    }
    protected override HttpStatusCode OnDetermineStatusCode(OperationResult result, FormatInfo format, HttpStatusCode? proposedStatusCode)
    {
        if(result.Errors?.Any() == true)
        {
            if(result.Errors.Any(e => e.Code == "AUTH_NOT_AUTHENTICATED"))
            {
                return HttpStatusCode.Unauthorized;
            }

            if(result.Errors.Any(e => e.Code == "DATA_NOT_FOUND"))
            {
                return HttpStatusCode.NotFound;
            }       
        }
        return base.OnDetermineStatusCode(result, format, proposedStatusCode);
    }
}