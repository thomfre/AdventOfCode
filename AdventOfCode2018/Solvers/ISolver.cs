namespace Thomfre.AdventOfCode2018.Solvers
{
    public interface ISolver
    {
        int DayNumber { get; }
        string Solve(ProblemPart part);
    }
}
