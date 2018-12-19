using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Thomfre.AdventOfCode2018.Tools;

namespace Thomfre.AdventOfCode2018.Solvers
{
    internal class Day19Solver : SolverBase
    {
        public Day19Solver(IInputLoader inputLoader) : base(inputLoader)
        {
        }

        public override int DayNumber => 19;

        long[] _registers = new long[6];

        public override string Solve(ProblemPart part)
        {
            StartExecutionTimer();
            string[] input = GetInput().Trim().Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            int instructionPointerLocation = int.Parse(input[0].Replace("#ip ", ""));            

            Processor processor = new Processor(instructionPointerLocation);

            List<(Operation operation, int[] register)> instructions = new List<(Operation operation, int[] register)>();
            for (int i = 1; i < input.Length; i++)
            {
                string[] instruction = input[i].Split(' ');
                Operation operation = (Operation) Enum.Parse(typeof(Operation), instruction[0], true);
                int[] registers = instruction.Skip(1).Select(int.Parse).ToArray();
                instructions.Add((operation, registers));
            }

            switch (part)
            {
                case ProblemPart.Part1:
                    RunApplication(processor, instructions, instructionPointerLocation);

                    AnswerSolution1 = _registers[0];
                    StopExecutionTimer();

                    return FormatSolution($"The value of register 0 after the program has been halted is [{ConsoleColor.Green}!{AnswerSolution1}]");
                case ProblemPart.Part2:
                    _registers = new long[6];
                    _registers[0] = 1;
                    RunApplication(processor, instructions, instructionPointerLocation);

                    AnswerSolution2 = _registers[0];

                    StopExecutionTimer();

                    return FormatSolution($"After running through all the instructions, the resulting value in register 0 is [{ConsoleColor.Green}!{AnswerSolution2}]");
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        private void RunApplication(Processor processor, List<(Operation operation, int[] register)> instructions, int instructionPointerLocation)
        {
            while (true)
            {
                long instructionPointer = _registers[instructionPointerLocation];
                if (instructionPointer >= instructions.Count)
                {
                    break;
                }

                (Operation Operation, int[] Register) instruction = instructions[(int)instructionPointer];

                _registers = processor.Process(instruction.Operation, instruction.Register, _registers);
                Console.WriteLine(string.Join(",", _registers));
            }
        }
    }

    [SuppressMessage("ReSharper", "CommentTypo")]
    internal class Processor
    {
        private readonly int _instructionPointerLocation;

        public Processor(int pointerLocation)
        {
            _instructionPointerLocation = pointerLocation;
        }

        public long[] Process(Operation operation, int[] instruction, long[] registers)
        {
            long[] resultingRegister = new long[6];
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

            resultingRegister[_instructionPointerLocation]++;

            return resultingRegister;
        }
    }

    [SuppressMessage("ReSharper", "IdentifierTypo")]
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
