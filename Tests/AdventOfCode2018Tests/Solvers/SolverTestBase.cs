using Autofac;
using FakeItEasy;
using NUnit.Framework;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Tests.Solvers
{
    internal abstract class SolverTestBase<TSolver>
    {
        protected IContainer Container;
        protected Fake<IInputLoader> FakeInputLoader;

        protected abstract string TestData { get; }

        public TSolver Solver { get; set; }

        [SetUp]
        public void Setup()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<SolverModule>();
            FakeInputLoader = new Fake<IInputLoader>();
            FakeInputLoader.AnyCall().WithReturnType<string>().Returns(TestData);
            builder.Register(r => (IInputLoader)FakeInputLoader).As<IInputLoader>();

            Container = builder.Build();

            Solver = Container.Resolve<TSolver>();
        }

        [Test]
        public abstract void TestInput1();

        [Test]
        public abstract void TestInput2();
    }
}