using JetBrains.Annotations;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day13SolverTests : SolverTestBase<Day13Solver>
    {
        protected override string TestData1 => @"/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ";
        protected override string TestData2 => @"/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/";
        protected override object CorrectAnswer1 => "7,3";
        protected override object CorrectAnswer2 => "6,4";
    }
}
