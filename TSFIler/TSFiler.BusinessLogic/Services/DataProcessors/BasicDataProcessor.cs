using System;
using System.Data;
using System.Text;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.DataProcessors
{
    public class BasicDataProcessor : IDataProcessor
    {
        public string ProcessData(string data)
        {
            string input = data;
            string prev;
            do
            {
                prev = input;
                input = EvaluateParentheses(input);
                input = EvaluateSimpleExpressions(input);
            } while (input != prev);

            return input;
        }

        private string EvaluateParentheses(string input)
        {
            while (true)
            {
                int start = -1;
                int end = -1;
                int depth = 0;

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '(')
                    {
                        if (depth == 0) start = i;
                        depth++;
                    }
                    else if (input[i] == ')')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            end = i;
                            break;
                        }
                    }
                }

                if (start == -1 || end == -1)
                {
                    break;
                }

                string inside = input.Substring(start + 1, end - start - 1);
                string computed = CalculateExpression(inside);
                input = input.Substring(0, start) + computed + input.Substring(end + 1);
            }

            return input;
        }

        private string EvaluateSimpleExpressions(string input)
        {
            StringBuilder result = new StringBuilder();
            int index = 0;
            bool changed = false;

            while (index < input.Length)
            {
                if (IsStartOfExpression(input, index, out int exprEnd))
                {
                    string expr = input.Substring(index, exprEnd - index);
                    string computed = CalculateExpression(expr);
                    result.Append(computed);
                    index = exprEnd;
                    changed = true;
                }
                else
                {
                    result.Append(input[index]);
                    index++;
                }
            }

            string newResult = result.ToString();
            if (changed && newResult != input)
            {
                return EvaluateSimpleExpressions(newResult);
            }

            return newResult;
        }

        private bool IsStartOfExpression(string input, int start, out int exprEnd)
        {
            exprEnd = start;
            int i = start;

            if (i < input.Length && input[i] == '-')
            {
                if (i + 1 < input.Length && char.IsDigit(input[i + 1]))
                {
                    i++;
                }
                else
                {
                    return false;
                }
            }

            if (!ParseNumber(input, ref i))
            {
                return false;
            }

            int lastValidPos = i;

            while (true)
            {
                while (i < input.Length && input[i] == ' ')
                    i++;

                if (i >= input.Length)
                    break;

                if (!IsOperator(input, i))
                    break;

                i++;

                while (i < input.Length && input[i] == ' ')
                    i++;

                if (!ParseNumber(input, ref i))
                {
                    i = lastValidPos;
                    break;
                }

                lastValidPos = i;
            }

            if (lastValidPos > start)
            {
                exprEnd = lastValidPos;
                return true;
            }

            return false;
        }

        private bool ParseNumber(string input, ref int i)
        {
            bool hasDigits = false;
            bool hasDot = false;

            while (i < input.Length)
            {
                char c = input[i];
                if (char.IsDigit(c))
                {
                    hasDigits = true;
                    i++;
                }
                else if (c == '.' && !hasDot)
                {
                    hasDot = true;
                    i++;
                }
                else
                {
                    break;
                }
            }

            return hasDigits;
        }

        private bool IsOperator(string input, int pos)
        {
            char c = input[pos];
            return c == '+' || c == '-' || c == '*' || c == '/';
        }

        private string CalculateExpression(string expression)
        {
            try
            {
                var result = new DataTable().Compute(expression, null);

                if (result is double d)
                {
                    if (double.IsInfinity(d) || double.IsNaN(d))
                    {
                        throw new DivideByZeroException("Деление на ноль обнаружено в выражении.");
                    }
                }

                return result.ToString();
            }
            catch (DivideByZeroException)
            {
                throw;
            }
            catch
            {
                return expression;
            }
        }
    }
}