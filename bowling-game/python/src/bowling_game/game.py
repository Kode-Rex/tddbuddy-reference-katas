def score(rolls: list[int]) -> int:
    total = 0
    i = 0
    while i < len(rolls) - 1:
        if rolls[i] + rolls[i + 1] == 10:
            total += 10 + rolls[i + 2]
        else:
            total += rolls[i] + rolls[i + 1]
        i += 2
    return total
