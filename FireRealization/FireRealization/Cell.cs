using Contract;

namespace FireRealization;

public class Cell : ICell
{
    public CellState State { get; set; }
    public CellState NextState { get; set; }
}