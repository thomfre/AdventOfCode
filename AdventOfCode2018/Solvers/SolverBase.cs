using System;
using System.Diagnostics;
using Humanizer;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public abstract class SolverBase : ISolver
    {
        private readonly IInputLoader _inputLoader;
        protected object AnswerSolution1;
        protected object AnswerSolution2;

        protected Stopwatch ExecutionTimer;

        protected SolverBase(IInputLoader inputLoader)
        {
            _inputLoader = inputLoader;

            ExecutionTimer = new Stopwatch();
        }

        public TimeSpan ExecutionTime => ExecutionTimer.Elapsed;

        public abstract int DayNumber { get; }

        public object Answer1 => AnswerSolution1;
        public object Answer2 => AnswerSolution2;

        public abstract string Solve(ProblemPart part);

        protected string GetInput()
        {
            return _inputLoader.LoadInput(DayNumber);
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
            return $"{solution} - Execution time: [Magenta!{ExecutionTimer.Elapsed.Humanize()}]";
        }
    }
}