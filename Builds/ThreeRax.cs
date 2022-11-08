using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.Builds.Terran;
using Sharky.Chat;
using Sharky.DefaultBot;
using Sharky.MicroTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendree.Builds
{
    class ThreeRax : TerranSharkyBuild
    {
        public ThreeRax(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
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

            MacroData.AddOnSwaps[this.Name() + "reactor"] = new AddOnSwap(UnitTypes.TERRAN_FACTORYREACTOR, UnitTypes.TERRAN_FACTORY, UnitTypes.TERRAN_STARPORT, true);

            AttackData.CustomAttackFunction = true;
            AttackData.UseAttackDataManager = false;
        }

        public override void OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;
            SetAttack();
            ThreeRaxOpener(frame);
            ThreeRaxMacro();
            AddProduction();
        }

        // This portion handles the beginning of the Reaper FE build, all the way until starting barracks upgrades.
        private void ThreeRaxOpener(int frame)
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
                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKSREACTOR) < 1)
                {
                    MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] = 1;
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
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_ORBITALCOMMAND) >= 1)
                {
                    BuildOptions.StrictWorkerCount = false;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_BARRACKS) < 3)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 3;
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKSREACTOR) != 0)
            {
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MARINE) < 50)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = 50;
                }
            }


            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_COMMANDCENTER) >= 2)
            {
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_ORBITALCOMMAND) < 2)
                {
                    MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 2;
                }
            }

            if (UnitCountService.Completed(UnitTypes.TERRAN_BARRACKSTECHLAB) < 2 && UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) > 1)
            {
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 2;
            }

            if (UnitCountService.Completed(UnitTypes.TERRAN_BARRACKSTECHLAB) >= 1)
            {
                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MARAUDER) < 4)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARAUDER] = 4;
                }
                MacroData.DesiredUpgrades[Upgrades.STIMPACK] = true;
                MacroData.DesiredUpgrades[Upgrades.SHIELDWALL] = true;
            }

            if (MacroData.FoodUsed >= 34)
            {
                if (MacroData.DesiredSupplyDepots < 3)
                {
                    MacroData.DesiredSupplyDepots = 3;
                }
            }

            if (MacroData.FoodUsed >= 46)
            {
                if (MacroData.DesiredSupplyDepots < 4)
                {
                    MacroData.DesiredSupplyDepots = 4;
                }
            }

            //var defenseSquadTask = (DefenseSquadTask)MicroTaskData.MicroTasks["DefenseSquadTask"];
            //defenseSquadTask.DesiredUnitsClaims = new List<DesiredUnitsClaim> { new DesiredUnitsClaim(UnitTypes.TERRAN_MARINE, 20), new DesiredUnitsClaim(UnitTypes.TERRAN_MARAUDER, 20) };
            //defenseSquadTask.Enable();

            //if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MARAUDER) >= 6 || UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MARINE) >= 18)
            //{
            //    defenseSquadTask.Disable();
            //}
        }

        // This portion of the code handles the macro follow up to 3 rax
        private void ThreeRaxMacro()
        {
            if (MacroData.Minerals - MacroData.VespeneGas > 300 && MacroData.FoodUsed >= 46 || MacroData.FoodUsed >= 58)
            {
                BuildOptions.StrictSupplyCount = false;

                if (MacroData.DesiredGases < 3)
                {
                    MacroData.DesiredGases = 3;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MARAUDER) < 35 && MacroData.DesiredGases >= 3)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARAUDER] = 35;
                }

                MacroData.DesiredTechCounts[UnitTypes.TERRAN_ENGINEERINGBAY] = 1;

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORY) < 1)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.TERRAN_FACTORY] = 1;
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_ENGINEERINGBAY) > 0 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) > 0)
                {
                    MacroData.DesiredUpgrades[Upgrades.TERRANINFANTRYWEAPONSLEVEL1] = true;
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_FACTORY) > 0)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_STARPORT) < 1 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_COMMANDCENTER) > 2)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_STARPORT] = 1;
                    }

                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_FACTORYREACTOR) < 1)
                    {
                        MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_FACTORYREACTOR] = 1;
                    }
                }

                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_STARPORTREACTOR) > 0)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_MEDIVAC) < 6)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MEDIVAC] = 6;
                    }
                }

                if (UnitCountService.Completed(UnitTypes.TERRAN_FACTORYREACTOR) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_REACTOR) > 0 || UnitCountService.Completed(UnitTypes.TERRAN_STARPORTREACTOR) > 0)
                {
                    MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_FACTORYREACTOR] = 0;
                }

                if (MacroData.Minerals > 400 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) > 0)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] < 3)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 3;
                    }

                    if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] < 4)
                    {
                        if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_ORBITALCOMMAND) < 3)
                        {
                            MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 3;
                        }
                    }

                    if (MacroData.DesiredGases < 4 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_COMMANDCENTER) > 2)
                    {
                        MacroData.DesiredGases = 4;
                    }
                }

                if (MacroData.Minerals > 400 && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_COMMANDCENTER) > 3)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_BARRACKS) < 5)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 5;
                    }
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_BARRACKS) > 3)
                {
                    if (UnitCountService.Completed(UnitTypes.TERRAN_BARRACKSTECHLAB) < 4)
                    {
                        MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 4;
                    }
                }

                if (UnitCountService.EnemyCount(UnitTypes.PROTOSS_COLOSSUS) > 0)
                {
                    if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_VIKINGFIGHTER) < 3 * UnitCountService.EnemyCount(UnitTypes.PROTOSS_COLOSSUS))
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_VIKINGFIGHTER] = 3 * UnitCountService.EnemyCount(UnitTypes.PROTOSS_COLOSSUS);
                    }
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_SCV) > 53)
                {
                    if (MacroData.DesiredGases < 6)
                    {
                        MacroData.DesiredGases = 6;
                    }

                    if (UnitCountService.EquivalentEnemyTypeCount(UnitTypes.TERRAN_GHOSTACADEMY) < 1)
                    {
                        MacroData.DesiredTechCounts[UnitTypes.TERRAN_GHOSTACADEMY] = 1;
                    }

                    if (UnitCountService.EquivalentEnemyTypeCount(UnitTypes.TERRAN_GHOSTACADEMY) > 0)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_GHOST] = 6;
                        MacroData.DesiredUpgrades[Upgrades.ENHANCEDSHOCKWAVES] = true;
                    }
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_SCV) > 55 && MacroData.Minerals > 450)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] < 4)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 4;
                    }
                }

                if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_COMMANDCENTER) > 3)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_PLANETARYFORTRESS] < 1)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_PLANETARYFORTRESS] = 1;
                    }
                }
            }
        }

        void AddProduction()
        {
            if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_COMMANDCENTER) > 2 && UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_SCV) > 55)
            {
                if (MacroData.Minerals > 500 && UnitCountService.Count(UnitTypes.TERRAN_BARRACKS) < 6)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] <= UnitCountService.Count(UnitTypes.TERRAN_BARRACKS))
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS]++;
                    }
                }
                if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) > 5)
                {
                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) % 2 == 0)
                    {
                        if ((MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] < UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKSTECHLAB) - 2))
                        {
                            MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] += 1;
                        }
                    }

                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) % 2 != 0)
                    {
                        if ((MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] < UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKSREACTOR) - 2))
                        {
                            MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] += 1;
                        }
                    }
                }
            }
        }

        void SetAttack()
        {
            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) >= 5 && UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) <= 8  || UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARINE) >= 18 && UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARINE) <= 23)
            {
                AttackData.Attacking = true;
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARAUDER) > 9 || UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_MARINE) > 24)
            {
                AttackData.Attacking = false;
            }

            if (MacroData.FoodUsed >= 190)
            {
                AttackData.Attacking = true;
            }
        }
    }
}
