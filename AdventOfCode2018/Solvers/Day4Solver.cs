using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Thomfre.AdventOfCode2018.Solvers
{
    [UsedImplicitly]
    internal class Day4Solver : SolverBase
    {
        public override int DayNumber => 4;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();

            string input = GetInput();
            Dictionary<DateTime, string> guardMovements = input.Split('\n')
                                                               .Select(m => new {Date = DateTime.Parse(m.Substring(1, 16)), Movement = m.Substring(18, m.Length - 18)})
                                                               .OrderBy(m => m.Date)
                                                               .ToDictionary(m => m.Date, m => m.Movement);

            GuardShift currentShift = null;
            DateTime lastTime = DateTime.MinValue;
            GuardState lastState;
            List<GuardShift> guardShifts = new List<GuardShift>();
            foreach (KeyValuePair<DateTime, string> guardMovement in guardMovements)
            {
                ;

                if (guardMovement.Value.Contains("falls asleep"))
                {
                    TimeSpan awakeTime = guardMovement.Key - lastTime;

                    currentShift.AwakeTime += (int) awakeTime.TotalMinutes;

                    lastState = GuardState.Asleep;
                }
                else if (guardMovement.Value.Contains("wakes up"))
                {
                    TimeSpan sleepTime = guardMovement.Key - lastTime;

                    currentShift.SleepTime += (int) sleepTime.TotalMinutes;

                    lastState = GuardState.Awake;
                }
                else if (guardMovement.Value.Contains("#"))
                {
                    if (currentShift != null)
                    {
                        guardShifts.Add(currentShift);
                    }

                    int start = guardMovement.Value.IndexOf("#", StringComparison.Ordinal);
                    int end = guardMovement.Value.IndexOf(" ", start, StringComparison.Ordinal);
                    currentShift = new GuardShift {Date = guardMovement.Key, GuardId = int.Parse(guardMovement.Value.Substring(start + 1, end - start - 1))};
                    lastState = GuardState.Awake;
                }

                lastTime = guardMovement.Key;
            }

            guardShifts.Add(currentShift);

            switch (part)
            {
                case ProblemPart.Part1:

                    var guard = guardShifts.GroupBy(s => s.GuardId).Select(s => new {GuardId = s.Key, TotalSleep = s.Sum(ss => ss.SleepTime)}).OrderByDescending(s => s.TotalSleep).FirstOrDefault();

                    StopExecutionTimer();

                    return FormatSolution($"Not solved yet: {guard.GuardId} : {guard.TotalSleep}");
                case ProblemPart.Part2:

                    StopExecutionTimer();

                    return FormatSolution($"Not solved yet");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        internal class GuardShift
        {
            public DateTime Date { get; set; }
            public int GuardId { get; set; }
            public int AwakeTime { get; set; }
            public int SleepTime { get; set; }
        }

        internal enum GuardState
        {
            Awake,
            Asleep
        }
    }
}