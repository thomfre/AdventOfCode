using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day17Solver : SolverBase
    {
        private readonly HashSet<(int X, int Y)> _clay = new HashSet<(int X, int Y)>();
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

                    int minY = _clay.Select(w => w.Y).Min()-1;
                    int maxY = _clay.Select(w => w.Y).Max()+1;
                    int minX = _clay.Select(w => w.X).Min()-1;
                    int maxX = _clay.Select(w => w.X).Max()+1;

                    for (int y = minY; y < maxY; y++)
                    {
                        Console.WriteLine();
                        for (int x = minX; x < maxX; x++)
                        {
                            if (_water.Contains((x, y)))
                            {
                                Console.Write("x");
                            }
                            else if (_clay.Contains((x, y)))
                            {
                                Console.Write("#");
                            }
                            else
                            {
                                Console.Write(".");
                            }
                        }
                    }


                    AnswerSolution1 = _water.Count(w => w.Y >= _minY && w.Y <= _maxY);
                    StopExecutionTimer();

                    return FormatSolution($"The score at the end is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    AnswerSolution2 = null;

                    StopExecutionTimer();

                    return FormatSolution($"The score at the end is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void FlowDown(int x, int y)
        {
            int i = 1;
            bool clayBelow = _clay.Contains((x, y + i));
            bool waterBelow = _water.Contains((x, y + i));
            while (!clayBelow && !waterBelow)
            {
                _water.Add((x, y + i));
                i++;
                clayBelow = _clay.Contains((x, y + i));
                waterBelow = _water.Contains((x, y + i));

                if (y + i > _maxY)
                {
                    return;
                }
            }

            FillAndFlowUp(x, y + i - 1);
        }

        private void FillAndFlowUp(int x, int y)
        {
            if (y > _maxY)
            {
                return;
            }

            int xMax = x;
            int xMin = x;
            int i = 0;
            while (!_clay.Contains((x + i++, y)))
            {
                xMax = x + i;
            }

            i = 0;
            while (!_clay.Contains((x - i++, y)))
            {
                xMin = x - i;
            }

            int yMovement = 0;
            bool flowing = true;            
            while (flowing)
            {
                i = 0;
                bool flowingOverEdge = false;
                int currentY = y - yMovement;
                while (!_clay.Contains((x + ++i, currentY)))
                {
                    int currentX = x + i;
                    if (_clay.Contains((currentX, currentY + 1))) flowingOverEdge = true;
                    if (flowingOverEdge && !_clay.Contains((currentX, currentY + 1)))
                    {
                        flowing = false;
                        FlowDown(currentX - 1, currentY);
                        break;
                    }
                    if (currentX > xMax + 1)
                    {
                        flowing = false;
                        FlowDown(currentX - 1, currentY);
                        break;
                    }

                    AddWater(currentX, currentY);
                }

                i = 0;
                flowingOverEdge = false;
                while (!_clay.Contains((x - ++i, currentY)))
                {
                    int currentX = x - i;
                    if (_clay.Contains((currentX, currentY + 1))) flowingOverEdge = true;
                    if (flowingOverEdge && !_clay.Contains((currentX, currentY + 1)))
                    {
                        flowing = false;
                        FlowDown(currentX + 1, currentY);
                        break;
                    }
                    if (currentX < xMin - 1)
                    {
                        flowing = false;
                        FlowDown(currentX + 1, currentY);
                        break;
                    }

                    AddWater(currentX, currentY);
                }

                yMovement++;
            }
        }

        private void AddWater(int x, int y)
        {
            if (_water.Contains((x, y)))
            {
                return;
            }

            _water.Add((x, y));
        }
    }
}
