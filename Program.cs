using System;

namespace expression_calculator
{
    class Program
    {
        static void Main()
       {
            Console.WriteLine("Enter the expression");
            Console.WriteLine("For \"exit\" type \"end\"");
            string inputExpression = Console.ReadLine();
            inputExpression = inputExpression.ToLower();
            while (inputExpression != "end")
            {
                Calculator calculator = new Calculator(inputExpression);
                calculator.Cal(inputExpression);
                Console.WriteLine("Enter the expression");
                Console.WriteLine("For \"exit\" type \"end\"");
                inputExpression = Console.ReadLine();
            }
            
        }
    }
}
