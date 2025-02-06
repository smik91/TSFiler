using System;
using System.Text.RegularExpressions;
using TSFiler.BusinessLogic.Services.Interfaces;
using NCalc;

namespace TSFiler.BusinessLogic.Services.DataProcessors
{
    public class LibDataProcessor : IDataProcessor
    {
        public string ProcessData(string data)
        {
            string input = data, prev;
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
            var pattern = @"\(([^()]+)\)";
            while (true)
            {
                var match = Regex.Match(input, pattern);
                if (!match.Success)
                {
                    break;
                }

                var inside = match.Groups[1].Value;
                var computedInside = EvaluateSimpleExpressions(inside);
                var computed = EvaluateWithNCalc(computedInside);

                input = input.Substring(0, match.Index) + computed + input.Substring(match.Index + match.Length);
            }
            return input;
        }

        private string EvaluateSimpleExpressions(string input)
        {

            var pattern = @"(?<![a-zA-Z\(])([+-]?\d+(?:\.\d+)?(?:\s*[+\-*/]\s*[+-]?\d+(?:\.\d+)?)*)(?![a-zA-Z\)])";
            return Regex.Replace(input, pattern, m => EvaluateWithNCalc(m.Value));
        }

        private string EvaluateWithNCalc(string expr)
        {
            try
            {
                var e = new Expression(expr);
                var r = e.Evaluate();
                if (r is double d && (double.IsInfinity(d) || double.IsNaN(d)))
                {
                    throw new DivideByZeroException("Деление на ноль обнаружено в выражении.");
                }
                return r.ToString();
            }
            catch
            {
                return expr;
            }
        }
    }
}
