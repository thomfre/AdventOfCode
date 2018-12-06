using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day6Solver : SolverBase
    {
        public Day6Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public int TotalDistanceMustBeUnder { get; set; } = 10000;

        public override int DayNumber => 6;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            Point[] points = input.Split('\n')
                                  .Select(i => i.Split(','))
                                  .Select(x => new Point(int.Parse(x.First().Trim()),
                                                         int.Parse(x.Last().Trim())))
                                  .ToArray();

            int maxX = points.Max(x => x.X);
            int maxY = points.Max(x => x.Y);
            int[,] grid = new int[maxX + 2, maxY + 2];

            switch (part)
            {
                case ProblemPart.Part1:
                    Dictionary<int, int> neighborCounters = Enumerable.Range(-1, points.Length + 1).ToDictionary(k => k, v => 0);

                    for (int x = 0; x <= maxX + 1; x++)
                    for (int y = 0; y <= maxY + 1; y++)
                    {
                        Point gridPoint = new Point(x, y);
                        (int index, int dist)[] distances = points
                                                           .Select((point, index) => (index, dist: ManhattanDistance(point, gridPoint)))
                                                           .OrderBy(c => c.dist)
                                                           .ToArray();

                        if (distances[1].dist == distances[0].dist) //Check if distance is tied with other
                        {
                            grid[x, y] = -1;
                        }
                        else
                        {
                            grid[x, y] = distances[0].index;
                            if (x > 0 && y > 0 && x < maxX + 1 && y < maxY + 1) //Stay off edges
                            {
                                neighborCounters[distances[0].index]++;
                            }
                            else
                            {
                                neighborCounters[distances[0].index] = 0;
                            }
                        }
                    }

                    AnswerSolution1 = neighborCounters
                                     .OrderByDescending(x => x.Value)
                                     .Select(x => x.Value)
                                     .First();

                    StopExecutionTimer();

                    return FormatSolution($"The size of the largest area that is finite is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    int regionSize = 0;

                    for (int x = 0; x <= maxX + 1; x++)
                    for (int y = 0; y <= maxY + 1; y++)
                    {
                        Point gridPoint = new Point(x, y);
                        int totalDistance = points.Sum(point => ManhattanDistance(point, gridPoint));

                        if (totalDistance < TotalDistanceMustBeUnder)
                        {
                            regionSize++;
                        }
                    }

                    AnswerSolution2 = regionSize;

                    StopExecutionTimer();

                    return FormatSolution($"The size of the region containing all locations with total distance less than 10k is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int ManhattanDistance(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }
    }
}