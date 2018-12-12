using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day12SolverTests : SolverTestBase<Day12Solver>
    {
        protected override string TestData1 => @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";
        protected override string TestData2 => TestData1;
        protected override object CorrectAnswer1 => 325;
        protected override object CorrectAnswer2 => 50000000501L;
    }
}