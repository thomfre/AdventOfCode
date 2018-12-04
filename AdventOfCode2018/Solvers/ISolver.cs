using System;

namespace Thomfre.AdventOfCode2018.Solvers
{
    public interface ISolver
    {
        int DayNumber { get; }
        object Answer1 { get; }
        object Answer2 { get; }
        TimeSpan ExecutionTime { get; }
        string Solve(ProblemPart part);
    }
}