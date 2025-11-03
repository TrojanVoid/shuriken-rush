using System;

namespace Com.ShurikenRush.System.Operation
{
    public enum OperationType { Add, Subtract, Multiply, Divide }

    [Serializable]
    public struct OperationSpec
    {
        public OperationType operationType;
        public int operand; // integer gates for HC runner; divide/multiply are integers
    }

    public static class OperationService
    {
        public static int Apply(int current, in OperationSpec op, int min = 0, int max = int.MaxValue)
        {
            long result = current;
            switch (op.operationType)
            {
                case OperationType.Add:      result = current + op.operand; break;
                case OperationType.Subtract: result = current - op.operand; break;
                case OperationType.Multiply: result = current * (long)op.operand; break;
                case OperationType.Divide:
                    if (op.operand <= 0) return current; // invalid; ignore
                    result = current / op.operand; // integer division
                    break;
            }
            if (result < min) result = min;
            if (result > max) result = max;
            return (int)result;
        }

        public static bool IsPositive(in OperationSpec op)
        {
            switch (op.operationType)
            {
                case OperationType.Add:      return op.operand >= 0;
                case OperationType.Subtract: return op.operand <= 0;
                case OperationType.Multiply: return op.operand >= 1;
                case OperationType.Divide:   return op.operand is <= 1 and > 0;
                default:                     return true;
            }
        }

        public static string ToDisplay(in OperationSpec op)
        {
            // Use proper symbols: ×, ÷, and Unicode minus (−)
            return op.operationType switch
            {
                OperationType.Add      => $"+{op.operand}",
                OperationType.Subtract => $"−{op.operand}",
                OperationType.Multiply => $"×{op.operand}",
                OperationType.Divide   => $"÷{op.operand}",
                _ => "?"
            };
        }
    }
}