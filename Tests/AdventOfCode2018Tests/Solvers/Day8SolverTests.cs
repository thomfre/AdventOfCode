using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day8SolverTests : SolverTestBase<Day8Solver>
    {
        protected override string TestData1 => "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
        protected override string TestData2 => TestData1;
        protected override object CorrectAnswer1 => 138;
        protected override object CorrectAnswer2 => 66;
    }
}