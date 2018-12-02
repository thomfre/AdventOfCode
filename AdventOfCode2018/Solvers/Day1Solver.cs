using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day1Solver : SolverBase
    {
        private HashSet<int> _frequencyList;
        private int _iterationCounter;
        public override int DayNumber => 1;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] commands = input.Split('\n');

            switch (part)
            {
                case ProblemPart.Part1:
                    int frequency = commands.Sum(command => int.Parse(command.Replace(" ", "")));

                    StopExecutionTimer();

                    return FormatSolution($"The resulting frequency is [{ConsoleColor.Green}!{frequency}]");
                case ProblemPart.Part2:
                    _frequencyList = new HashSet<int>();

                    int firstRepeatedFrequency = LookForDuplicateFrequencies(commands, 0);

                    StopExecutionTimer();

                    return
                        FormatSolution($"After [{ConsoleColor.Yellow}!{_iterationCounter}] full iterations, the first repeated frequency found was [{ConsoleColor.Green}!{firstRepeatedFrequency}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int LookForDuplicateFrequencies(string[] commands, int currentFrequency)
        {
            foreach (string command in commands)
            {
                currentFrequency += int.Parse(command.Replace(" ", ""));

                if (_frequencyList.Contains(currentFrequency))
                {
                    return currentFrequency;
                }

                _frequencyList.Add(currentFrequency);
            }

            _iterationCounter++;

            return LookForDuplicateFrequencies(commands, currentFrequency);
        }
    }
}