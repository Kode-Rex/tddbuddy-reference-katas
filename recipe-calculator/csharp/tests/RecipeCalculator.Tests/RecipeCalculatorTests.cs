using FluentAssertions;
using Xunit;

namespace RecipeCalculator.Tests;

public class RecipeCalculatorTests
{
    [Fact]
    public void Empty_recipe_scales_to_empty_recipe()
    {
        var recipe = new Dictionary<string, double>();

        RecipeCalculator.Scale(recipe, 2).Should().BeEmpty();
    }

    [Fact]
    public void Single_ingredient_doubles_when_factor_is_two()
    {
        var recipe = new Dictionary<string, double> { ["flour"] = 100 };

        RecipeCalculator.Scale(recipe, 2)
            .Should().Equal(new Dictionary<string, double> { ["flour"] = 200 });
    }

    [Fact]
    public void Single_ingredient_halves_when_factor_is_one_half()
    {
        var recipe = new Dictionary<string, double> { ["flour"] = 100 };

        RecipeCalculator.Scale(recipe, 0.5)
            .Should().Equal(new Dictionary<string, double> { ["flour"] = 50 });
    }

    [Fact]
    public void Multiple_ingredients_all_scale_by_the_same_factor()
    {
        var recipe = new Dictionary<string, double>
        {
            ["flour"] = 200,
            ["sugar"] = 100,
            ["butter"] = 50
        };

        RecipeCalculator.Scale(recipe, 3)
            .Should().Equal(new Dictionary<string, double>
            {
                ["flour"] = 600,
                ["sugar"] = 300,
                ["butter"] = 150
            });
    }

    [Fact]
    public void Factor_of_one_returns_identical_quantities()
    {
        var recipe = new Dictionary<string, double>
        {
            ["flour"] = 100,
            ["sugar"] = 50
        };

        RecipeCalculator.Scale(recipe, 1).Should().Equal(recipe);
    }

    [Fact]
    public void Factor_of_zero_zeroes_every_quantity()
    {
        var recipe = new Dictionary<string, double>
        {
            ["flour"] = 100,
            ["sugar"] = 50
        };

        RecipeCalculator.Scale(recipe, 0)
            .Should().Equal(new Dictionary<string, double>
            {
                ["flour"] = 0,
                ["sugar"] = 0
            });
    }

    [Fact]
    public void Fractional_quantities_scale_without_being_rounded()
    {
        var recipe = new Dictionary<string, double> { ["salt"] = 1.5 };

        RecipeCalculator.Scale(recipe, 3)
            .Should().Equal(new Dictionary<string, double> { ["salt"] = 4.5 });
    }

    [Fact]
    public void Negative_factor_is_rejected()
    {
        var recipe = new Dictionary<string, double> { ["flour"] = 100 };

        Action scale = () => RecipeCalculator.Scale(recipe, -1);

        scale.Should().Throw<ArgumentOutOfRangeException>();
    }
}
