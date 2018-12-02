using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day2Solver : SolverBase
    {
        public override int DayNumber => 2;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] boxIds = input.Split('\n');

            switch (part)
            {
                case ProblemPart.Part1:
                    int containsTwo = 0;
                    int containsThree = 0;

                    foreach (string boxId in boxIds)
                    {
                        var charCounts = boxId.ToCharArray().GroupBy(c => c).Select(c => new {Char = c.Key, Count = c.Count()}).ToArray();
                        if (charCounts.Any(c => c.Count == 2))
                        {
                            containsTwo++;
                        }

                        if (charCounts.Any(c => c.Count == 3))
                        {
                            containsThree++;
                        }
                    }

                    int checksum = containsTwo * containsThree;

                    StopExecutionTimer();
                    return FormatSolution($"The checksum for the box IDs are [{ConsoleColor.Red}!{checksum}]");
                case ProblemPart.Part2:

                    Dictionary<string, char[]> boxIdDictionary = boxIds.ToDictionary(b => b.Trim(), c => c.Trim().ToCharArray());

                    int similaritiesNeeded = boxIdDictionary.First().Value.Length - 1;
                    foreach (KeyValuePair<string, char[]> boxId in boxIdDictionary)
                    {
                        foreach (KeyValuePair<string, char[]> boxIdOther in boxIdDictionary)
                        {
                            int matching = 0;
                            string matchingChars = "";
                            for (int i = 0; i < boxId.Value.Length; i++)
                            {
                                if (boxId.Value[i] != boxIdOther.Value[i])
                                {
                                    continue;
                                }

                                matching++;
                                matchingChars += boxId.Value[i];
                            }

                            if (matching != similaritiesNeeded)
                            {
                                continue;
                            }

                            StopExecutionTimer();

                            return FormatSolution($"The common letters between [{ConsoleColor.Yellow}!{boxId.Key}] and [{ConsoleColor.Yellow}!{boxIdOther.Key}] is [{ConsoleColor.Red}!{matchingChars}]");
                        }
                    }

                    StopExecutionTimer();
                    return "Unable to find solution";
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }
}