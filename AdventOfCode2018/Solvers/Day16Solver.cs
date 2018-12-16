using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day16Solver : SolverBase
    {
        public Day16Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 16;

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] input = GetInput().Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            List<Sample> samples = new List<Sample>();

            int programStart = -1;

            for (int i = 0; i < input.Length; i += 4)
            {
                if (!input[i].StartsWith("Before"))
                {
                    programStart = i + 3;
                    break;
                }

                IEnumerable<int> start = input[i].Replace("Before: [", "").Replace("]", string.Empty).Split(',').Select(x => int.Parse(x.Trim()));
                IEnumerable<int> instruction = input[i + 1].Split(' ').Select(int.Parse);
                IEnumerable<int> end = input[i + 2].Replace("After:  [", "").Replace("]", string.Empty).Split(',').Select(x => int.Parse(x.Trim()));

                samples.Add(new Sample(start, end, instruction));
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    AnswerSolution1 = samples.Count(sample => sample.MatchingOperations.Count >= 3);

                    StopExecutionTimer();

                    return FormatSolution($"A total of [{ConsoleColor.Green}!{AnswerSolution1}] samples have three or more possible opcodes");
                case ProblemPart.Part2:
                    Dictionary<int, Operation> opCodes = new Dictionary<int, Operation>();

                    Dictionary<Operation, List<int>> possibleOpCodes = Enum.GetValues(typeof(Operation)).Cast<Operation>()
                                                                           .Select(o => new
                                                                                        {
                                                                                            operation = o,
                                                                                            possibilities = samples
                                                                                                           .Where(s => s.MatchingOperations.Contains(o))
                                                                                                           .Select(s => s.Instruction[0])
                                                                                                           .Distinct()
                                                                                                           .ToList()
                                                                                        }).ToDictionary(x => x.operation, x => x.possibilities);

                    possibleOpCodes.Where(p => p.Value.Count == 1).ForEach(p => opCodes.Add(p.Value[0], p.Key));
                    CleanOptions(possibleOpCodes, opCodes);

                    while (possibleOpCodes.Count > 0)
                    {
                        (Operation operation, List<int> candidates) = possibleOpCodes.OrderBy(p => p.Value.Count).First();
                        List<int> winningCandidates = new List<int>();
                        foreach (int candidate in candidates)
                        {
                            List<Sample> samplesToCheck = samples.Where(s => s.Instruction[0] == candidate).Select(s => s).ToList();
                            bool isWrong = false;
                            foreach (Sample sampleToCheck in samplesToCheck)
                            {
                                int[] testResult =
                                    Processor.Process(operation, sampleToCheck.Instruction.Skip(1).ToArray(), sampleToCheck.RegisterStart);
                                if (testResult.SequenceEqual(sampleToCheck.RegisterEnd))
                                {
                                    continue;
                                }

                                isWrong = true;
                                break;
                            }

                            if (!isWrong)
                            {
                                winningCandidates.Add(candidate);
                            }
                        }

                        if (winningCandidates.Count == 1)
                        {
                            opCodes.Add(winningCandidates[0], operation);
                            CleanOptions(possibleOpCodes, opCodes);
                        }
                    }

                    int[] registers = new int[4];
                    for (int i = programStart; i < input.Length; i++)
                    {
                        int[] instruction = input[i].Split(' ').Select(int.Parse).ToArray();

                        registers = Processor.Process(opCodes[instruction[0]], instruction.Skip(1).ToArray(), registers);
                    }

                    AnswerSolution2 = registers[0];

                    StopExecutionTimer();

                    return FormatSolution($"After running through all the instructions, the resulting value in register 0 is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void CleanOptions(Dictionary<Operation, List<int>> possibleOpCodes, Dictionary<int, Operation> opCodes)
        {
            foreach (KeyValuePair<int, Operation> opCode in opCodes)
            {
                possibleOpCodes.Remove(opCode.Value);
                possibleOpCodes.Where(p => p.Value.Contains(opCode.Key)).ForEach(p => p.Value.Remove(opCode.Key));
            }
        }

        internal class Sample
        {
            public Sample(IEnumerable<int> start, IEnumerable<int> end, IEnumerable<int> instruction)
            {
                RegisterStart = start.ToArray();
                RegisterEnd = end.ToArray();
                Instruction = instruction.ToArray();

                FindMatchingOperations();
            }

            public int[] RegisterStart { get; }
            public int[] RegisterEnd { get; }
            public int[] Instruction { get; }

            public List<Operation> MatchingOperations { get; } = new List<Operation>();

            private void FindMatchingOperations()
            {
                int[] instructionWithoutOperation = Instruction.Skip(1).ToArray();

                Enum.GetValues(typeof(Operation)).Cast<Operation>()
                    .ForEach(operation =>
                             {
                                 int[] resultingRegister = Processor.Process(operation, instructionWithoutOperation, RegisterStart);
                                 if (resultingRegister.SequenceEqual(RegisterEnd))
                                 {
                                     MatchingOperations.Add(operation);
                                 }
                             });
            }
        }

        internal static class Processor
        {
            public static int[] Process(Operation operation, int[] instruction, int[] registers)
            {
                int[] resultingRegister = new int[4];
                registers.CopyTo(resultingRegister, 0);

                int a = instruction[0];
                int b = instruction[1];
                int c = instruction[2];

                switch (operation)
                {
                    case Operation.Addr:
                        //addr (add register) stores into register C the result of adding register A and register B.
                        resultingRegister[c] = registers[a] + registers[b];
                        break;
                    case Operation.Addi:
                        //addi (add immediate) stores into register C the result of adding register A and value B.
                        resultingRegister[c] = registers[a] + b;
                        break;
                    case Operation.Mulr:
                        //mulr (multiply register) stores into register C the result of multiplying register A and register B.
                        resultingRegister[c] = registers[a] * registers[b];
                        break;
                    case Operation.Muli:
                        //muli (multiply immediate) stores into register C the result of multiplying register A and value B.
                        resultingRegister[c] = registers[a] * b;
                        break;
                    case Operation.Banr:
                        //banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                        resultingRegister[c] = registers[a] & registers[b];
                        break;
                    case Operation.Bani:
                        //bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                        resultingRegister[c] = registers[a] & b;
                        break;
                    case Operation.Borr:
                        //borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                        resultingRegister[c] = registers[a] | registers[b];
                        break;
                    case Operation.Bori:
                        //bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                        resultingRegister[c] = registers[a] | b;
                        break;
                    case Operation.Setr:
                        //setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                        resultingRegister[c] = registers[a];
                        break;
                    case Operation.Seti:
                        //seti (set immediate) stores value A into register C. (Input B is ignored.)
                        resultingRegister[c] = a;
                        break;
                    case Operation.Gtir:
                        //gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                        resultingRegister[c] = a > registers[b] ? 1 : 0;
                        break;
                    case Operation.Gtri:
                        //gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                        resultingRegister[c] = registers[a] > b ? 1 : 0;
                        break;
                    case Operation.Gtrr:
                        //gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                        resultingRegister[c] = registers[a] > registers[b] ? 1 : 0;
                        break;
                    case Operation.Eqir:
                        //eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                        resultingRegister[c] = a == registers[b] ? 1 : 0;
                        break;
                    case Operation.Eqri:
                        //eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                        resultingRegister[c] = registers[a] == b ? 1 : 0;
                        break;
                    case Operation.Eqrr:
                        //eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                        resultingRegister[c] = registers[a] == registers[b] ? 1 : 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
                }

                return resultingRegister;
            }
        }

        internal enum Operation
        {
            Addr,
            Addi,
            Mulr,
            Muli,
            Banr,
            Bani,
            Borr,
            Bori,
            Setr,
            Seti,
            Gtir,
            Gtri,
            Gtrr,
            Eqir,
            Eqri,
            Eqrr
        }
    }
}