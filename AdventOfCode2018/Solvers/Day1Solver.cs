using System;
using System.Collections.Generic;
using System.Linq;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public class Day1Solver : SolverBase
    {
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

                    return FormatSolution($"The resulting frequency is [Green!{frequency}]");
                case ProblemPart.Part2:
                    _frequencyList = new List<int>();

                    int firstRepeatedFrequency = LookForDuplicateFrequencies(commands, 0);

                    StopExecutionTimer();

                    return FormatSolution($"After {_iterationCounter} full iterations, the first repeated frequency found was [Green!{firstRepeatedFrequency}]");                   
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private List<int> _frequencyList;
        private int _iterationCounter = 0;

        private int LookForDuplicateFrequencies(string[] commands, int currentFrequency)
        {
            foreach (string command in commands)
            {
                currentFrequency += int.Parse(command.Replace(" ", ""));

                if (_frequencyList.Contains(currentFrequency)) return currentFrequency;
                _frequencyList.Add(currentFrequency);                
            }

            _iterationCounter++;

            return LookForDuplicateFrequencies(commands, currentFrequency);
        }
    }
}
