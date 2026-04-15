def open_doors(num_doors: int) -> list[int]:
    is_open = [False] * (num_doors + 1)
    for pass_num in range(1, num_doors + 1):
        for door in range(pass_num, num_doors + 1, pass_num):
            is_open[door] = not is_open[door]

    return [door for door in range(1, num_doors + 1) if is_open[door]]
