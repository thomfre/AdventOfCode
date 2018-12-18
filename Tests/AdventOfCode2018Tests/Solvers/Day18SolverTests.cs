using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day18SolverTests : SolverTestBase<Day18Solver>
    {
        protected override string TestData1 => @".#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.";

        protected override string TestData2 => TestData1;

        protected override object CorrectAnswer1 => 1147;
        protected override object CorrectAnswer2 => 0;
    }
}
