﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Quartz;
using Quartz.Impl;
using ViolastroBot.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ViolastroBot;

public sealed class Startup
{
    private DiscordSocketClient _client;
    
    public async Task Initialize()
    {
        IServiceProvider services = ConfigureServices();
        _client = services.GetRequiredService<DiscordSocketClient>();
        _client.Log += Log;
        
        services.GetRequiredService<CommandService>().Log += Log;
        
        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));
        await _client.StartAsync();
        
        await services.GetRequiredService<CommandHandlerService>().InitializeAsync();
        
        await Task.Delay(-1);
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    
    private static IServiceProvider ConfigureServices()
    {    
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        
        IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
        
        DiscordSocketConfig config = new()
        {
            LogLevel = LogSeverity.Info,
            GatewayIntents = GatewayIntents.Guilds
                             | GatewayIntents.GuildMessages
                             | GatewayIntents.GuildMessageReactions
                             | GatewayIntents.GuildMembers
                             | GatewayIntents.GuildBans
                             | GatewayIntents.GuildEmojis
                             | GatewayIntents.DirectMessages
                             | GatewayIntents.DirectMessageReactions
                             | GatewayIntents.MessageContent
                 
        };

        IServiceCollection services = new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton(scheduler)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlerService>();
            
        return services.BuildServiceProvider();
    }
}