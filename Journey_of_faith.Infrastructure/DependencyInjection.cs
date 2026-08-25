using HotChocolate.Execution;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.graphql;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.churches;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.users;
using Journey_of_faith.Infrastructure.graphql.Resolvers;
using Journey_of_faith.Infrastructure.graphql.types;
// using Journey_of_faith.Infrastructure.graphql.types.churches;
using Journey_of_faith.Infrastructure.graphql.types.quizes;
// using Journey_of_faith.Infrastructure.graphql.types.songs;
using Journey_of_faith.Infrastructure.graphql.types.users;
using Journey_of_faith.Infrastructure.identity;
using Journey_of_faith.Infrastructure.identity.services;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Journey_of_faith.Infrastructure.repositories;
using Journey_of_faith.Infrastructure.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using FirebaseAdmin;
using System.Text;
using Google.Apis.Auth.OAuth2;
using System.IdentityModel.Tokens.Jwt;

namespace Journey_of_faith.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"), sqlServerOptionsAction: sqloption =>
                {
                    sqloption.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(20),
                        errorNumbersToAdd: null

                    );
                });
            });

            service.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            service.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;

                // sign in 
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetValue<string>("Token:Issuer"),
                        ValidAudience = configuration.GetValue<string>("Token:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token:Key") ?? string.Empty))
                    };
                });

            return service;
        }
    }


    public static class RegisterService
    {
        public static IServiceCollection AddRegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<TableSchemaName>(
                configuration.GetSection("Db")
            );
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileStorageService, FileStorageQuestion>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IChurchRepository, ChurchRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped(typeof(IGetOneToOneData<,>), typeof(GetDataRepository<,>));
            services.AddScoped(typeof(IGetOneToManyData<,>), typeof(GetDataRepository<,>));
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IDataHandler, DataHandlerRequest>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
            services.AddScoped<IFirebaseNotification, FirebaseNotification>();
            return services;
        }
    }



    public static class RegisterAutoMapper
    {
        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Journey_of_faith.Infrastructure.identity.ApplicationUser, Journey_of_faith.Domain.entities.User>().ReverseMap();
                cfg.CreateMap<UserChurch, UserChurch>().ReverseMap();
                cfg.CreateMap<Diocese, Domain.entities.location.Diocese>().ReverseMap();
                cfg.CreateMap<Church, Domain.entities.location.Church>().ReverseMap();
                cfg.CreateMap<persistence.entities.music.Song, Domain.entities.musics.Song>().ReverseMap();
                cfg.CreateMap<persistence.entities.music.Artist, Domain.entities.musics.Artist>().ReverseMap();
                cfg.CreateMap<persistence.entities.quiz.Quiz, Domain.entities.quiz.Quiz>().ReverseMap();
                cfg.CreateMap<persistence.entities.quiz.Topic, Domain.entities.quiz.Topic>().ReverseMap();
            });

            return services;
        }
    }


    // public static class RegisterGraphQL
    // {
    //     public static IServiceCollection AddGraphQLExtension(this IServiceCollection services)
    //     {
    //         services.AddGraphQLServer()


    //             .AddQueryType(typeof(Query))
    //             .AddTypeExtension(typeof(UserNodeResolver))
    //             .AddTypeExtension(typeof(UserChurches))
    //             .AddDataLoader<IChurchsByUserIdDataLoader, ChurchsByUserIdDataLoader>()
    //             .AddTypeExtension(typeof(TopicNodeResolver))
    //             .AddTypeExtension(typeof(QuizNodeResovler))
    //             .AddTypeExtension(typeof(QuizQueryExtension))
    //             .AddTypeExtension(typeof(QuestionQueryExtension))
    //             .AddTypeExtension(typeof(AnswerQueryExtension))
    //             .AddDataLoader<IQuestionByQuizDataLoader, QuestionByQuizDataLoader>()
    //             .AddDataLoader<IAnswerByQuestionDataLoader, AnswerByQuestionDataLoader>()
    //             .AddDataLoader<IQuizByTopicDataLoader, QuizByTopicDataLoader>()


    //             .AddTypeExtension(typeof(SongNodeResolver))
    //             .AddTypeExtension(typeof(ArtistNodeResolver))
    //             .AddTypeExtension(typeof(ArtistExtension))
    //             .AddTypeExtension(typeof(SongCategoryExtension))
    //             .AddTypeExtension(typeof(UserSongAysnc))
    //             .AddTypeExtension(typeof(AlbumQueryExtension))
    //             .AddTypeExtension(typeof(ArtistQueryExtension))
    //             .AddTypeExtension(typeof(SongCategoryExtension))


    //             .AddTypeExtension(typeof(ChurchNodeResolver))
    //             .AddTypeExtension(typeof(DioceseNodeResolver))
    //             .AddTypeExtension(typeof(DioceseQueryExtension))
    //             .AddTypeExtension(typeof(ChurchQueryExtension))
    //             .AddTypeExtension(typeof(MassScheduleQueryExtension))
    //             .AddTypeExtension(typeof(UserChurchQueryExtension))


    //             .AddDataLoader<ISongsByUserIdDataLoader, SongsByUserIdDataLoader>()
    //             .AddDataLoader<IAlbumDataLoader, AlbumDataLoader>()
    //             .AddDataLoader<IArtistDataLoader, ArtistDataLoader>()
    //             .AddDataLoader<ICategoryByIdDataLoader, CategoryByIdDataLoader>()
    //             .AddDataLoader<ISongByArtistDataLoader, SongByArtistDataLoader>()
    //             .AddDataLoader<ISongByCategoryDataLoader, SongByCategoryDataLoader>()
    //             .AddDataLoader<IMassSchedulesDataLoader, MassSchedulesDataLoader>()
    //             .AddDataLoader<IDioceseByChurchDataLoader, DioceseByChurchDataLoader>()
    //             .AddDataLoader<IUserChurchByMappingDataLoader, UserChurchByMappingDataLoader>()
    //             .AddDataLoader<IChurchesDataLoader, ChurchesDataLoader>()


    //             .AddFiltering()
    //             .AddSorting()
    //             .AddCacheControl()
    //             .AddWarmupTask(async (executor, cancellationToken) =>
    //             {
    //                 var request = OperationRequestBuilder.New()
    //                     .SetDocument("{ __typename }")
    //                     .MarkAsWarmupRequest()
    //                     .Build();
    //                 await executor.ExecuteAsync(request, cancellationToken: cancellationToken);
    //             });
    //         return services;
    //     }
    // }

    // public static class RegisterFirebase
    // {
    //     public static IServiceCollection AddFirebaseService(this IServiceCollection services, IConfiguration configuration)
    //     {
    //         var credentialPath = configuration.GetValue<string>("Firebase:CredentialFilePath");
    //         if (!string.IsNullOrEmpty(credentialPath))
    //         {
    //             var fullPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), credentialPath);
    //             if (!File.Exists(fullPath))
    //             {
    //                 throw new FileNotFoundException($"Firebase credential file not found at: {fullPath}");
    //             }
    //             if (FirebaseApp.DefaultInstance == null)
    //             {
    //                 var credential = CredentialFactory
    //                     .FromFile<ServiceAccountCredential>(fullPath)
    //                     .ToGoogleCredential();
    //                 FirebaseApp.Create(new AppOptions()
    //                 {
    //                     Credential = credential
    //                 });
    //             }
    //         }
    //         return services;
    //     }
    // }
}
