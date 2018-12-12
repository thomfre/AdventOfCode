using System;
using System.Collections.Generic;
using System.Linq;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day1Solver : SolverBase
    {
        private HashSet<int> _frequencyList;
        private int _iterationCounter;

        public Day1Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 1;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] commands = input.Split('\n');

            switch (part)
            {
                case ProblemPart.Part1:
                    int frequency = commands.Sum(command => int.Parse(command.Replace(" ", string.Empty)));

                    AnswerSolution1 = frequency;

                    StopExecutionTimer();

                    return FormatSolution($"The resulting frequency is [{ConsoleColor.Green}!{frequency}]");
                case ProblemPart.Part2:
                    _frequencyList = new HashSet<int>();
                    _iterationCounter = 0;

                    int firstRepeatedFrequency = LookForDuplicateFrequencies(commands, 0);

                    AnswerSolution2 = firstRepeatedFrequency;

                    StopExecutionTimer();

                    return
                        FormatSolution($"After [{ConsoleColor.Yellow}!{_iterationCounter}] full iterations ({_frequencyList.Count - 1} different frequencies), the first repeated frequency found was [{ConsoleColor.Green}!{firstRepeatedFrequency}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private int LookForDuplicateFrequencies(string[] commands, int currentFrequency)
        {
            foreach (string command in commands)
            {
                currentFrequency += int.Parse(command.Replace(" ", string.Empty));

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
