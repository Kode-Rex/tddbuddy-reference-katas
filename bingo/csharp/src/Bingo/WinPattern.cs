namespace Bingo;

public enum WinPatternKind
{
    None,
    Row,
    Column,
    DiagonalMain,
    DiagonalAnti,
}

public readonly record struct WinPattern(WinPatternKind Kind, int Index)
{
    public static readonly WinPattern None = new(WinPatternKind.None, -1);
    public static readonly WinPattern DiagonalMain = new(WinPatternKind.DiagonalMain, -1);
    public static readonly WinPattern DiagonalAnti = new(WinPatternKind.DiagonalAnti, -1);
    public static WinPattern Row(int index) => new(WinPatternKind.Row, index);
    public static WinPattern Column(int index) => new(WinPatternKind.Column, index);
}
