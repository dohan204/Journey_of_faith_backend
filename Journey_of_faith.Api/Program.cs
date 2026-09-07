// using Dapper;
// using Journey_of_faith.Api.cache;
// using Journey_of_faith.Api.middlewares;
// using Journey_of_faith.Application;
// using Journey_of_faith.Infrastructure;
// using Journey_of_faith.Infrastructure.persistence.entities.events;
// using Journey_of_faith.Infrastructure.persistence.entities.location;
// using Microsoft.Extensions.FileProviders;
// using Microsoft.OpenApi;
// using Microsoft.Extensions.DependencyInjection;

// using System.Data;
// using static Microsoft.Extensions.DependencyInjection.SchemaRequestExecutorBuilderExtensions;
// using System.Diagnostics;
// using Journey_of_faith.Infrastructure.context;
// using OfficeOpenXml;
// var builder = WebApplication.CreateBuilder(args);

// SqlMapper.AddTypeHandler(new GuidTypeHandler());
// builder.Services.AddHttpContextAccessor();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: "allowFrontend", policy =>
//         policy.AllowAnyOrigin()
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//     );
// });
// // Add services to the container.

// // builder.Services.AddFirebaseService(builder.Configuration);
// builder.Services.AddInfrastructure(builder.Configuration);
// builder.Services.AddRegisterService(builder.Configuration);
// builder.Services.AddApplication();
// builder.Services.AddAutoMapperConfig();
// builder.Services.AddControllers();
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// builder.Services.AddProblemDetails();
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// // builder.Services.AddGraphQLExtension();
// // builder.Services.AddGraphQLServer()
// //     .AddQueryType<Query>()
// //     .AddTypeExtension<UserNode>()
// //     .AddTypeExtension<UserQueryResolver>()
// //     .AddTypeExtension<ChurchNode>()
// //     .AddTypeExtension<ChurchQueryResolver>()
// //     .AddTypeExtension<UserSongExtenstion>()
// //     .AddDataLoader<UserCacheDataLoader>()
// //     .AddDataLoader<EventCommentByUserIdDataLoader>()
// //     .AddDataLoader<ReminderSettingByUserIdDataLoader>()
// //     .AddDataLoader<EventFollowerByUserIdDataLoader>()
// //     .AddDataLoader<QuizAttemptByUserIdDataLoader>()
// //     .AddDataLoader<PrayCommentByUserIdDataLoader>()
// //     .AddDataLoader<ChurchCacheDataLoader>()
// //     .AddDataLoader<GetDioceseByIdDataLoader>()
// //     .AddDataLoader<SongByIdsDataLoader>()
// //     .AddMutationType<Mutation>()
// //     .AddErrorFilter<GlobalErrorFilter>()
// //     .AddFiltering()
// //     // .UsePersistedOperationPipeline()
// //     // .AddFileSystemOperationDocumentStorage("./persisted_operations")

// //     .AddHttpResponseFormatter<CustomHttpResponseFormatter>()
// //     .AddHttpRequestInterceptor<CustomHttpRequestInterceptor>();
// //     // .ModifyRequestOptions(opt => opt.PersistedOperations.OnlyAllowPersistedDocuments = true);


// ExcelPackage.License.SetNonCommercialOrganization("JourneyOfFaith");

// // builder.Services.AddScoped<CustomHttpResponseFormatter>();
// var app = builder.Build();


// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//     _ = dbContext.Model;

//     await dbContext.Database.CanConnectAsync();
// }

// app.Use(async (context, next) =>
// {
//     var sw = Stopwatch.StartNew();
//     Console.WriteLine(context.Request.Headers.Authorization);
//     // string tokenConvert = context.Request.Headers.Authorization;
//     // if (tokenConvert != null)
//     // {
//     //     string tokenConcat = tokenConvert.Substring(6);
//     //     Console.WriteLine("Token convert: {0}", tokenConcat);
//     //     context.Request.Headers.Authorization = tokenConcat;
//     // }
//     await next(context);

//     sw.Stop();

//     Console.WriteLine($"Request time: {sw.ElapsedMilliseconds}ms");
// });
// // Configure the HTTP request pipeline.
// app.MapOpenApi();
// app.UseSwaggerUI(options =>
// {
//     options.SwaggerEndpoint("/openapi/v1.json", "v1");
// });


// app.UseCors("allowFrontend");
// app.UseExceptionHandler();
// app.UseHttpsRedirection();
// var currentDirectoryFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "uploads");
// if (!Directory.Exists(currentDirectoryFile))
// {
//     Directory.CreateDirectory(currentDirectoryFile);
// }
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(currentDirectoryFile),
//     RequestPath = "/uploads"
// });

// if (app.Environment.IsDevelopment())
// {

//     // app.MapNitroApp("/graphql/ui");
// }
// app.UseAuthentication();
// app.UseAuthorization();


// // app.MapGraphQL();
// app.MapControllers();

// app.Run();



// public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
// {
//     public override void SetValue(IDbDataParameter parameter, Guid value)
//     {
//         parameter.Value = value.ToString();
//     }

//     public override Guid Parse(object value)
//     {
//         return Guid.Parse(value.ToString()!);
//     }
// }

// public partial class Program { }
using Dapper;
using Journey_of_faith.Api.cache;
using Journey_of_faith.Api.middlewares;
using Journey_of_faith.Application;
using Journey_of_faith.Infrastructure;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.persistence.entities.events;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using OfficeOpenXml;

using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

SqlMapper.AddTypeHandler(new GuidTypeHandler());

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowFrontend", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

// Add services to the container.

// builder.Services.AddFirebaseService(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFirebaseService(builder.Configuration);
builder.Services.AddRegisterService(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddAutoMapperConfig();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader()  
    );
}).AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// builder.Services.AddGraphQLExtension();

// builder.Services.AddGraphQLServer()
//     .AddQueryType<Query>()
//     .AddTypeExtension<UserNode>()
//     .AddTypeExtension<UserQueryResolver>()
//     .AddTypeExtension<ChurchNode>()
//     .AddTypeExtension<ChurchQueryResolver>()
//     .AddTypeExtension<UserSongExtenstion>()
//     .AddDataLoader<UserCacheDataLoader>()
//     .AddDataLoader<EventCommentByUserIdDataLoader>()
//     .AddDataLoader<ReminderSettingByUserIdDataLoader>()
//     .AddDataLoader<EventFollowerByUserIdDataLoader>()
//     .AddDataLoader<QuizAttemptByUserIdDataLoader>()
//     .AddDataLoader<PrayCommentByUserIdDataLoader>()
//     .AddDataLoader<ChurchCacheDataLoader>()
//     .AddDataLoader<GetDioceseByIdDataLoader>()
//     .AddDataLoader<SongByIdsDataLoader>()
//     .AddMutationType<Mutation>()
//     .AddErrorFilter<GlobalErrorFilter>()
//     .AddFiltering()
//     // .UsePersistedOperationPipeline()
//     // .AddFileSystemOperationDocumentStorage("./persisted_operations")
//     .AddHttpResponseFormatter<CustomHttpResponseFormatter>()
//     .AddHttpRequestInterceptor<CustomHttpRequestInterceptor>();
//     // .ModifyRequestOptions(opt => opt.PersistedOperations.OnlyAllowPersistedDocuments = true);

ExcelPackage.License.SetNonCommercialOrganization("JourneyOfFaith");

// builder.Services.AddScoped<CustomHttpResponseFormatter>();

var app = builder.Build();


// ==========================================================
// DATABASE CHECK
// ==========================================================
// Khi chạy Integration Test với Environment = "Testing",
// không thực hiện CanConnectAsync() ở đây.
// CustomWebApplicationFactory sẽ cấu hình InMemory Database.
// ==========================================================

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _ = dbContext.Model;

        await dbContext.Database.CanConnectAsync();
    }
}


// ==========================================================
// REQUEST LOGGING
// ==========================================================

app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();

    Console.WriteLine(context.Request.Headers.Authorization);

    // string tokenConvert = context.Request.Headers.Authorization;

    // if (tokenConvert != null)
    // {
    //     string tokenConcat = tokenConvert.Substring(6);
    //     Console.WriteLine("Token convert: {0}", tokenConcat);
    //     context.Request.Headers.Authorization = tokenConcat;
    // }

    await next();

    sw.Stop();

    Console.WriteLine($"Request time: {sw.ElapsedMilliseconds}ms");
});


// ==========================================================
// HTTP REQUEST PIPELINE
// ==========================================================

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseCors("allowFrontend");

app.UseExceptionHandler();

app.UseHttpsRedirection();


// ==========================================================
// UPLOADS
// ==========================================================

var currentDirectoryFile =
    System.IO.Path.Combine(
        Directory.GetCurrentDirectory(),
        "uploads"
    );

if (!Directory.Exists(currentDirectoryFile))
{
    Directory.CreateDirectory(currentDirectoryFile);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(currentDirectoryFile),
    RequestPath = "/uploads"
});


// ==========================================================
// DEVELOPMENT
// ==========================================================

if (app.Environment.IsDevelopment())
{
    // app.MapNitroApp("/graphql/ui");
}


// ==========================================================
// AUTHENTICATION / AUTHORIZATION
// ==========================================================

app.UseAuthentication();

app.UseAuthorization();


// ==========================================================
// CONTROLLERS
// ==========================================================

// app.MapGraphQL();

app.MapControllers();


// ==========================================================
// RUN
// ==========================================================

app.Run();


// ==========================================================
// GUID TYPE HANDLER
// ==========================================================

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


// ==========================================================
// PROGRAM CLASS FOR INTEGRATION TESTING
// ==========================================================

public partial class Program
{
}