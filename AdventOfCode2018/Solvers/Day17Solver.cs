using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using OutputColorizer;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day17Solver : SolverBase
    {
        private readonly HashSet<(int X, int Y)> _clay = new HashSet<(int X, int Y)>();
        private readonly HashSet<(int X, int Y)> _restedWater = new HashSet<(int X, int Y)>();
        private readonly HashSet<(int X, int Y)> _water = new HashSet<(int X, int Y)>();
        private int _maxY;
        private int _minY;

        public Day17Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 17;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] scanResult = GetInput().Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (string scan in scanResult)
            {
                int[] x = null;
                int[] y = null;
                string[] scanParts = scan.Split(',');
                foreach (string scanPart in scanParts)
                {
                    string[] partSplit = scanPart.Split('=');
                    int[] i;
                    if (partSplit[1].Contains(".."))
                    {
                        int[] valueParts = partSplit[1].Split("..").Select(int.Parse).ToArray();
                        i = Enumerable.Range(valueParts[0], valueParts[1] - valueParts[0] + 1).ToArray();
                    }
                    else
                    {
                        i = new[] {int.Parse(partSplit[1])};
                    }

                    if (partSplit[0].Trim() == "x")
                    {
                        x = i;
                    }
                    else
                    {
                        y = i;
                    }
                }

                if (x == null || y == null)
                {
                    continue;
                }

                foreach (int xx in x)
                foreach (int yy in y)
                {
                    _clay.Add((xx, yy));
                }
            }

            _maxY = _clay.OrderByDescending(c => c.Y).Select(c => c.Y).First();
            _minY = _clay.OrderBy(c => c.Y).Select(c => c.Y).First();

            switch (part)
            {
                case ProblemPart.Part1:

                    FlowDown(500, 0);

                    int minY = _clay.Select(w => w.Y).Min() - 1;
                    int maxY = _clay.Select(w => w.Y).Max() + 1;
                    int minX = _clay.Select(w => w.X).Min() - 1;
                    int maxX = _clay.Select(w => w.X).Max() + 1;

                    for (int y = minY; y < maxY; y++)
                    {
                        Console.WriteLine();
                        for (int x = minX; x < maxX; x++)
                        {
                            if (_restedWater.Contains((x, y)))
                            {
                                Colorizer.Write("[DarkBlue!w]");
                            }
                            else if (_water.Contains((x, y)))
                            {
                                Colorizer.Write("[Blue!o]");
                            }
                            else if (_clay.Contains((x, y)))
                            {
                                Colorizer.Write("[Red!#]");
                            }
                            else
                            {
                                Console.Write(".");
                            }
                        }
                    }

                    AnswerSolution1 = _water.Count(w => w.Y >= _minY && w.Y <= _maxY);
                    StopExecutionTimer();

                    return FormatSolution($"The water can reach a total of [{ConsoleColor.Green}!{AnswerSolution1}] tiles");
                case ProblemPart.Part2:
                    AnswerSolution2 = _restedWater.Count();

                    StopExecutionTimer();

                    return FormatSolution($"After the stream has gone dry, a total of [{ConsoleColor.Green}!{AnswerSolution2}] water tiles are remaining");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void FlowDown(int x, int y)
        {
            int i = 1;
            bool clayBelow = _clay.Contains((x, y + i));
            while (!clayBelow)
            {
                _water.Add((x, y + i));

                i++;
                clayBelow = _clay.Contains((x, y + i));

                if (y + i > _maxY)
                {
                    return;
                }
            }

            FillContainer(x, y + i - 1);
        }

        private void FillContainer(int x, int y)
        {
            if (y > _maxY)
            {
                return;
            }

            int yMovement = 0;
            bool flowing = true;

            while (flowing)
            {
                int i = 0;
                int currentY = y - yMovement;
                while (!_clay.Contains((x + ++i, currentY)))
                {
                    int currentX = x + i;

                    if (!_clay.Contains((currentX, currentY + 1)) && !_water.Contains((currentX, currentY + 1)))
                    {
                        flowing = false;
                        if (_clay.Contains((currentX - 1, currentY + 1)))
                        {
                            AddWater(currentX, currentY);
                            MakeWaterBelowRested(currentX - 2, currentY + 1);
                            FlowDown(currentX, currentY);
                        }

                        break;
                    }

                    AddWater(currentX, currentY);
                }

                i = 0;
                while (!_clay.Contains((x - ++i, currentY)))
                {
                    int currentX = x - i;
                    if (!_clay.Contains((currentX, currentY + 1)) && !_water.Contains((currentX, currentY + 1)))
                    {
                        flowing = false;
                        if (_clay.Contains((currentX + 1, currentY + 1)))
                        {
                            AddWater(currentX, currentY);
                            MakeWaterBelowRested(currentX + 2, currentY + 1);
                            FlowDown(currentX, currentY);
                        }

                        break;
                    }

                    AddWater(currentX, currentY);
                }

                yMovement++;
            }
        }

        private void MakeWaterBelowRested(int x, int y)
        {
            int xMax = x;
            int xMin = x;
            int yMin = y;
            int yMax = y;
            int i = 0;

            i = 0;
            while (!_clay.Contains((x, y + i++)) || (_clay.Contains((x, y + i)) && _water.Contains((x, y + i))))
            {
                yMax = y + i;
            }

            while (_clay.Contains((x + ++i, yMax)))
            {
                xMax = x + i;
            }

            i = 0;
            while (_clay.Contains((x - ++i, yMax)))
            {
                xMin = x - i;
            }

            

            _water.Where(w => w.Y >= yMin && w.Y < yMax && w.X > xMin && w.X <= xMax).ForEach(w =>
                                                                                          {
                                                                                              if (!_restedWater.Contains((w.X, w.Y)))
                                                                                              {
                                                                                                  _restedWater.Add(w);
                                                                                              }
                                                                                          });
        }

        private void AddWater(int x, int y)
        {
            if (_water.Contains((x, y)))
            {
                return;
            }

            _water.Add((x, y));


            //Sorry, I got tired of fighting with this day
            if (!_water.Contains((x - 1, y)) && _water.Contains((x - 2, y)) && !_clay.Contains((x - 1, y)))
            {
                _water.Add((x - 1, y));
            }

            if (!_water.Contains((x + 1, y)) && _water.Contains((x + 2, y)) && !_clay.Contains((x + 1, y)))
            {
                _water.Add((x + 1, y));
            }
        }
    }
}
