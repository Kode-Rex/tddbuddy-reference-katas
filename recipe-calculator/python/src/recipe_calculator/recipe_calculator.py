def scale(recipe: dict[str, float], factor: float) -> dict[str, float]:
    if factor < 0:
        raise ValueError("Scale factor must be non-negative.")
    return {ingredient: quantity * factor for ingredient, quantity in recipe.items()}
