using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Solvers;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    [UsedImplicitly]
    internal class Day15Tests : SolverTestBase<Day15Solver>
    {
        protected override string TestData1 => @"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";
        protected override string TestData2 => @"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";
        protected override object CorrectAnswer1 => 27730;
        protected override object CorrectAnswer2 => 4988;

        [TestCase(@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######", 27730)]
        [TestCase(@"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######", 36334)]
        [TestCase(@"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######", 39514)]
        [TestCase(@"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######", 27755)]
        [TestCase(@"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######", 28944)]
        [TestCase(@"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########", 18740)]
        [TestCase(@"################################
##########.###.###..############
##########..##......############
#########...##....##############
######.....###..G..G############
##########..........############
##########.............#########
#######G..#.G...#......#########
#..G##....##..#.G#....#...######
##......###..##..####.#..#######
#G.G..#..#....#.###...G..#######
#.....GG##................######
#....G........#####....E.E.#####
#####G...#...#######........####
####.E#.G...#########.......####
#...G.....#.#########......#####
#.##........#########.......####
######......#########........###
######......#########..E.#....##
#######..E.G.#######..........##
#######E......#####............#
#######...G............E.......#
####............##............##
####..G.........##..........E.##
####.G.G#.....####E...##....#.##
#######.......####...####..#####
########....E....########..#####
##########.......#########...###
##########.......#########..####
##########....############..####
###########...##################
################################", 235400)]
        public void Solution_for_first_part_is_calculated_correctly(string input, int correctAnswer)
        {
            A.CallTo(() => AutoFake.Resolve<IInputLoader>().LoadInput(A<int>._)).Returns(input);
            Solver.Solve(ProblemPart.Part1);
            Solver.Answer1.Should().Be(correctAnswer);
        }
    }
}
