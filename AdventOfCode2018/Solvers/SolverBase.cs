using System.IO;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public abstract class SolverBase : ISolver
    {
        public abstract string DayName { get; }

        protected string InputFile => $@"Input\{DayName}.input";

        public abstract string Solve(ProblemPart part);

        protected string GetInput()
        {
            return File.ReadAllText(InputFile);
        }
    }
}
