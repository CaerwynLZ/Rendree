using Sharky;
using Sharky.DefaultBot;
using System;
using SC2APIProtocol;

namespace Rendree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Rendree");

            var gameConnection = new GameConnection();
            var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

            var terranBuildChoices = new RendreeBuildChoices(defaultSharkyBot);
            defaultSharkyBot.BuildChoices[Race.Terran] = terranBuildChoices.BuildChoices;

            var Rendree = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

            var myRace = Race.Terran;

            if (args.Length == 0)
            {
                gameConnection.RunSinglePlayer(Rendree, @"GlitteringAshesAIE.SC2Map", myRace, Race.Protoss, Difficulty.VeryHard, AIBuild.Macro).Wait();
            }
            else
            {
                gameConnection.RunLadder(Rendree, myRace, args).Wait();
            }
        }
    }
}
