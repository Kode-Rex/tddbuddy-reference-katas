import pytest

from recipe_calculator import scale


def test_empty_recipe_scales_to_empty_recipe():
    assert scale({}, 2) == {}


def test_single_ingredient_doubles_when_factor_is_two():
    assert scale({"flour": 100}, 2) == {"flour": 200}


def test_single_ingredient_halves_when_factor_is_one_half():
    assert scale({"flour": 100}, 0.5) == {"flour": 50}


def test_multiple_ingredients_all_scale_by_the_same_factor():
    assert scale({"flour": 200, "sugar": 100, "butter": 50}, 3) == {
        "flour": 600,
        "sugar": 300,
        "butter": 150,
    }


def test_factor_of_one_returns_identical_quantities():
    assert scale({"flour": 100, "sugar": 50}, 1) == {"flour": 100, "sugar": 50}


def test_factor_of_zero_zeroes_every_quantity():
    assert scale({"flour": 100, "sugar": 50}, 0) == {"flour": 0, "sugar": 0}


def test_fractional_quantities_scale_without_being_rounded():
    assert scale({"salt": 1.5}, 3) == {"salt": 4.5}


def test_negative_factor_is_rejected():
    with pytest.raises(ValueError):
        scale({"flour": 100}, -1)
