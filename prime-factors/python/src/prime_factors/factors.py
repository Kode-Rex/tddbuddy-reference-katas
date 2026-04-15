def generate(n: int) -> list[int]:
    factors: list[int] = []
    while n % 2 == 0:
        factors.append(2)
        n //= 2
    if n > 1:
        factors.append(n)
    return factors
