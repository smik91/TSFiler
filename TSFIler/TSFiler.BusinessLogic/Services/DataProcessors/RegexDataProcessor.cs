using System;
using System.Data;
using System.Text.RegularExpressions;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.DataProcessors
{
    public class RegexDataProcessor : IDataProcessor
    {
        private static readonly Regex ParenthesesRegex = new Regex(@"\([^()]+\)", RegexOptions.Compiled);

        private static readonly Regex ExpressionRegex = new Regex(
            @"(?<![\d.])-?\d+(\.\d+)?(\s*[\+\-\*/]\s*-?\d+(\.\d+)?)+",
            RegexOptions.Compiled);

        private static readonly Regex ValidExpressionRegex = new Regex(@"^[0-9\+\-\*/\.\s]+$", RegexOptions.Compiled);

        public string ProcessData(string data)
        {
            string input = data;
            string previousInput;

            do
            {
                previousInput = input;

                input = ParenthesesRegex.Replace(input, match =>
                {
                    var expression = match.Value.Trim('(', ')').Replace(" ", "");

                    if (!IsValidExpression(expression))
                        return match.Value;

                    var result = ComputeExpression(expression);
                    return result?.ToString() ?? match.Value;
                });

                input = ExpressionRegex.Replace(input, match =>
                {
                    var expression = match.Value.Replace(" ", "");

                    if (!IsValidExpression(expression))
                        return match.Value;

                    var result = ComputeExpression(expression);
                    return result?.ToString() ?? match.Value;
                });

            } while (input != previousInput);

            return input;
        }

        private bool IsValidExpression(string expression)
        {
            return ValidExpressionRegex.IsMatch(expression);
        }

        private object? ComputeExpression(string expression)
        {
            var dataTable = new DataTable();

            expression = expression.Replace(",", ".").Replace(" ", "");

            try
            {
                var result = dataTable.Compute(expression, null);

                if (result is double d)
                {
                    if (double.IsInfinity(d) || double.IsNaN(d))
                    {
                        throw new DivideByZeroException("Деление на ноль обнаружено в выражении.");
                    }
                }

                return result;
            }
            catch (DivideByZeroException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }
    }
}