using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Humanizer;
using OutputColorizer;
using Thomfre.AdventOfCode2018.Solvers;

namespace Thomfre.AdventOfCode2018
{
    internal class SolutionPresenter : ISolutionPresenter
    {
        private readonly IEnumerable<ISolver> _solvers;

        public SolutionPresenter(IEnumerable<ISolver> solvers)
        {
            _solvers = solvers;
        }

        public void Start()
        {
            Console.Clear();

            Console.SetCursorPosition(0, 0);
            Colorizer.WriteLine($"[{ConsoleColor.Red}!Advent of Code 2018 - Solver]");
            Colorizer.WriteLine($"[{ConsoleColor.DarkGreen}!by thomfre]");
            Colorizer.WriteLine("[Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*][Green!*][Red!*]");
            Console.WriteLine();

            OutputMenu();
        }

        private static void ClearAndResetOutput()
        {
            int currentRow = Console.CursorTop - 1;
            while (currentRow >= 4)
            {
                Console.SetCursorPosition(0, currentRow);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentRow);
                currentRow--;
            }
        }

        private void OutputMenu()
        {
            Colorizer.WriteLine($"[{ConsoleColor.Blue}!Menu]");
            Colorizer.WriteLine($"[{ConsoleColor.Cyan}!All]: Output all solutions");
            Colorizer.WriteLine($"[{ConsoleColor.Cyan}!Day x]: Output solution for day x, replace x with day number");
            Colorizer.WriteLine($"[{ConsoleColor.Cyan}!Exit]: Exit application");

            string selection = ReadLine.Read("Please enter your selection: ");
            ClearAndResetOutput();

            if (selection.ToLower() == "all")
            {
                TimeSpan totalExecutionTime = TimeSpan.Zero;
                foreach (ISolver solver in _solvers)
                {
                    totalExecutionTime = totalExecutionTime.Add(ShowSolution(solver));
                }
                Colorizer.WriteLine($"[{ConsoleColor.Magenta}!Total execution time for all calculations: {totalExecutionTime.Humanize()}]");
            }
            else if (selection.ToLower().StartsWith("day "))
            {
                int.TryParse(Regex.Match(selection, @"\d+").Value, out int dayNumber);
                if (dayNumber < 1 || dayNumber > 24)
                {
                    Colorizer.WriteLine($"[{ConsoleColor.DarkRed}!Error:] invalid day selection");
                }
                else
                {
                    ISolver solver = _solvers.FirstOrDefault(s => s.DayNumber == dayNumber);
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
                Colorizer.WriteLine($"[{ConsoleColor.DarkRed}!Error:] Invalid selection");
            }

            OutputMenu();
        }

        private TimeSpan ShowSolution(ISolver solver)
        {
            TimeSpan totalExecutionTime = TimeSpan.Zero;

            try
            {                
                Colorizer.WriteLine($"Solving - Day {solver.DayNumber}");
                Console.Write("Working...");
                Colorizer.WriteLine($"\rPart 1: {solver.Solve(ProblemPart.Part1)}");
                totalExecutionTime = totalExecutionTime.Add(solver.ExecutionTime);
                Console.Write("Working...");

                Colorizer.WriteLine($"\rPart 2: {solver.Solve(ProblemPart.Part2)}");
                totalExecutionTime = totalExecutionTime.Add(solver.ExecutionTime);
                Console.WriteLine("------");                
            }
            catch (Exception exception)
            {
                Colorizer.WriteLine($"[{ConsoleColor.DarkRed}!Unhandled exception] - oh wow, thomfre really messed up this time: {exception.Message}");
            }

            return totalExecutionTime;
        }
    }
}