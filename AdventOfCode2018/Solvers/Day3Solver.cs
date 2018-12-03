﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day3Solver : SolverBase
    {
        public override int DayNumber => 3;

        private Claim[] _claims;
        private Dictionary<Coordinate, HashSet<int>> _fabricMap;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();

            if (_fabricMap == null)
            {
                string input = GetInput();
                _claims = input.Split('\n').Select(c => new Claim(c)).ToArray();
                _fabricMap = new Dictionary<Coordinate, HashSet<int>>();
                foreach (Claim claim in _claims)
                {
                    for (int x = claim.Left; x < claim.Left + claim.Width; x++)
                    {
                        for (int y = claim.Top; y < claim.Top + claim.Height; y++)
                        {
                            Coordinate coordinate = new Coordinate(x, y);
                            if (_fabricMap.ContainsKey(coordinate))
                            {
                                _fabricMap[coordinate].Add(claim.ClaimId);
                            }
                            else
                            {
                                _fabricMap.Add(coordinate, new HashSet<int> { claim.ClaimId });
                            }
                        }
                    }
                }
            }            

            switch (part)
            {
                case ProblemPart.Part1:                    
                    int collisions = _fabricMap.Count(m => m.Value.Count > 1);

                    StopExecutionTimer();

                    return FormatSolution($"The number of colliding squares are [{ConsoleColor.Red}!{collisions}]");
                case ProblemPart.Part2:
                    int uniqueClaimId = -1;

                    foreach (Claim claim in _claims)
                    {
                        if (_fabricMap.Any(f => f.Value.Contains(claim.ClaimId) && f.Value.Count > 1))
                        {
                            continue;
                        }

                        uniqueClaimId = claim.ClaimId;
                        break;
                    }

                    StopExecutionTimer();

                    return FormatSolution($"The only claim with unique squares claimed are [{ConsoleColor.Red}!{uniqueClaimId}]");
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