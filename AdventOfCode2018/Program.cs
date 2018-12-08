using Autofac;

namespace Thomfre.AdventOfCode2018
{
    internal class Program
    {
        public static IContainer Container;

        private static void Main()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<SolverModule>();
            Container = builder.Build();
        }
    }
}