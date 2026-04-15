from collections.abc import Sequence


def make_change(amount: int, denominations: Sequence[int]) -> list[int]:
    coins: list[int] = []
    remaining = amount
    for coin in denominations:
        while remaining >= coin:
            coins.append(coin)
            remaining -= coin
    return coins
