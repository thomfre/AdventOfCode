using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day16SolverTests : SolverTestBase<Day16Solver>
    {
        protected override string TestData1 => @"Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]";

        protected override string TestData2 => TestData1;
        protected override object CorrectAnswer1 => 1;
        protected override object CorrectAnswer2 => null;
    }
}
