// If you need to see how to use, check Example.cs

using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace asuka_bot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // This is what we use to grab a PE EXE's file version.
        public string exeVersion = FileVersionInfo.GetVersionInfo(Application.StartupPath + "\\asuka-bot.exe").FileVersion;  // Replace this with whatever the real exe's name is!
                                                                                                                               // By default this can be left empty.
        public string discordNetDllVersion = FileVersionInfo.GetVersionInfo(Application.StartupPath + "\\Discord.Net.Core.dll").FileVersion;

        // Default colors for embeds that I want to use.
        public Color color = Color.Magenta;

        private string BotUptime()
        {
            // This string method calculates the amount of time the bot has been active and subtracts that from the current time and converts it to string so that the value is returnable.
            TimeSpan uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();
            return uptime.ToString(@"d\.hh\:mm\:ss");        // Removing 8 characters from the end of the string because we want the returned value to not go smaller than seconds.
        }

        [Command("uptime")]
        private async Task Uptime()
        {
            // Since the method we are calling returns a string we need convert the string into a message.
            await Context.Channel.SendMessageAsync($"{BotUptime()}");
        }

        [Command("ver")]
        private async Task Ver()
        {
            // This method displays the version of the bot

            // The following lines read a file called buildinfo.txt which is placed in the output directory at compile time which displays the date and time of compile.
            // TODO: Check if the compile time is created in the post-build event, in case the build event errors out.
            var buildInfo = Application.StartupPath + "\\buildinfo.txt";
            var reader = string.Join("", File.ReadAllLines(buildInfo, Encoding.UTF8));

            EmbedBuilder verEmbed = new EmbedBuilder();

            verEmbed.WithAuthor(Context.User);
            verEmbed.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());
            verEmbed.WithColor(Color.Magenta);
            verEmbed.WithTitle($"{Context.Client.CurrentUser.Username}'s Version");
            verEmbed.WithDescription("Version information of the bot.");
            verEmbed.AddField("Current Version Number:", $"{exeVersion}", false);
            verEmbed.AddField("Current Release Status:", "Pre-release", false);
            verEmbed.AddField("Discord.Net Library Version: ", $"{discordNetDllVersion}", false);  // This lets you know what build we are at.
            verEmbed.AddField("Current Build Stamp:", $"{reader}", false);
            //verEmbed.AddField("Build Target Folder:", "", false);         // Not yet ready for implementation!!
            verEmbed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, verEmbed.Build());
        }

        [Command("version")]
        private async Task Version()
        {
            // This is a demonstration that you can actually call other methods to make a command exactly the same as the other without writing the same code.
            await Ver();
        }

        [Command("uwu")]
        private async Task AwdBios()
        {
            // Just a fun command for now.
            await Context.Channel.SendMessageAsync("Did you just 'uwu' me bitch?");
        }

        [Command("shutdown")]
        private async Task Shutdown()
        {
            if (Context.Message.Author.Id == 309194468052172802)
            {
                await Context.Channel.SendMessageAsync("Gute Nacht!");
                Environment.Exit(0);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nuh-uh, you ain't allowed to force me to sleep!");
            }
        }

        [Command("help")]
        private async Task Help()
        {
            // This is the help section. You can create a help group if you want to create help sections for each command (see "example.cs" for proper setup).
            EmbedBuilder helpEmbed = new EmbedBuilder();
            helpEmbed.WithAuthor(Context.User);
            helpEmbed.WithColor(Color.Magenta);
            helpEmbed.WithTitle("Asuka");
            helpEmbed.AddField("`~uwu`", "we'll do something with this :wink:", true);
            helpEmbed.AddField("`~uptime`", "Sends the amount of time Cortana has been active.", true);
            helpEmbed.AddField("`~ver`", "Prints the version of the bot.", true);
            helpEmbed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, helpEmbed.Build());
        }
    }
}