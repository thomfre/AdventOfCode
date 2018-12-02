using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Autofac;
using OutputColorizer;
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
        }
    }
}
