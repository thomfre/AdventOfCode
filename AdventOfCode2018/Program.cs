using System;
using System.Collections.Generic;
using Autofac;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018
{
    internal class Program
    {
        public static IContainer Container;

        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<SolverModule>();
            Container = builder.Build();


            IEnumerable<ISolver> solvers = Container.Resolve<IEnumerable<ISolver>>();

            foreach (var solver in solvers)
            {
                Console.WriteLine($"Solving - {solver.DayName}");
                Console.WriteLine("Part 1: " + solver.Solve(ProblemPart.Part1));
                Console.WriteLine("Part 2: " + solver.Solve(ProblemPart.Part2));
            }
        }
    }
}
