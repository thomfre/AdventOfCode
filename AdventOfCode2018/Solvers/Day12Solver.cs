using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day12Solver : SolverBase
    {
        public Day12Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 12;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] input = GetInput().Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            bool[] initialState = input[0].Replace("initial state:", string.Empty).Trim().ToCharArray().Select(c => c == '#').ToArray();

            Dictionary<int, bool> growthRules = input.Skip(2)
                                                     .Select(l =>
                                                                 (l.Split(' ').First().ToCharArray()
                                                                   .Select((pot, iterator) => new {pot, iterator})
                                                                   .Where(x => x.pot == '#').Sum(x => (int) Math.Pow(2, x.iterator)),
                                                                  l.Split(' ').Last().Select(x => x == '#').First()))
                                                     .ToDictionary(x => x.Item1, x => x.Item2);

            HashSet<int> plants = new HashSet<int>();
            for (int i = 0; i < initialState.Length; i++)
            {
                if (initialState[i])
                {
                    plants.Add(i);
                }
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    AnswerSolution1 = EvolvePlants(plants, growthRules, 20);

                    StopExecutionTimer();

                    return FormatSolution($"The answer [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    AnswerSolution2 = EvolvePlants(plants, growthRules, 50000000000);

                    StopExecutionTimer();

                    return FormatSolution($"The answer [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private long EvolvePlants(HashSet<int> plants, Dictionary<int, bool> growthRules, long numberOfIterations)
        {
            long plantScore = 0, lastPlantScore = 0;
            long lastPlantScoreDiff = 0;
            for (int iteration = 1; iteration <= numberOfIterations; iteration++)
            {
                HashSet<int> nextPlants = new HashSet<int>();
                for (int pot = plants.Min() - 3; pot <= plants.Max() + 3; pot++)
                {
                    int sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (plants.Contains(pot + i - 2))
                        {
                            sum += (int) Math.Pow(2, i);
                        }
                    }

                    if (growthRules.ContainsKey(sum) && growthRules[sum])
                    {
                        nextPlants.Add(pot);
                    }
                }

                plantScore = nextPlants.Sum();
                long plantScoreDiff = plantScore - lastPlantScore;
                if (plantScoreDiff == lastPlantScoreDiff)
                {
                    plantScore = plantScore + (numberOfIterations - iteration) * lastPlantScoreDiff;
                    break;
                }

                lastPlantScoreDiff = plantScoreDiff;
                lastPlantScore = plantScore;

                plants = nextPlants;
            }

            return plantScore;
        }
    }
}
