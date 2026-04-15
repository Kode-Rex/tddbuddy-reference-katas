def next_term(term: str) -> str:
    output: list[str] = []
    i = 0
    while i < len(term):
        digit = term[i]
        run_length = 1
        while i + run_length < len(term) and term[i + run_length] == digit:
            run_length += 1
        output.append(f"{run_length}{digit}")
        i += run_length
    return "".join(output)


def look_and_say(seed: str, iterations: int) -> str:
    if iterations < 0:
        raise ValueError("iterations must be non-negative")
    term = seed
    for _ in range(iterations):
        term = next_term(term)
    return term
