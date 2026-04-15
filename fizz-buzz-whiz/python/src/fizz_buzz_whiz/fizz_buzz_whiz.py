def say(n: int) -> str:
    divisible_by_three = n % 3 == 0
    divisible_by_five = n % 5 == 0

    if divisible_by_three and divisible_by_five:
        return "FizzBuzz"
    if divisible_by_three:
        return "Fizz"
    if divisible_by_five:
        return "Buzz"
    return str(n)
