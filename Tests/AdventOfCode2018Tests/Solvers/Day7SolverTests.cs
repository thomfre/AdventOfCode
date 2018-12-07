using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal class Day7SolverTests : SolverTestBase<Day7Solver>
    {
        protected override string TestData1 => @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => "CABDFE";
        protected override object CorrectAnswer2 => 15;

        public override void CustomSetup()
        {
            Solver.WorkersAvailable = 2;
            Solver.BaseTimeCost = 0;
        }
    }
}
