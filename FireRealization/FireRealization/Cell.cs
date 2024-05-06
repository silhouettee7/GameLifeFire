using Contract;

namespace FireRealization;

public class Cell(CellState state) : ICell
{
    public CellState PreviousState { get; set; } = state;
    public CellState State { get; set; } = state;
}