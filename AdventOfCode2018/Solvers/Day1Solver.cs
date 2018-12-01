using System;
using System.Linq;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public class Day1Solver : SolverBase
    {
        public override string DayName => "Day1";

        public override string Solve(ProblemPart part)
        {
            string input = GetInput();

            switch (part)
            {
                case ProblemPart.Part1:
                    string[] commands = input.Split('\n');

                    int currentValue = commands.Sum(command => int.Parse(command.Replace(" ", "")));

                    return currentValue.ToString();
                case ProblemPart.Part2:
                    throw new NotImplementedException();                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }
    }
}
