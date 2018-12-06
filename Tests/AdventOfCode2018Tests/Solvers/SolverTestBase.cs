using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Solvers;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal abstract class SolverTestBase<TSolver> where TSolver : ISolver
    {
        private AutoFake _autoFake;
        protected abstract string TestData1 { get; }
        protected abstract string TestData2 { get; }
        protected abstract object CorrectAnswer1 { get; }
        protected abstract object CorrectAnswer2 { get; }

        public TSolver Solver { get; set; }

        [SetUp]
        public void Setup()
        {
            _autoFake = new AutoFake();
            Solver = _autoFake.Resolve<TSolver>();
            CustomSetup();
        }

        public virtual void CustomSetup()
        {

        }

        [TearDown]
        public void TearDown()
        {
            _autoFake?.Dispose();
        }

        [Test]
        public void Solution_for_first_part_is_calculated_correctly()
        {
            A.CallTo(() => _autoFake.Resolve<IInputLoader>().LoadInput(A<int>._)).Returns(TestData1);
            Solver.Solve(ProblemPart.Part1);
            Solver.Answer1.Should().Be(CorrectAnswer1);
        }

        [Test]
        public void Solution_for_second_part_is_calculated_correctly()
        {
            A.CallTo(() => _autoFake.Resolve<IInputLoader>().LoadInput(A<int>._)).Returns(TestData2);
            Solver.Solve(ProblemPart.Part2);
            Solver.Answer2.Should().Be(CorrectAnswer2);
        }
    }
}
