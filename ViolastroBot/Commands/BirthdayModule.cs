﻿using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using ViolastroBot.DiscordServerConfiguration;

namespace ViolastroBot.Commands;

[Name("Minion Toaster")]
public sealed class BirthdayModule : ModuleBase<SocketCommandContext>
{
    [Command("bday")]
    [Discord.Commands.Summary("Assigns the birthday role to the the mentioned user.")]
    SocketUser user = userMessage.Author;
    // Assuming you have access to the user object and the desired role name
    string roleRestriction = "Moderator"; // Replace with the actual role name you're checking for

    // Check if the user has the desired role
    if (user.Roles.Any(role => role.Name == roleRestriction)
    {    
        public Task AssignBirthdayRole([Remainder] string _ = "")
           {
               if (Context.Message.MentionedUsers.Count == 0)
           {
               return Task.CompletedTask;
           }

           SocketGuildUser user = Context.Guild.GetUser(Context.Message.MentionedUsers.First().Id);
           SocketRole birthdayRole = Context.Guild.GetRole(Roles.Birthday);

           if (user.Roles.Any(role => role.Id == Roles.Birthday))
           {
               return user.RemoveRoleAsync(birthdayRole);
           }
        
           user.AddRoleAsync(birthdayRole);
        
           return ReplyAsync($"Happy dabby birthday, {user.Mention}! Bwehehe!!");
        }
    }
}

  
