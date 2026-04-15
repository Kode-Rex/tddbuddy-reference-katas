using FluentAssertions;
using Xunit;

namespace TicTacToe.Tests;

public class BoardTests
{
    [Fact]
    public void Empty_board_reports_game_in_progress()
    {
        var board = new Board();

        board.Outcome().Should().Be(Outcome.InProgress);
        board.CurrentTurn().Should().Be(Cell.X);
    }

    [Fact]
    public void First_placement_puts_X_on_the_board()
    {
        var board = new Board().Place(0, 0);

        board.CellAt(0, 0).Should().Be(Cell.X);
        board.CurrentTurn().Should().Be(Cell.O);
        board.Outcome().Should().Be(Outcome.InProgress);
    }

    [Fact]
    public void X_wins_by_completing_the_top_row()
    {
        var board = new BoardBuilder()
            .WithXAt(0, 0).WithXAt(0, 1)
            .WithOAt(1, 0).WithOAt(1, 1)
            .Build();

        board.Place(0, 2).Outcome().Should().Be(Outcome.XWins);
    }

    [Fact]
    public void X_wins_by_completing_the_left_column()
    {
        var board = new BoardBuilder()
            .WithXAt(0, 0).WithXAt(1, 0)
            .WithOAt(0, 1).WithOAt(1, 1)
            .Build();

        board.Place(2, 0).Outcome().Should().Be(Outcome.XWins);
    }

    [Fact]
    public void X_wins_on_the_main_diagonal()
    {
        var board = new BoardBuilder()
            .WithXAt(0, 0).WithXAt(1, 1)
            .WithOAt(0, 1).WithOAt(0, 2)
            .Build();

        board.Place(2, 2).Outcome().Should().Be(Outcome.XWins);
    }

    [Fact]
    public void O_wins_on_the_anti_diagonal()
    {
        var board = new BoardBuilder()
            .WithXAt(0, 0).WithXAt(1, 0).WithXAt(2, 1)
            .WithOAt(0, 2).WithOAt(1, 1)
            .Build();

        board.Place(2, 0).Outcome().Should().Be(Outcome.OWins);
    }

    [Fact]
    public void Full_board_with_no_winning_line_is_a_draw()
    {
        var board = new BoardBuilder()
            .WithXAt(0, 0).WithOAt(0, 1).WithXAt(0, 2)
            .WithXAt(1, 0).WithXAt(1, 1).WithOAt(1, 2)
            .WithOAt(2, 0).WithXAt(2, 1).WithOAt(2, 2)
            .Build();

        board.Outcome().Should().Be(Outcome.Draw);
    }

    [Fact]
    public void Placing_on_an_occupied_cell_raises_cell_occupied()
    {
        var board = new BoardBuilder().WithXAt(0, 0).Build();

        var act = () => board.Place(0, 0);

        act.Should().Throw<CellOccupiedException>()
            .WithMessage("cell already occupied");
    }

    [Fact]
    public void Placing_with_a_row_out_of_bounds_raises_out_of_bounds()
    {
        var board = new Board();

        var above = () => board.Place(3, 0);
        var below = () => board.Place(-1, 0);

        above.Should().Throw<OutOfBoundsException>().WithMessage("coordinates out of bounds*");
        below.Should().Throw<OutOfBoundsException>().WithMessage("coordinates out of bounds*");
    }

    [Fact]
    public void Placing_with_a_column_out_of_bounds_raises_out_of_bounds()
    {
        var board = new Board();

        var act = () => board.Place(0, 3);

        act.Should().Throw<OutOfBoundsException>().WithMessage("coordinates out of bounds*");
    }

    [Fact]
    public void Placing_after_a_win_raises_game_over()
    {
        var won = new BoardBuilder()
            .WithXAt(0, 0).WithXAt(0, 1).WithXAt(0, 2)
            .WithOAt(1, 0).WithOAt(1, 1)
            .Build();

        var act = () => won.Place(2, 2);

        act.Should().Throw<GameOverException>().WithMessage("game is already over");
    }

    [Fact]
    public void BoardBuilder_produces_the_board_the_test_literal_describes()
    {
        var board = new BoardBuilder().WithXAt(0, 0).WithOAt(1, 1).Build();

        board.CellAt(0, 0).Should().Be(Cell.X);
        board.CellAt(1, 1).Should().Be(Cell.O);
        board.CellAt(2, 2).Should().Be(Cell.Empty);
        board.Outcome().Should().Be(Outcome.InProgress);
        board.CurrentTurn().Should().Be(Cell.X);

        var oneAhead = new BoardBuilder().WithXAt(0, 0).Build();
        oneAhead.CurrentTurn().Should().Be(Cell.O);
    }
}
