using System;
using System.Text.RegularExpressions;

namespace expression_calculator
{
    class Calculator
    {
        string inputExpression;
        public string InputExpression
        {
            get
            {
                return this.inputExpression;
            }
            set
            {
                this.inputExpression = value;
            }
        }
        public Calculator(string inputExpression)
        {
            this.InputExpression = inputExpression;
        }
        public void Cal(string inputExpression)
        {
            string bracketsPattern = @"\(([^)(|]+)\)";
            string absoluteValuePatter = @"\|([^|()]+)\|";
            string valuePattern = @"(-?\d+(?:[.,]\d+)?)";
            string powerBracketsPattern = @"\(([^)(|]+)\)\^(-?\d+(?:[.,]\d+)?)"; //(2+2)^2
            string samePatternTwicePattern = @"\-\-|\+\+";
            string sumNextToSubtractPattern = @"\-\+|\+\-";
            string brackets = Regex.Match(inputExpression, bracketsPattern).ToString();
            string absoluteValue = Regex.Match(inputExpression, absoluteValuePatter).ToString();
            string powerBrackets = Regex.Match(inputExpression, powerBracketsPattern).ToString();
            string currentExpression = string.Empty;
            string currentRes = string.Empty;
            double val = 0;
            double val1 = 0;
            double res = 0;
            CorrentingInputExpression(ref inputExpression);
            if (ValidInput(inputExpression) == false)
            {
                Console.WriteLine("Invalid Expression");
            }
            else
            {
                while (true)
                {
                    if (powerBrackets != "")
                    {
                        while (true)
                        {
                            var value1 = Regex.Match(powerBrackets, valuePattern, RegexOptions.RightToLeft);
                            val1 = double.Parse(value1.Value);
                            brackets = Regex.Match(powerBrackets, bracketsPattern).ToString();
                            currentExpression = brackets;
                            CalculateCurrentExpression(ref currentExpression, valuePattern, ref currentRes, val, val1,res);
                            var value = Regex.Match(currentRes, valuePattern);
                            val = double.Parse(value.Value);
                            currentRes = Math.Pow(val, val1).ToString();
                            Regex repl = new Regex(powerBracketsPattern);
                            inputExpression = repl.Replace(inputExpression, currentRes, 1);
                            if (FinalResCurrentExpression(currentExpression, valuePattern) == true)
                            {
                                brackets = Regex.Match(inputExpression, bracketsPattern).ToString();
                                break;
                            }
                        }
                    }
                    else if (brackets != "" || absoluteValue != "")
                    {
                        if (brackets != "")
                        {
                            currentExpression = brackets;
                            CalculateCurrentExpression(ref currentExpression, valuePattern, ref currentRes, val, val1,res);
                            Regex repl1 = new Regex(bracketsPattern);
                            inputExpression = repl1.Replace(inputExpression, currentRes, 1);
                        }
                        else if (absoluteValue != "")
                        {
                            currentExpression = absoluteValue;
                            CalculateCurrentExpression(ref currentExpression, valuePattern, ref currentRes, val, val1, res);
                            Regex repl1 = new Regex(absoluteValuePatter);
                            currentRes = currentExpression.Trim('|');
                            currentRes = currentRes.TrimStart('-');
                            inputExpression = repl1.Replace(inputExpression, currentRes, 1);
                        }
                    }
                    else
                    {
                        currentExpression = inputExpression;
                        CalculateCurrentExpression(ref currentExpression, valuePattern, ref currentRes, val, val1,res);
                        Console.WriteLine(currentExpression);
                        break;
                    }
                     brackets = Regex.Match(inputExpression, bracketsPattern).ToString();
                     absoluteValue = Regex.Match(inputExpression, absoluteValuePatter).ToString();
                     powerBrackets = Regex.Match(inputExpression, powerBracketsPattern).ToString();
                     inputExpression = Regex.Replace(inputExpression, samePatternTwicePattern, "+");
                    inputExpression = Regex.Replace(inputExpression, sumNextToSubtractPattern, "-");
                }
            }
        }
        public static bool ValidInput(string inputExpression)
        {
            string pattern = @"[ -~][^\( -9,^,|,√]";
            MatchCollection vapidInput = Regex.Matches(inputExpression, pattern);
            string openBracketsParretn = @"\(";
            var openBrackets = Regex.Matches(inputExpression, openBracketsParretn).Count;
            string closseBracketsPattern = @"\)";
            MatchCollection claseBrachets = Regex.Matches(inputExpression, closseBracketsPattern);
            string absoluteValuePattern = @"\|";
            MatchCollection absoluteValue = Regex.Matches(inputExpression, absoluteValuePattern);
            string divineByZeroPattern = @"/0";
            MatchCollection divineByZero = Regex.Matches(inputExpression, divineByZeroPattern);
            if (vapidInput.Count != 0 || openBrackets != claseBrachets.Count || absoluteValue.Count % 2 != 0 || divineByZero.Count != 0)
            {
                return false;
            }
            return true;
        }
        public static void CorrentingInputExpression(ref string inputExpression)
        {
            string correctingBracketsPattern = @"(-?\d+(?:[.,]\d+)?)\(([^)(|]+)\)";     //2(2+2)
            string correctingBracketsPattern1 = @"\(([^)(|]+)\)(\d+(?:[.,]\d+)?)";    //(2+2)2
            string correctingBracketsPattern2 = @"\)\(";                                //(2+2)(2+2)
            string correctingAbsoluteValuePattern = @"(-?\d+(?:[.,]\d+)?)\|([^|()]+)\|";// 2|2+2|
            string correctingAbsoluteValuePattern1 = @"\|([^|()]+)\|(\d+(?:[.,]\d+)?)";        //|2+2|2
            string correctingSqrtPattern = @"(-?\d+(?:[.,]\d+)?)√(\d+(?:[.,]\d+)?)";    //2√2
            string correctingPercentPattern = @"(-?\d+(?:[.,]\d+)?)\*(-?\d+(?:[.,]\d+)?)%|(-?\d+(?:[.,]\d+)?)%\*(-?\d+(?:[.,]\d+)?)";
            MatchCollection corretingBrackets = Regex.Matches(inputExpression, correctingBracketsPattern);
            MatchCollection correctingBrackets1 = Regex.Matches(inputExpression, correctingBracketsPattern1);
            MatchCollection correctingBrackets2 = Regex.Matches(inputExpression, correctingBracketsPattern2);
            MatchCollection correctingAbsoluteValue = Regex.Matches(inputExpression, correctingAbsoluteValuePattern);
            MatchCollection correctingAbsoluteValue1 = Regex.Matches(inputExpression, correctingAbsoluteValuePattern1);
            MatchCollection correctingSqrt = Regex.Matches(inputExpression, correctingSqrtPattern);
            MatchCollection correctingPercent = Regex.Matches(inputExpression, correctingPercentPattern);
            if (corretingBrackets.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingBracketsPattern).ToString();
                    string[] splitCorrectingBrackets = correcting.Split('(');
                    string outputcorretingBrackets = splitCorrectingBrackets[0] + "*(" + splitCorrectingBrackets[1];
                    Regex repl1 = new Regex(correctingBracketsPattern);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingBrackets, 1);
                    corretingBrackets = Regex.Matches(inputExpression, correctingBracketsPattern);
                }
                while (corretingBrackets.Count != 0);
            }
            if (correctingBrackets1.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingBracketsPattern1).ToString();
                    string[] splitCorrectingBrackets = correcting.Split(')');
                    string outputcorretingBrackets = splitCorrectingBrackets[0] + ")*" + splitCorrectingBrackets[1];
                    Regex repl1 = new Regex(correctingBracketsPattern1);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingBrackets, 1);
                    correctingBrackets1 = Regex.Matches(inputExpression, correctingBracketsPattern1);
                }
                while (correctingBrackets1.Count != 0);
            }
            if (correctingBrackets2.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingBracketsPattern2).ToString();
                    string[] splitCorrectingBrackets = correcting.Split(')');
                    string outputcorretingBrackets = splitCorrectingBrackets[0] + ")*" + splitCorrectingBrackets[1];
                    Regex repl1 = new Regex(correctingBracketsPattern2);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingBrackets, 1);
                    correctingBrackets2 = Regex.Matches(inputExpression, correctingBracketsPattern2);
                }
                while (correctingBrackets2.Count != 0);
            }
            if (correctingAbsoluteValue.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingAbsoluteValuePattern).ToString();
                    string[] splitCorrectingAbsoluteValue = correcting.Split('|');
                    string outputcorretingAbsoluteValue = splitCorrectingAbsoluteValue[0] + "*|" + splitCorrectingAbsoluteValue[1] + "|";
                    Regex repl1 = new Regex(correctingAbsoluteValuePattern);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingAbsoluteValue, 1);
                    correctingAbsoluteValue = Regex.Matches(inputExpression, correctingAbsoluteValuePattern);
                }
                while (correctingAbsoluteValue.Count != 0);
            }
            if (correctingAbsoluteValue1.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingAbsoluteValuePattern1).ToString();
                    string[] splitCorrectingAbsoluteValue1 = correcting.Split('|');
                    string outputcorretingAbsoluteValue1 = "|" + splitCorrectingAbsoluteValue1[1] +"|*" + splitCorrectingAbsoluteValue1[2];
                    Regex repl1 = new Regex(correctingAbsoluteValuePattern1);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingAbsoluteValue1, 1);
                    correctingAbsoluteValue1 = Regex.Matches(inputExpression, correctingAbsoluteValuePattern1);
                }
                while (correctingAbsoluteValue1.Count != 0);
            }
            if (correctingSqrt.Count > 0)
            {
                do
                {
                    string correcting = Regex.Match(inputExpression, correctingSqrtPattern).ToString();
                    string[] splitCorrectingSqrt = correcting.Split('√');
                    string outputcorretingSqrt = splitCorrectingSqrt[0] + "*√" + splitCorrectingSqrt[1];
                    Regex repl1 = new Regex(correctingSqrtPattern);
                    inputExpression = repl1.Replace(inputExpression, outputcorretingSqrt, 1);
                    correctingSqrt = Regex.Matches(inputExpression, correctingSqrtPattern);
                }
                while (correctingSqrt.Count != 0);
            }
            if (correctingPercent.Count > 0)
            {
                do
                {
                string correctingCurrentPercent = Regex.Match(inputExpression, correctingPercentPattern).ToString();
                correctingCurrentPercent = correctingCurrentPercent.Replace("%", "/100");
                Regex repl = new(correctingPercentPattern);
                inputExpression = repl.Replace(inputExpression, correctingCurrentPercent, 1);
                correctingPercent = Regex.Matches(inputExpression, correctingPercentPattern);

                }
                while (correctingPercent.Count != 0);
            }
        }
        public static bool FinalResCurrentExpression(string currentExpression, string valuePattern)
        {
            MatchCollection numbers = Regex.Matches(currentExpression, valuePattern);
            if (numbers.Count == 1)
            {
                return true;
            }
            return false;
        }
        public static void GetDigite(string expression, string valuePattern, ref double val, ref double val1)
        {
            var value = Regex.Match(expression, valuePattern);
            var value1 = Regex.Match(expression, valuePattern, RegexOptions.RightToLeft);
            val = double.Parse(value.Value);
            val1 = double.Parse(value1.Value);
        }
        public static void Multiplication(double val, double val1,double res, ref string currentRes, ref string currentExpression)
        {
            res = Math.Round(val * val1, 14);
            currentRes = Math.Round(res, 13).ToString();
            Regex repl = new(@"(-?\d+(?:[.,]\d+)?)\*(-?\d+(?:[.,]\d+)?)");
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void Division(double val, double val1, double res, ref string currentRes, ref string currentExpression)
        {
            Regex repl = new(@"(-?\d+(?:[.,]\d+)?)\/(-?\d+(?:[.,]\d+)?)");
            if (val1 == 0)
            {
                currentExpression = repl.Replace(currentExpression, "Incorect input", 1);
            }
            res = Math.Round(val / val1, 14);
            currentRes= Math.Round(res, 13).ToString();
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void SumSubtract(double val, double val1,double res, ref string currentRes, ref string currentExpression)
        {
            res = Math.Round(val + val1, 14);
            currentRes = Math.Round(res, 13).ToString();
            Regex repl = new(@"(-?\d+(?:[.,]\d+)?)[\+\-](-?\d+(?:[.,]\d+)?)");
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void MultiplicationDivision(string multiplication, string division, ref string currentRes, ref string currentExpression, string multiplicationDivision, string valuePattern, double val, double val1, double res)
        {
            GetDigite(multiplicationDivision, valuePattern, ref val, ref val1);
            if (multiplication != "")
            {
                Multiplication(val, val1, res, ref currentRes, ref currentExpression);
            }
            else if (division != "")
            {
                Division(val, val1,res, ref currentRes, ref currentExpression);
            }
        }
        public static void SumSubtractionPercent(string sumSubtractionPercent, string sumSubtraction, string percent, ref string currentRes, ref string currentExpression, string valuePattern, double val, double val1,double res)
        {
            GetDigite(sumSubtractionPercent, valuePattern, ref val, ref val1);
            if (percent != "")
            {
                Percent(val, val1,res, ref currentRes, ref currentExpression);
            }

            else if (sumSubtraction != "")
            {
                SumSubtract(val, val1,res, ref currentRes, ref currentExpression);
            }
        }
        public static void PowerSqrt(string power, string sqrt, ref string currentRes, ref string currentExpression, string powerSqrt, string valuePattern, double val, double val1,double res)
        {
            GetDigite(powerSqrt, valuePattern, ref val, ref val1);
            if (power != "")
            {
                Power(val, val1,res, ref currentRes, ref currentExpression);
            }
            else if (sqrt != "")
            {
                Sqrt(val1,res ,ref currentRes, ref currentExpression);
            }
        }
        public static void Power(double val, double val1, double res, ref string currentRes, ref string currentExpression)
        {
            if (val < 0)
            {
                val *= -1;
                res = Math.Pow(val, val1) * (-1);
                res = Math.Round(res, 14);
                currentRes = Math.Round(res,13).ToString();
            }
            else
            {
                res= Math.Pow(val, val1);
                res = Math.Round(res, 14);
                currentRes = Math.Round(res, 13).ToString();
            }
            Regex repl = new(@"(-?\d+(?:[.,]\d+)?)\^(-?\d+(?:[.,]\d+)?)");
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void Sqrt(double val1,double res, ref string currentRes, ref string currentExpression)
        {
            res= Math.Sqrt(val1);
            res = Math.Round(res, 14);
            currentRes = Math.Round(res, 13).ToString();
            Regex repl = new(@"√(\d+(?:[.,]\d+)?)");
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void Percent(double val, double val1,double res, ref string currentRes, ref string currentExpression)
        {
            res = (val + val1 / 100 * val);
            res = Math.Round(res, 14);
            currentRes = Math.Round(res, 13).ToString();
            Regex repl = new(@"(-?\d+(?:[.,]\d+)?)[\+\-](-?\d+(?:[.,]\d+)?)%");
            currentExpression = repl.Replace(currentExpression, currentRes, 1);
        }
        public static void Calculation(ref string currentExpression, string valuePattern, ref string currentRes, double val, double val1,double res)
        {
                string powerSqrtPattern = @"(((-?\d+(?:[.,]\d+)?)\^(-?\d+(?:[.,]\d+)?)))|(√(\d+(?:[.,]\d+)?))";
                string powerSqrt = Regex.Match(currentExpression, powerSqrtPattern).ToString();
                string multiplicationDivisionPattern = @"((-?\d+(?:[.,]\d+)?)[\*\/](-?\d+(?:[.,]\d+)?))";
                string multiplicationDivision = Regex.Match(currentExpression, multiplicationDivisionPattern).ToString();
                string sumSubtractionPercentPattern = @"((-?\d+(?:[.,]\d+)?)[\+\-](-?\d+(?:[.,]\d+)?)%?)";
                string sumSubtractionPercent = Regex.Match(currentExpression, sumSubtractionPercentPattern).ToString();
                if (powerSqrt != "")
                {
                    string powerSymbolPattern = @"\^";
                    string sqrtSymbolPattern = @"√";
                    string power = Regex.Match(powerSqrt, powerSymbolPattern).ToString();
                    string sqrt = Regex.Match(powerSqrt, sqrtSymbolPattern).ToString();
                    PowerSqrt(power, sqrt, ref currentRes, ref currentExpression, powerSqrt, valuePattern, val, val1,res);
                }
                else if (multiplicationDivision != "")
                {
                    string multiplicationSymbolPattern = @"\*";
                    string divisionSymbolPattern = @"\/";
                    string multiplication = Regex.Match(multiplicationDivision, multiplicationSymbolPattern).ToString();
                    string division = Regex.Match(multiplicationDivision, divisionSymbolPattern).ToString();
                    MultiplicationDivision(multiplication, division, ref currentRes, ref currentExpression, multiplicationDivision, valuePattern, val, val1, res);
                }
                else if (sumSubtractionPercent != "")
                {
                    string sumSubtractPattern = @"(-?\d+(?:[.,]\d+)?)[\+\-](-?\d+(?:[.,]\d+)?)";
                    string percentPattern = @"((-?\d+(?:[.,]\d+)?)[\+\-](-?\d+(?:[.,]\d+)?)%)";
                    string sumSubtract = Regex.Match(sumSubtractionPercent, sumSubtractPattern).ToString();
                    string percent = Regex.Match(sumSubtractionPercent, percentPattern).ToString();
                    SumSubtractionPercent(sumSubtractionPercent, sumSubtract, percent, ref currentRes, ref currentExpression, valuePattern, val, val1,res);
                }
                else
                {
                currentExpression = "Incorect input";
                }
        }
        public static void CalculateCurrentExpression(ref string currentExpression,string valuePattern,ref string currentRes,double val,double val1,double res)
        {
            while (true)
            {
                if (FinalResCurrentExpression(currentExpression, valuePattern) == true)
                {
                    currentExpression = currentExpression.TrimStart('(');
                    currentRes = currentExpression.TrimEnd(')');
                    break;
                }
                else if (currentExpression == "Incorect input")
                {
                    break;
                }
                Calculation(ref currentExpression, valuePattern, ref currentRes, val, val1,res);
            }
        }
    }
}