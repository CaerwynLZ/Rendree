using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.DefaultBot;
using Sharky.MicroControllers;
using Rendree.Builds;
using System.Collections.Generic;

namespace Rendree
{
    public class RendreeBuildChoices
    {
        public Sharky.Builds.BuildChoices BuildChoices { get; private set; }

        public RendreeBuildChoices(DefaultSharkyBot defaultSharkyBot)
        {
            var marauderHellbat = new MarauderHellbat(defaultSharkyBot);
            var threeRax = new ThreeRax(defaultSharkyBot);
            var scvMicroController = new IndividualMicroController(defaultSharkyBot, defaultSharkyBot.SharkyAdvancedPathFinder, MicroPriority.JustLive, false);
            
            var terranBuilds = new Dictionary<string, ISharkyBuild>()
            {
                [marauderHellbat.Name()] = marauderHellbat,
                [threeRax.Name()] = threeRax
            };

            var versusEverything = new List<List<string>>
            {

            };

            var versusTerran = new List<List<string>>
            {
            };

            var versusProtoss = new List<List<string>>
            {
                new List<string> { threeRax.Name() },
            };

            var versusZerg = new List<List<string>>
            {
                new List<string> { marauderHellbat.Name() },
            };

            var transitions = new List<List<string>>
            {
            };

            var buildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = versusZerg,
                [Race.Zerg.ToString()] = versusZerg,
                [Race.Protoss.ToString()] = versusProtoss,
                [Race.Random.ToString()] = versusZerg,
                ["Transition"] = transitions,
            };

            BuildChoices = new Sharky.Builds.BuildChoices { Builds = terranBuilds, BuildSequences = buildSequences };
        }
    }
}
