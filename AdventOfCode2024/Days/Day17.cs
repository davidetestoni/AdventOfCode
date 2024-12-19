using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public partial class Day17(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day17.txt");

    [Fact]
    public void Part1()
    {
        var registerA = int.Parse(RegisterRegex().Match(_lines[0]).Groups[1].Value);
        var registerB = int.Parse(RegisterRegex().Match(_lines[1]).Groups[1].Value);
        var registerC = int.Parse(RegisterRegex().Match(_lines[2]).Groups[1].Value);

        var program = ProgramRegex()
            .Match(_lines[4]).Groups[1].Value
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        var emulator = new Emulator(registerA, registerB, registerC, program);
        
        var outputString = string.Join(',', emulator.Execute().Select(o => o.ToString()));
        
        output.WriteLine(outputString);
        
        Assert.Equal("3,7,1,7,2,1,0,6,3", outputString);
    }

    [Fact]
    public void Part2()
    {
        var registerB = int.Parse(RegisterRegex().Match(_lines[1]).Groups[1].Value);
        var registerC = int.Parse(RegisterRegex().Match(_lines[2]).Groups[1].Value);

        var program = ProgramRegex()
            .Match(_lines[4]).Groups[1].Value
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        // TODO: I scanned the entire int space without finding a match.
        // Maybe I need to use longs?
        var a = 0;

        while (true)
        {
            // There is an output before a jump
            
            var emulator = new Emulator(a, registerB, registerC, program, matchInstructions: true);
            var outputs = emulator.Execute();

            if (program.Length == outputs.Count && program.SequenceEqual(outputs))
            {
                break;
            }

            if (a % 10_000_000 == 0)
            {
                output.WriteLine(a.ToString());
            }
            
            a++;
        }
        
        output.WriteLine(a.ToString());
    }

    private class Emulator(int a, int b, int c, int[] instructions, bool matchInstructions = false)
    {
        private int A { get; set; } = a;
        private int B { get; set; } = b;
        private int C { get; set; } = c;
        private bool MatchInstructions { get; } = matchInstructions;

        private int[] Instructions { get; } = instructions;

        // Executes the program and returns outputs
        public List<int> Execute()
        {
            // Instruction pointer
            var ip = 0;
            var outputs = new List<int>();

            while (ip < Instructions.Length)
            {
                var opCode = Instructions[ip];
                var operand = Instructions[ip + 1];

                switch (opCode)
                {
                    // The adv instruction (opcode 0) performs division.
                    // The numerator is the value in the A register.
                    // The denominator is found by raising 2 to the power of the instruction's combo operand.
                    // (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.)
                    // The result of the division operation is truncated to an integer and then written to the A register.
                    case 0:
                        A /= 1 << GetComboOperand(operand);
                        ip += 2;
                        break;
                    
                    // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand,
                    // then stores the result in register B.
                    case 1:
                        B ^= operand;
                        ip += 2;
                        break;
                    
                    // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
                    // (thereby keeping only its lowest 3 bits), then writes that value to the B register.
                    case 2:
                        B = GetComboOperand(operand) & 7;
                        ip += 2;
                        break;
                    
                    // The jnz instruction (opcode 3) does nothing if the A register is 0.
                    // However, if the A register is not zero, it jumps by setting the instruction pointer
                    // to the value of its literal operand;
                    // if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
                    case 3:
                        ip = A != 0 ? operand : ip + 2;
                        break;
                    
                    // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
                    // then stores the result in register B.
                    // (For legacy reasons, this instruction reads an operand but ignores it.)
                    case 4:
                        B ^= C;
                        ip += 2;
                        break;
                    
                    // The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
                    // then outputs that value.
                    case 5:
                        outputs.Add(GetComboOperand(operand) & 7);
                        
                        if (MatchInstructions && !Instructions.Take(outputs.Count).SequenceEqual(outputs))
                        {
                            return outputs;
                        }
                        
                        ip += 2;
                        break;
                    
                    // The bdv instruction (opcode 6) works exactly like the adv instruction
                    // except that the result is stored in the B register.
                    // (The numerator is still read from the A register.)
                    case 6:
                        B = A / (1 << GetComboOperand(operand));
                        ip += 2;
                        break;
                    
                    // The cdv instruction (opcode 7) works exactly like the adv instruction
                    // except that the result is stored in the C register.
                    // (The numerator is still read from the A register.)
                    case 7:
                        C = A / (1 << GetComboOperand(operand));
                        ip += 2;
                        break;
                }
            }
            
            return outputs;
        }

        private int GetComboOperand(int opcode)
            => opcode switch
            {
                >= 0 and <= 3 => opcode,
                4 => A,
                5 => B,
                6 => C,
                7 => throw new InvalidOperationException("Reserved opcode"),
                _ => throw new InvalidOperationException("Invalid opcode")
            };
    }

    [GeneratedRegex(@"^Register .: (\d+)$")]
    private static partial Regex RegisterRegex();
    
    [GeneratedRegex("^Program: (.*)$")]
    private static partial Regex ProgramRegex();
}
