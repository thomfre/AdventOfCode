using System;
using System.Collections.Generic;
using System.Linq;
using OutputColorizer;
using Thomfre.AdventOfCode2018.Tools;
using static MoreLinq.Extensions.ForEachExtension;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day15Solver : SolverBase
    {
        public Day15Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 15;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] battleGroundInput = GetInput().Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToArray();

            switch (part)
            {
                case ProblemPart.Part1:

                    AnswerSolution1 = PlayGame(battleGroundInput, 3, false);
                    StopExecutionTimer();

                    return FormatSolution($"The score at the end is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    bool solutionFound = false;
                    int elvesAttackPower = 4;
                    while (!solutionFound)
                    {
                        int result = PlayGame(battleGroundInput, elvesAttackPower++, true);
                        if (result == -1)
                        {
                            continue;
                        }

                        solutionFound = true;
                        AnswerSolution2 = result;
                    }

                    StopExecutionTimer();

                    return FormatSolution($"The score at the end, with all elves remaining, is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int PlayGame(IReadOnlyList<string> battleGroundInput, int elvesAttackPower, bool failOnDeadElves)
        {
            Dictionary<(int X, int Y), CaveElement> cave = new Dictionary<(int X, int Y), CaveElement>();
            HashSet<Unit> units = new HashSet<Unit>();

            for (int battleGroundRow = 0; battleGroundRow < battleGroundInput.Count; battleGroundRow++)
            for (int battleGroundColumn = 0; battleGroundColumn < battleGroundInput[battleGroundRow].Length; battleGroundColumn++)
            {
                char cavePoint = battleGroundInput[battleGroundRow][battleGroundColumn];
                (int X, int Y) coordinates = (battleGroundColumn, battleGroundRow);
                CaveElement caveElement = CaveElement.Ground;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (cavePoint)
                {
                    case 'E':
                        units.Add(new Unit(UnitType.Elf, coordinates, elvesAttackPower));
                        caveElement = CaveElement.Unit;
                        break;
                    case 'G':
                        units.Add(new Unit(UnitType.Goblin, coordinates));
                        caveElement = CaveElement.Unit;
                        break;
                    case '.':
                        caveElement = CaveElement.Ground;
                        break;
                    case '#':
                        caveElement = CaveElement.Wall;
                        break;
                }

                cave.Add(coordinates, caveElement);
            }

            int battleRoundCounter = 0;
            bool gameActive = true;
            while (gameActive)
            {
                PrintCave(false, cave, units, battleRoundCounter);

                IOrderedEnumerable<Unit> unitMoveOrder = units.Where(u => u.IsAlive).OrderBy(u => u.Location.Y).ThenBy(u => u.Location.X);
                foreach (Unit unit in unitMoveOrder)
                {
                    unit.PlayRound(cave, units);
                    if (unit.VictoryDeclared)
                    {
                        gameActive = false;
                    }
                }

                if (failOnDeadElves && units.Any(u => u.UnitType == UnitType.Elf && u.IsDead))
                {
                    return -1;
                }

                if (units.Select(u => u.UnitType).Distinct().Count() > 1
                 || !units.Any(u => u.VictoryDeclared))
                {
                    battleRoundCounter++;
                }
            }

            int remainingSurvivorHealth = units.Where(u => u.IsAlive).Sum(u => u.Health);
            return (battleRoundCounter - 1) * remainingSurvivorHealth;
        }

        private void PrintCave(bool printEnabled, Dictionary<(int X, int Y), CaveElement> cave, HashSet<Unit> units, int battleRoundCounter)
        {
            if (!printEnabled)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Battle round " + battleRoundCounter);
            for (int y = 0; y <= cave.Keys.Max(c => c.Y); y++)
            {
                if (y > 0)
                {
                    Console.WriteLine();
                }

                for (int x = 0; x <= cave.Keys.Max(c => c.X); x++)
                {
                    (int X, int Y) point = (x, y);
                    switch (cave[point])
                    {
                        case CaveElement.Wall:
                            Colorizer.Write("[DarkGray!#]");
                            break;
                        case CaveElement.Ground:
                            Colorizer.Write("[Gray!.]");
                            break;
                        case CaveElement.Unit:
                            Unit unit = units.First(u => u.Location == point);
                            Colorizer.Write(unit.UnitType == UnitType.Elf ? "[Green!E]" : "[Red!G]");
                            break;
                    }
                }
            }
        }

        internal class Unit
        {
            public Unit(UnitType type, (int X, int Y) startLocation, int attackPower = 3)
            {
                UnitType = type;
                Location = startLocation;
                Health = 200;
                AttackPower = attackPower;
            }

            public UnitType UnitType { get; }
            public (int X, int Y) Location { get; set; }
            public int Health { get; set; }
            public int AttackPower { get; set; }
            public bool IsDead => Health <= 0;
            public bool IsAlive => Health > 0;
            public bool VictoryDeclared { get; private set; }

            public void PlayRound(Dictionary<(int X, int Y), CaveElement> cave, HashSet<Unit> units)
            {
                if (IsDead)
                {
                    return;
                }

                Unit[] enemies = units.Where(u => u.UnitType != UnitType && u.IsAlive).ToArray();
                if (!enemies.Any())
                {
                    VictoryDeclared = true;
                    return;
                }

                HashSet<(int X, int Y)> inAttackRange = enemies.Where(e => IsInRange(e.Location)).Select(e => e.Location).ToHashSet();

                if (!inAttackRange.Any())
                {
                    MoveIfAble(cave, enemies);
                    inAttackRange = enemies.Where(e => IsInRange(e.Location)).Select(e => e.Location).ToHashSet();
                }

                if (inAttackRange.Any())
                {
                    AttackIfAble(cave, enemies, inAttackRange);
                }
            }

            private void MoveIfAble(Dictionary<(int X, int Y), CaveElement> cave, Unit[] enemies)
            {
                HashSet<(int X, int Y)> reachableEnemySurroundings = enemies
                                                                    .SelectMany(e => GetNeighbors(e.Location.X, e.Location.Y)
                                                                                    .Where(x => cave[x] == CaveElement.Ground).Select(x => x))
                                                                    .Select(x => x).ToHashSet();

                if (reachableEnemySurroundings.Count == 0)
                {
                    return;
                }

                TryToMove(cave, reachableEnemySurroundings);
            }

            private void AttackIfAble(Dictionary<(int X, int Y), CaveElement> cave, IEnumerable<Unit> enemies, HashSet<(int X, int Y)> inAttackRange)
            {
                Unit enemyToAttack = enemies.Where(u => inAttackRange.Contains(u.Location))
                                            .OrderBy(u => u.Health)
                                            .ThenBy(u => u.Location.Y)
                                            .ThenBy(u => u.Location.X)
                                            .First();

                enemyToAttack.Health -= AttackPower;
                if (enemyToAttack.Health <= 0)
                {
                    cave[enemyToAttack.Location] = CaveElement.Ground;
                }
            }

            private bool IsInRange((int X, int Y) location)
            {
                return Math.Abs(Location.X - location.X) + Math.Abs(Location.Y - location.Y) == 1;
            }

            private void TryToMove(Dictionary<(int X, int Y), CaveElement> cave, HashSet<(int X, int Y)> reachableEnemySurroundings)
            {
                Queue<(int X, int Y)> queue = new Queue<(int X, int Y)>();
                Dictionary<(int x, int y), (int px, int py)> previous = new Dictionary<(int X, int Y), (int pX, int pY)>();
                queue.Enqueue((Location.X, Location.Y));
                previous.Add((Location.X, Location.Y), (-1, -1));
                while (queue.Count > 0)
                {
                    (int x, int y) = queue.Dequeue();
                    GetNeighbors(x, y).Where(n => cave[n] == CaveElement.Ground)
                                      .Select(n => n)
                                      .ForEach(neighbor =>
                                               {
                                                   if (previous.ContainsKey(neighbor) ||
                                                       cave[(neighbor.X, neighbor.Y)] !=
                                                       CaveElement.Ground)
                                                   {
                                                       return;
                                                   }

                                                   queue.Enqueue(neighbor);
                                                   previous.Add(neighbor, (x, y));
                                               });
                }

                List<(int x, int y)> bestPath = reachableEnemySurroundings.Select(t => (t.X, t.Y, route: GetRoute(t.X, t.Y)))
                                                                          .Where(t => t.route != null)
                                                                          .OrderBy(t => t.route.Count)
                                                                          .ThenBy(t => t.Y)
                                                                          .ThenBy(t => t.X)
                                                                          .Select(t => t.route)
                                                                          .FirstOrDefault();

                if (bestPath == null)
                {
                    return;
                }

                cave[Location] = CaveElement.Ground;
                Location = (bestPath[0].x, bestPath[0].y);
                cave[Location] = CaveElement.Unit;

                List<(int x, int y)> GetRoute(int enemyX, int enemyY)
                {
                    if (!previous.ContainsKey((enemyX, enemyY)))
                    {
                        return null;
                    }

                    List<(int x, int y)> path = new List<(int x, int y)>();
                    (int x, int y) = (enemyX, enemyY);
                    while (x != Location.X || y != Location.Y)
                    {
                        path.Add((x, y));
                        (x, y) = previous[(x, y)];
                    }

                    path.Reverse();
                    return path;
                }
            }

            private List<(int X, int Y)> GetNeighbors(int x, int y)
            {
                return new List<(int X, int Y)>
                       {
                           (x, y - 1),
                           (x - 1, y),
                           (x + 1, y),                           
                           (x, y + 1),                           
                       };
            }
        }

        internal enum UnitType
        {
            Elf,
            Goblin
        }

        internal enum CaveElement
        {
            Wall,
            Ground,
            Unit
        }
    }
}
