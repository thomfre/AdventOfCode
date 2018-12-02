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

            Console.Clear();
            Console.SetCursorPosition(0,0);
            Colorizer.WriteLine($"[{ConsoleColor.Red}!Advent of Code 2018 - Solver]");
            Colorizer.WriteLine($"[{ConsoleColor.Gray}!by thomfre]");
            Console.WriteLine();

            OutputMenu();
        }

        private static void ClearAndResetOutput()
        {
            int currentRow = Console.CursorTop - 1;
            while (currentRow >= 3)
            {
                Console.SetCursorPosition(0, currentRow);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentRow);
                currentRow--;
            }
        }

        private static void OutputMenu()
        {
            Colorizer.WriteLine("[Blue!Menu]");
            Colorizer.WriteLine("[Cyan!All]: Output all solutions");
            Colorizer.WriteLine("[Cyan!Day x]: Output solution for day x, replace x with day number");
            Colorizer.WriteLine("[Cyan!Exit]: Exit application");

            string selection = ReadLine.Read("Please enter your selection: ");
            ClearAndResetOutput();

            IEnumerable<ISolver> solvers = Container.Resolve<IEnumerable<ISolver>>();

            if (selection.ToLower() == "all")
            {
                foreach (var solver in solvers)
                {
                    Colorizer.WriteLine($"Solving - Day {solver.DayNumber}");
                    Console.Write("Working...");
                    Colorizer.WriteLine($"\rPart 1: {solver.Solve(ProblemPart.Part1)}");
                    Console.Write("Working...");

                    Colorizer.WriteLine($"\rPart 2: {solver.Solve(ProblemPart.Part2)}");
                    Console.WriteLine("------");
                }
            }
            else if (selection.ToLower().StartsWith("day "))
            {
                int.TryParse(Regex.Match(selection, @"\d+").Value, out var dayNumber);
                if (dayNumber < 1 || dayNumber > 24)
                {
                    Colorizer.WriteLine($"[{ConsoleColor.DarkRed}!Error:] invalid day selection");
                }
                else
                {
                    var solver = solvers.FirstOrDefault(s => s.DayNumber == dayNumber);
                    if (solver == null)
                    {
                        Colorizer.WriteLine($"[{ConsoleColor.DarkRed}!Error:] no solver found for selected day");
                    }
                    else
                    {
                        Colorizer.WriteLine($"Solving - Day {solver.DayNumber}");
                        Console.Write("Working...");
                        Colorizer.WriteLine($"\rPart 1: {solver.Solve(ProblemPart.Part1)}");
                        Console.Write("Working...");

                        Colorizer.WriteLine($"\rPart 2: {solver.Solve(ProblemPart.Part2)}");
                        Console.WriteLine("------");
                    }
                }
            }
            else if (selection.ToLower() == "exit")
            {
                return;
            }
            else
            {
                
            }

            OutputMenu();
        }
    }
}
