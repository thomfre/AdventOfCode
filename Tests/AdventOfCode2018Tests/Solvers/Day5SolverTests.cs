using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal class Day5SolverTests : SolverTestBase<Day5Solver>
    {
        protected override string TestData1 => "dabAcCaCBAcCcaDA";
        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 10;
        protected override object CorrectAnswer2 => 4;
    }
}
