using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.Builds.Terran;
using Sharky.Chat;
using Sharky.DefaultBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendree.Builds
{
    public class MarauderHellbat : TerranSharkyBuild
    {
        public MarauderHellbat(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
        {
            
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);
            BuildOptions.WallOffType = Sharky.Builds.BuildingPlacement.WallOffType.Full;

            BuildOptions.StrictGasCount = true;
            MacroData.DesiredGases = 0;

            BuildOptions.StrictSupplyCount = true;
            MacroData.DesiredSupplyDepots = 0;

            BuildOptions.StrictWorkerCount = true;
            MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 19;

            MacroData.AddOnSwaps[this.Name() + "reactor"] = new AddOnSwap(UnitTypes.TERRAN_BARRACKSREACTOR, UnitTypes.TERRAN_BARRACKS, UnitTypes.TERRAN_FACTORY, true);
        }

        public override void OnFrame(ResponseObservation observation)
        {
            Random rand = new Random();
            int followUpChoice = rand.Next(0, 2);
            var frame = (int)observation.Observation.GameLoop;

            TvZReaperFE(frame);
            MarauderHellbatAllin();
            MechFollowUp();
            //switch (followUpChoice)
            //{
            //    case 0:
            //        MechFollowUp();
            //        break;

            //    case 1:
            //        BioFollowUp();
            //        break;

            //    default:
            //        MechFollowUp();
            //        break;
            //}
        }

        // This portion handles the beginning of the Reaper FE build, all the way until starting the starport.
        private void TvZReaperFE(int frame)
        {
            if (MacroData.FoodUsed >= 14)
            {
                if (MacroData.DesiredSupplyDepots < 1)
                {
                    MacroData.DesiredSupplyDepots = 1;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_SUPPLYDEPOT) > 0)
            {
                if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] < 1)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 1;
                }
                if (MacroData.DesiredGases < 1)
                {
                    MacroData.DesiredGases = 1;
                }
            }

            if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_BARRACKS) > 0)
            {
                if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] < 2)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 2;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) > 0)
            {
                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_REAPER) < 1)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_REAPER] = 1;
                }
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_ORBITALCOMMAND) < 1)
                {
                    MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 1;
                }
                if (UnitCountService.BuildingsInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0)
                {
                    if (MacroData.DesiredSupplyDepots < 2)
                    {
                        MacroData.DesiredSupplyDepots = 2;
                    }
                    if (MacroData.DesiredGases < 2 && UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_SUPPLYDEPOT) == 2)
                    {
                        MacroData.DesiredGases = 2;
                    }
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_ORBITALCOMMAND) >= 1)
                {
                    BuildOptions.StrictWorkerCount = false;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORY) < 1)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_FACTORY] = 1;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_REAPER) > 0)
            {
                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARINE) < 1)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = 1;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARINE) > 0)
            {
                if (MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] < 1)
                {
                    MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] = 1;
                    if (UnitCountService.Completed(UnitTypes.TERRAN_MARINE) > 0)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = 0;
                    }

                    if (UnitCountService.Completed(UnitTypes.TERRAN_REAPER) > 0)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_REAPER] = 0;
                    }
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_COMMANDCENTER) >= 2)
            {
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_ORBITALCOMMAND) < 2)
                {
                    MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 2;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_FACTORY) > 0)
            {
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_STARPORT) < 1)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_STARPORT] = 1;
                }
            }

            if (UnitCountService.Completed(UnitTypes.TERRAN_BARRACKSREACTOR) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_REACTOR) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_FACTORYREACTOR) > 0)
            {
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] = 0;
            }
        }

        // This portion of the code handles the allin part of the build
        private void MarauderHellbatAllin()
        {
            if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORYREACTOR) > 0)
            {
                if (MacroData.DesiredSupplyDepots < 3)
                {
                    MacroData.DesiredSupplyDepots = 3;
                }

                if (MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] < 1)
                {
                    MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 1;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORYREACTOR) > 0)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_HELLION) < 10)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_HELLION] = 10;
                    }

                    BuildOptions.StrictWorkerCount = true;
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 23;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_BARRACKSTECHLAB) > 0)
                {
                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) < 3)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARAUDER] = 3;
                    }

                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) >= 3)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARAUDER] = 0;
                    }
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_STARPORT) > 0)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_VIKINGFIGHTER) < 1)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_VIKINGFIGHTER] = 1;
                    }

                    MacroData.DesiredTechCounts[UnitTypes.TERRAN_ARMORY] = 1;
                }

                if (UnitCountService.UnitsInProgressCount(UnitTypes.TERRAN_VIKINGFIGHTER) > 0)
                {
                    if (MacroData.DesiredSupplyDepots < 4)
                    {
                        MacroData.DesiredSupplyDepots = 4;
                    }
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_VIKINGFIGHTER) > 0)
                {
                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MEDIVAC) == 0)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MEDIVAC] = 1;
                    }
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_HELLION) >= 2)
                {
                    BuildOptions.StrictWorkerCount = false;
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKSTECHLAB) > 0)
                {
                    MacroData.DesiredUpgrades[Upgrades.PUNISHERGRENADES] = true;
                }

                if (UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.TERRAN_HELLION) >= 6)
                {
                    BuildOptions.StrictSupplyCount = false;
                }
            }
        }

        // [MECH] This portion of the code will be the last part of the build, handling the macro after the attack.
        private void MechFollowUp()
        {
            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_HELLION) >= 6)
            {
                
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_COMMANDCENTER) < 3)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 3;
                }
                if (MacroData.DesiredGases < 4 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_COMMANDCENTER) > 3)
                {
                    MacroData.DesiredGases = 4;
                }

                if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FUSIONCORE) < 1 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_COMMANDCENTER) >= 3 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_REFINERY) >= 4)
                {
                    MacroData.DesiredTechCounts[UnitTypes.TERRAN_FUSIONCORE] = 1;
                }

                //MacroData.AddOnSwaps[this.Name() + "tech lab"] = new AddOnSwap(UnitTypes.TERRAN_BARRACKSTECHLAB, UnitTypes.TERRAN_BARRACKS, UnitTypes.TERRAN_STARPORT, true);

                //if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MEDIVAC) > 0 && UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) >= 3)
                //{
                //    MacroData.AddOnSwaps[this.Name() + "tech lab"] = new AddOnSwap(UnitTypes.TERRAN_BARRACKSTECHLAB, UnitTypes.TERRAN_BARRACKS, UnitTypes.TERRAN_STARPORT, true);
                //}

                if (UnitCountService.Completed(UnitTypes.TERRAN_BARRACKSTECHLAB) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_TECHLAB) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_STARPORTTECHLAB) > 0)
                {
                    MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 0;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_HELLION) < 15)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_HELLION] = 15;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_SIEGETANK) < 10)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_HELLION] = 10;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_THOR) < 5)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_THOR] = 5;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORY) < 5)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_FACTORY] = 5;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_COMMANDCENTER) >= 3)
                {
                    BuildOptions.StrictGasCount = false;
                }
            }
        }

        // [BIO] This portion of the code will be the last part of the build, handling the macro after the attack.
        private void BioFollowUp()
        {

        }
    }
}
