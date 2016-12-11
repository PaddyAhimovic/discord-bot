using Discord;
using Discord.Commands;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiscordBot
{
    //TODO: classes for each person (doubtful), nickname changes for fun, utility (deleting), russian roullette
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;
        //this should initialize the meme database and RNG
        List<string> memes = File.ReadAllLines("C:\\Users\\Paddy\\Documents\\Projects\\DiscordBot\\Memes.txt").ToList();
        List<string> animemes = File.ReadAllLines("C:\\Users\\Paddy\\Documents\\Projects\\DiscordBot\\Animes.txt").ToList();
        Random rng;

        public MyBot()
        {
            rng = new Random();

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            //the bot either excepts mentions or exclamation marks as a prefix to it's commands
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();
            
            //yeah yeah I know that this could all be slimmed down with classes but fuck that
            //either way these are the descriptions of everyone with images
            commands.CreateCommand("Christian")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Gay weeb who unironically likes visual novels");
                });

            commands.CreateCommand("Jacob")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("The only person who's watched more one piece than Kevin Gallagher");
                    await e.Channel.SendMessage("http://i.imgur.com/C2HNQ2s.png");
                });

            commands.CreateCommand("Jeffrey")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("How the fuck did we let a jew be admin?");
                    await e.Channel.SendMessage("https://cdn.discordapp.com/attachments/158721080813420544/256994569122480128/20150825_131902.jpg");
                });

            commands.CreateCommand("Paddy")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Did you mean to say \"!meg \"?");
                    await e.Channel.SendMessage("http://i.imgur.com/9sHddY7.png");
                });

            commands.CreateCommand("Ryan")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Washed up CS player who has a \"normal\" shoe size");
                    await e.Channel.SendMessage("http://i.imgur.com/Okofs8D.png");
                });

            commands.CreateCommand("Ennis")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Put your cock away!");
                    await e.Channel.SendMessage("http://i.imgur.com/wfRbBAN.png");
                });

            commands.CreateCommand("Daniel")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Slashing asses since 1998");
                });

            commands.CreateCommand("I gotta move here")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("It'll bring down the house");
                    await e.Channel.SendMessage("http://i.imgur.com/p4KhA0y.png");
                });

            commands.CreateCommand("Liam")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Melee is a children's party game and fails as a competitive one");
                    await e.Channel.SendMessage("http://i.imgur.com/FAa3zon.png");
                });

            //now everything is pretty much done through function calls because that's nicer
            AddMeme();
            AddAnime();
            OutputMeme();
            OutputAnime();
            //connects bot to all the servers it's in
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjU2OTU5NDkyNzE2MTAxNjMy.CyzwFQ.qIGLgrLGIAW7YpJL58vrn_qtmx4", TokenType.Bot);
            });
        }

        //takes most recent message before command and stores it as a new anime
        private void AddAnime()
        {
            commands.CreateCommand("add anime")
                .Do(async (e) =>
                {
                    String newAnime;
                    Message[] x;

                    x = await e.Channel.DownloadMessages(2);
                    newAnime = x[1].RawText;
                    if (!newAnime.Contains("csgoani.me"))
                    {
                        await e.Channel.SendMessage("Why would you waste a good meme like that?");
                        await e.Channel.SendMessage("I'm not putting any memes in the cancer database");
                    }
                    else if (animemes.Contains(newAnime))
                    {
                        await e.Channel.SendMessage("Anime was a mistake and your reposting only makes it worse");
                    }
                    else
                    {
                        animemes.Add(newAnime);
                        File.WriteAllLines("C:\\Users\\Paddy\\Documents\\Projects\\DiscordBot\\Animes.txt", animemes);
                        await e.Channel.SendMessage("You're cancer for doing this to me...");
                        await e.Channel.SendMessage("Anime added (go fuck yourself)");
                    }
                });
        }

        //takes most recent message before command and stores it as a meme (it should always be a url but I guess copy pastas can work)
        private void AddMeme()
        {
            commands.CreateCommand("add meme")
                .Do(async (e) =>
                {
                    String newMeme;
                    Message[] x;

                    x = await e.Channel.DownloadMessages(2);
                    newMeme = x[1].RawText;
                    if (newMeme.Contains("csgoani.me"))
                    {
                        await e.Channel.SendMessage("Our memes are too pure for that cancer");
                    }
                    else if (memes.Contains(newMeme))
                    {
                        await e.Channel.SendMessage("That's an old meme you dip!");
                    }
                    else
                    {
                        memes.Add(newMeme);
                        File.WriteAllLines("C:\\Users\\Paddy\\Documents\\Projects\\DiscordBot\\Memes.txt", memes);
                        await e.Channel.SendMessage("Meme added");
                    }
                });
        }

        //outputs cancer
        private void OutputAnime()
        {
            commands.CreateCommand("anime")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(animemes[rng.Next(animemes.Count)]);
                });
        }

        //outputs a random meme
        private void OutputMeme()
        {
            commands.CreateCommand("meme")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(memes[rng.Next(memes.Count)]);
                });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
