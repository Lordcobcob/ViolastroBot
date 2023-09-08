﻿using Microsoft.Extensions.DependencyInjection;
using ViolastroBot.Extensions;
using ViolastroBot.RandomWords;
using ViolastroBot.Services.Logging;

namespace ViolastroBot.Commands.RouletteActions;

/// <summary>
/// Sets the user's nickname to a random name.
/// </summary>
public sealed class SetUsernameToRandomWords : RouletteAction
{
    private readonly ILoggingService _logger;
    
    public SetUsernameToRandomWords(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILoggingService>();
    }
    
    protected override async Task ExecuteAsync()
    {
        List<string> words = new WordRandomizer().GetRandomWords(1, 3);
        string name = string.Join(" ", words).CapitalizeFirstCharacter();
    
        try
        {
            await Context.Guild.GetUser(Context.User.Id).ModifyAsync(properties => properties.Nickname = name);
            await ReplyAsync($"Bwehehe!! Ya name is now \"{name}\"!!");
        }
        catch (Discord.Net.HttpException ex)
        {
            await _logger.LogMessageAsync($"Failed to change {Context.User.Mention}'s name to \"{name}\".{Environment.NewLine}Exception: {ex.Message}");
        }
        catch (Exception ex) 
        {
            await _logger.LogMessageAsync($"An unexpected error occurred: {ex.Message}");
        }
    }
}