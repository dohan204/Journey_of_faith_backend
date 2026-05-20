using Dapper;
using Journey_of_faith.Api.cache;
using Journey_of_faith.Api.middlewares;
using Journey_of_faith.Api.types;
using Journey_of_faith.Api.types.data;
using Journey_of_faith.Api.types.extensions;
using Journey_of_faith.Api.types.filter;
using Journey_of_faith.Api.types.resolvers;
using Journey_of_faith.Application;
using Journey_of_faith.Infrastructure;
using Journey_of_faith.Infrastructure.persistence.entities.events;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;
using Microsoft.Extensions.DependencyInjection;

using System.Data;
using static Microsoft.Extensions.DependencyInjection.SchemaRequestExecutorBuilderExtensions;
var builder = WebApplication.CreateBuilder(args);

SqlMapper.AddTypeHandler(new GuidTypeHandler());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowFrontend", policy =>
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});
// Add services to the container.


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRegisterService(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAutoMapperConfig();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddTypeExtension<UserNode>()
    .AddTypeExtension<UserQueryResolver>()
    .AddTypeExtension<ChurchNode>()
    .AddTypeExtension<ChurchQueryResolver>()
    .AddDataLoader<UserCacheDataLoader>()
    .AddDataLoader<EventCommentByUserIdDataLoader>()
    .AddDataLoader<ReminderSettingByUserIdDataLoader>()
    .AddDataLoader<EventFollowerByUserIdDataLoader>()
    .AddDataLoader<QuizAttemptByUserIdDataLoader>()
    .AddDataLoader<PrayCommentByUserIdDataLoader>()
    .AddDataLoader<ChurchCacheDataLoader>()
    .AddDataLoader<GetDioceseByIdDataLoader>()
    .AddMutationType<Mutation>()
    .AddErrorFilter<GlobalErrorFilter>()
    .AddFiltering()
    .UsePersistedOperationPipeline()
    .AddFileSystemOperationDocumentStorage("./persisted_operations")

    .AddHttpResponseFormatter<CustomHttpResponseFormatter>()
    .AddHttpRequestInterceptor<CustomHttpRequestInterceptor>()
    .ModifyRequestOptions(opt => opt.PersistedOperations.OnlyAllowPersistedDocuments = true);



// builder.Services.AddScoped<CustomHttpResponseFormatter>();
var app = builder.Build();


app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path);
    await next();
    Console.WriteLine(context.Response.StatusCode);
});
// Configure the HTTP request pipeline.
  app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });


app.UseCors("allowFrontend");
app.UseExceptionHandler();
app.UseHttpsRedirection();
var currentDirectoryFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if(!Directory.Exists(currentDirectoryFile))
{
    Directory.CreateDirectory(currentDirectoryFile);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(currentDirectoryFile),
    RequestPath = "/uploads"
});

if(app.Environment.IsDevelopment())
{

app.MapNitroApp("/graphql/ui");
}
app.UseAuthentication();
app.UseAuthorization();


app.MapGraphQL();
app.MapControllers();

app.Run();



public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }

    public override Guid Parse(object value)
    {
        return Guid.Parse(value.ToString()!);
    }
}
