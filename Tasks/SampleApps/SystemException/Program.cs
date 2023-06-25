using System;
using A=System.Console;

class Program
{
    static void Main()
    {
        try
        {
            int[] numbers = { 1, 2, 3 };
            int index = 4;

            int result = numbers[index]; // Throws IndexOutOfRangeException
            A.WriteLine("Result: " + result);
        }
        catch (IndexOutOfRangeException exception)
        {
            A.WriteLine("Error: Index is out of range.");
            // Handles the IndexOutOfRangeException
        }
        catch (DivideByZeroException exception)
        {
            A.WriteLine("Error: Cannot divide by zero.");
            // Handles the DivideByZeroException
        }
        catch (InvalidOperationException exception)
        {
            A.WriteLine("Error: Invalid operation.");
            // Handles the InvalidOperationException
        }
        catch (Exception exception)
        {
            A.WriteLine("An error occurred: " + exception.Message);
            // Handles other exceptions
        }
    }
}
