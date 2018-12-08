using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day2SolverTests : SolverTestBase<Day2Solver>
    {
        protected override string TestData1 => @"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab";

        protected override string TestData2 => @"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";

        protected override object CorrectAnswer1 => 12;
        protected override object CorrectAnswer2 => "fgij";
    }
}
