using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day3Solver : SolverBase
    {
        public override int DayNumber => 3;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();

            string input = GetInput();
            Claim[] claims = input.Split('\n').Select(c => new Claim(c)).ToArray();
            Dictionary<Coordinate, int> fabricMap = new Dictionary<Coordinate, int>();

            switch (part)
            {
                case ProblemPart.Part1:
                    foreach (Claim claim in claims)
                    {
                        for (int x = claim.Left; x < claim.Left + claim.Width; x++)
                        {
                            for (int y = claim.Top; y < claim.Top + claim.Height; y++)
                            {
                                Coordinate coordinate = new Coordinate(x, y);
                                if (fabricMap.ContainsKey(coordinate))
                                {
                                    fabricMap[coordinate]++;
                                }
                                else
                                {
                                    fabricMap.Add(coordinate, 1);
                                }
                            }
                        }
                    }

                    int collisions = fabricMap.Count(m => m.Value > 1);

                    StopExecutionTimer();

                    return FormatSolution($"The number of colliding squares are [{ConsoleColor.Red}!{collisions}]");
                case ProblemPart.Part2:
                    StopExecutionTimer();

                    return "Not solved yet";
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        internal struct Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        internal class Claim
        {
            public Claim(string claim)
            {
                string[] claimParts = claim.Split(' ');
                ClaimId = int.Parse(claimParts[0].Replace("#", ""));
                int[] xy = claimParts[2].Replace(":", "").Split(',').Select(int.Parse).ToArray();
                Left = xy[0];
                Top = xy[1];
                int[] wh = claimParts[3].Split('x').Select(int.Parse).ToArray();
                Width = wh[0];
                Height = wh[1];
            }

            public int ClaimId { get; }
            public int Left { get; }
            public int Top { get; }
            public int Width { get; }
            public int Height { get; }
        }
    }
}