namespace StringCalculator;

public static class Calculator
{
    public static int Add(string numbers)
    {
        if (numbers == "") return 0;
        var sum = 0;
        foreach (var token in numbers.Split(','))
        {
            sum += int.Parse(token);
        }
        return sum;
    }
}
