def is_balanced(input: str) -> bool:
    depth = 0
    for c in input:
        if c == "[":
            depth += 1
        elif c == "]":
            depth -= 1
            if depth < 0:
                return False
    return depth == 0
