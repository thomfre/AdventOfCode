using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day17SolverTests : SolverTestBase<Day17Solver>
    {
        protected override string TestData1 => @"x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504";

        protected override string TestData2 => TestData1;
        protected override object CorrectAnswer1 => 57;
        protected override object CorrectAnswer2 => 29;
    }
}
