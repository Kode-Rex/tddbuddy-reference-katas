def score(rolls: list[int]) -> int:
    total = 0
    roll_index = 0
    for _frame_index in range(10):
        if rolls[roll_index] + rolls[roll_index + 1] == 10:
            total += 10 + rolls[roll_index + 2]
        else:
            total += rolls[roll_index] + rolls[roll_index + 1]
        roll_index += 2
    return total
