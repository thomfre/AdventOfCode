using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal class Day6SolverTests : SolverTestBase<Day6Solver>
    {
        protected override string TestData1 => @"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 17;
        protected override object CorrectAnswer2 => 16;

        public override void CustomSetup()
        {
            Solver.TotalDistanceMustBeUnder = 32;
        }
    }
}