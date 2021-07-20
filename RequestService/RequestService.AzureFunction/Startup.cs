﻿using AutoMapper;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Handlers;
using RequestService.Mappers;
using RequestService.Repo;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using RequestService.Core.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using RequestService.Core.Services;
using UserService.Core.Utils;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RequestService.Handlers.BusinessLogic;
using HelpMyStreet.Cache.Extensions;
using HelpMyStreet.Cache;
using HelpMyStreet.Contracts.GroupService.Response;

[assembly: FunctionsStartup(typeof(RequestService.AzureFunction.Startup))]
namespace RequestService.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
         ExecutionContextOptions executioncontextoptions = builder.Services.BuildServiceProvider()
           .GetService<IOptions<ExecutionContextOptions>>().Value;
            string currentDirectory = executioncontextoptions.AppDirectory;

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            IConfigurationRoot config = configBuilder.Build();

            Dictionary<HttpClientConfigName, ApiConfig> httpClientConfigs = config.GetSection("Apis").Get<Dictionary<HttpClientConfigName, ApiConfig>>();

            foreach (KeyValuePair<HttpClientConfigName, ApiConfig> httpClientConfig in httpClientConfigs)
            {

                builder.Services.AddHttpClient(httpClientConfig.Key.ToString(), c =>
                {
                    c.BaseAddress = new Uri(httpClientConfig.Value.BaseAddress);

                    c.Timeout = httpClientConfig.Value.Timeout ?? new TimeSpan(0, 0, 0, 15);

                    foreach (KeyValuePair<string, string> header in httpClientConfig.Value.Headers)
                    {
                        c.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                    c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    MaxConnectionsPerServer = httpClientConfig.Value.MaxConnectionsPerServer ?? 15,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });

            }

            IConfigurationSection applicationConfigSettings = config.GetSection("ApplicationConfig");
            builder.Services.Configure<ApplicationConfig>(applicationConfigSettings);

            IConfigurationSection connectionStringSettings = config.GetSection("ConnectionStrings");
            builder.Services.Configure<ConnectionStrings>(connectionStringSettings);

            ConnectionStrings connectionStrings = new ConnectionStrings();
            connectionStringSettings.Bind(connectionStrings);


            builder.Services.AddMediatR(typeof(PostRequestForHelpHandler).Assembly);
            builder.Services.AddAutoMapper(typeof(AddressDetailsProfile).Assembly);
            builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
            builder.Services.AddTransient<IUserService, Core.Services.UserService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<ICommunicationService, CommunicationService>();
            builder.Services.AddTransient<IGroupService, GroupService>();

            builder.Services.AddTransient<IRepository, Repository>();
            builder.Services.AddTransient<IDistanceCalculator, DistanceCalculator>();            
            builder.Services.AddTransient<IJobService, JobService>();
            builder.Services.AddTransient<IJobFilteringService, JobFilteringService>();
            builder.Services.AddTransient<IArchiveService, ArchiveService>();
            builder.Services.AddTransient<IManageRequestsService, ManageRequestsService>();
            builder.Services.AddTransient<IMultiJobs, MultiJobs>();

            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>)));

            builder.Services.AddMemCache();
            builder.Services.AddSingleton(x => x.GetService<IMemDistCacheFactory<double?>>().GetCache(new TimeSpan(30, 0, 0, 0), ResetTimeFactory.OnMidday));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    ConfigureDbContextOptionsBuilder(options, connectionStrings.RequestService),
                ServiceLifetime.Transient
            );            

            // automatically apply EF migrations
            // DbContext is being created manually instead of through DI as it throws an exception and I've not managed to find a way to solve it yet: 
            // 'Unable to resolve service for type 'Microsoft.Azure.WebJobs.Script.IFileLoggingStatusManager' while attempting to activate 'Microsoft.Azure.WebJobs.Script.Diagnostics.HostFileLoggerProvider'.'
            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ConfigureDbContextOptionsBuilder(dbContextOptionsBuilder, connectionStrings.RequestService);
            ApplicationDbContext dbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

            dbContext.Database.Migrate();
        }

        private void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder options, string connectionString)
        {
            options
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }
    }
}