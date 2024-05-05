using Contract;

namespace FireRealization;

public class Cell(CellState state) : ICell
{
    public CellState State { get; set; } = state;
    public CellState NextState { get; set; } = state;
}