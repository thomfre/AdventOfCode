using System.Diagnostics;
using System.IO;
using Humanizer;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public abstract class SolverBase : ISolver
    {
        protected Stopwatch ExecutionTimer;

        protected SolverBase()
        {
            ExecutionTimer = new Stopwatch();
        }

        protected string InputFile => $@"Input\{DayName}.input";

        public abstract string DayName { get; }

        public abstract string Solve(ProblemPart part);

        protected string GetInput()
        {
            return File.ReadAllText(InputFile);
        }

        protected void StartExecutionTimer()
        {
            ExecutionTimer.Reset();
            ExecutionTimer.Start();
        }

        protected void StopExecutionTimer()
        {
            ExecutionTimer.Stop();
        }

        protected string FormatSolution(string solution)
        {
            return $"{solution} - Execution time: {ExecutionTimer.Elapsed.Humanize()}";
        }
    }
}
