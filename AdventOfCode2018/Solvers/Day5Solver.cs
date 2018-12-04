using System;
using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day5Solver : SolverBase
    {
        public Day5Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 5;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string input = GetInput();
            string[] inputSplit = input.Split('\n');

            switch (part)
            {
                case ProblemPart.Part1:

                    AnswerSolution1 = null;

                    StopExecutionTimer();

                    return FormatSolution($"Not solved");
                case ProblemPart.Part2:

                    AnswerSolution2 = null;

                    StopExecutionTimer();

                    return FormatSolution($"Not solved");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }
}
