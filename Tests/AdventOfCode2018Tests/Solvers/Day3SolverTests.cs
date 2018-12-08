using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day3SolverTests : SolverTestBase<Day3Solver>
    {
        protected override string TestData1 => @"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 4;
        protected override object CorrectAnswer2 => 3;
    }
}
