namespace Thomfre.AdventOfCode2018.Solvers
{
    public interface ISolver
    {
        string DayName { get; }
        string Solve(ProblemPart part);
    }
}
