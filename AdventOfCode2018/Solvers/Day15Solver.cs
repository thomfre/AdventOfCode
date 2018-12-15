using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using OutputColorizer;
using Thomfre.AdventOfCode2018.Tools;

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

            Dictionary<Point, CaveElement> cave = new Dictionary<Point, CaveElement>();
            HashSet<Unit> units = new HashSet<Unit>();

            for (int battleGroundRow = 0; battleGroundRow < battleGroundInput.Length; battleGroundRow++)
            for (int battleGroundColumn = 0; battleGroundColumn < battleGroundInput[battleGroundRow].Length; battleGroundColumn++)
            {
                char cavePoint = battleGroundInput[battleGroundRow][battleGroundColumn];
                Point coordinates = new Point(battleGroundColumn, battleGroundRow);
                CaveElement caveElement = CaveElement.Ground;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (cavePoint)
                {
                    case 'E':
                        units.Add(new Unit(UnitType.Elf, coordinates));
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

            switch (part)
            {
                case ProblemPart.Part1:
                    int battleRoundCounter = 0;
                    while (!units.Any(u => u.VictoryDeclared))
                    {
                        PrintCave(false, cave, units, battleRoundCounter);

                        IOrderedEnumerable<Unit> unitMoveOrder = units.OrderBy(u => u.Location.Y).ThenBy(u => u.Location.X);
                        foreach (Unit unit in unitMoveOrder)
                        {
                            unit.PlayRound(cave, units);
                        }

                        battleRoundCounter++;
                    }

                    int remainingSurvivorHealth = units.Where(u => u.IsAlive).Sum(u => u.Health);
                    AnswerSolution1 = (battleRoundCounter - 1) * remainingSurvivorHealth;
                    StopExecutionTimer();

                    return FormatSolution($"The answer is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:


                    AnswerSolution2 = null;

                    StopExecutionTimer();

                    return FormatSolution($"The answer is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void PrintCave(bool printEnabled, Dictionary<Point, CaveElement> cave, HashSet<Unit> units, int battleRoundCounter)
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
                    Point point = new Point(x, y);
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
            public Unit(UnitType type, Point startLocation)
            {
                UnitType = type;
                Location = startLocation;
                Health = 200;
                AttackPower = 3;
            }

            public UnitType UnitType { get; }
            public Point Location { get; set; }
            public int Health { get; set; }
            public int AttackPower { get; set; }
            public bool IsDead => Health <= 0;
            public bool IsAlive => Health > 0;
            public bool VictoryDeclared { get; private set; }

            public void PlayRound(Dictionary<Point, CaveElement> cave, HashSet<Unit> units)
            {
                if (IsDead)
                {
                    return;
                }

                MoveIfAble(cave, units);

                AttackIfAble(cave, units);
            }

            private void MoveIfAble(Dictionary<Point, CaveElement> cave, HashSet<Unit> units)
            {
                IEnumerable<Unit> enemies = units.Where(u => u.UnitType != UnitType && u.IsAlive).ToArray();
                HashSet<Point> inRange = new HashSet<Point>();
                HashSet<Point> inAttackRange = new HashSet<Point>();

                if (!enemies.Any())
                {
                    VictoryDeclared = true;
                    return;
                }

                enemies.ForEach(enemy =>
                                {
                                    int enemyX = enemy.Location.X, enemyY = enemy.Location.Y;
                                    Point[] enemySurroundings =
                                    {
                                        new Point(enemyX + 1, enemyY),
                                        new Point(enemyX - 1, enemyY),
                                        new Point(enemyX, enemyY + 1),
                                        new Point(enemyX, enemyY - 1)
                                    };
                                    enemySurroundings.ForEach(es =>
                                                              {
                                                                  if (cave[es] == CaveElement.Ground)
                                                                  {
                                                                      inRange.Add(es);
                                                                  }

                                                                  if (es == Location)
                                                                  {
                                                                      inAttackRange.Add(enemy.Location);
                                                                  }
                                                              });
                                });

                if (inRange.Count == 0 || inAttackRange.Count > 0)
                {
                    return;
                }

                Dictionary<Point, (Point moveTo, int hopsNeeded)> routes = new Dictionary<Point, (Point moveTo, int hopsNeeded)>();

                foreach (Point pointInRange in inRange)
                {
                    int step = 0;
                    Dictionary<Point, int> shits = new Dictionary<Point, int> {{Location, step}};

                    while (!shits.ContainsKey(pointInRange))
                    {
                        step++;
                        bool anyAbleToMove = false;
                        List<Point> keys = shits.Where(pair => pair.Value == step - 1).Select(pair => pair.Key).ToList();
                        foreach (Point currentPoint in keys)
                        {
                            Point[] neighbors =
                            {
                                new Point(currentPoint.X + 1, currentPoint.Y),
                                new Point(currentPoint.X - 1, currentPoint.Y),
                                new Point(currentPoint.X, currentPoint.Y + 1),
                                new Point(currentPoint.X, currentPoint.Y - 1)
                            };
                            neighbors.ForEach(p =>
                                              {
                                                  if (shits.ContainsKey(p))
                                                  {
                                                      return;
                                                  }

                                                  if (cave[p] == CaveElement.Ground)
                                                  {
                                                      shits.Add(p, step);
                                                      anyAbleToMove = true;
                                                  }

                                                  if (p != pointInRange)
                                                  {
                                                      return;
                                                  }

                                                  HashSet<Point> route = new HashSet<Point> {pointInRange};
                                                  for (int i = step - 1; i > 0; i--)
                                                  {
                                                      foreach (KeyValuePair<Point, int> point in shits.Where(point => point.Value == i))
                                                      {
                                                          if (ManhattanDistance(point.Key, route.Last()) == 1)
                                                          {
                                                              route.Add(point.Key);
                                                          }
                                                      }
                                                  }

                                                  routes.Add(pointInRange, (route.Reverse().First(), step));
                                              });
                        }

                        if (!anyAbleToMove)
                        {
                            break;
                        }
                    }
                }

                if (routes.Count == 0)
                {
                    return;
                }

                Point nextStep = routes
                                .OrderBy(r => r.Value.hopsNeeded)
                                .ThenBy(r => r.Key.Y)
                                .ThenBy(r => r.Key.Y)
                                .ThenBy(r => r.Value.moveTo.Y)
                                .ThenBy(r => r.Value.moveTo.X)
                                .Select(r => r.Value.moveTo).FirstOrDefault();

                cave[Location] = CaveElement.Ground;
                Location = nextStep;
                cave[Location] = CaveElement.Unit;
            }

            private void AttackIfAble(Dictionary<Point, CaveElement> cave, HashSet<Unit> units)
            {
                IEnumerable<Unit> enemies = units.Where(u => u.UnitType != UnitType && u.IsAlive);
                HashSet<Point> inAttackRange = new HashSet<Point>();

                enemies.ForEach(enemy =>
                                {
                                    int enemyX = enemy.Location.X, enemyY = enemy.Location.Y;
                                    Point[] enemySurroundings =
                                    {
                                        new Point(enemyX + 1, enemyY),
                                        new Point(enemyX - 1, enemyY),
                                        new Point(enemyX, enemyY + 1),
                                        new Point(enemyX, enemyY - 1)
                                    };
                                    enemySurroundings.ForEach(es =>
                                                              {
                                                                  if (es == Location)
                                                                  {
                                                                      inAttackRange.Add(enemy.Location);
                                                                  }
                                                              });
                                });

                if (inAttackRange.Count == 0)
                {
                    return;
                }

                Unit enemyToAttack = units.Where(u => inAttackRange.Contains(u.Location))
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

            private int ManhattanDistance(Point from, Point to)
            {
                return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
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
