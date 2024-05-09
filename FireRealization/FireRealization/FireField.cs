using Contract;
namespace FireRealization;

public class FireField(int sizeX, int sizeY) : IField<Cell>, IProcessFire
{
    private Cell[,]? _field;
    private Cell[,]? _tempField;
    private bool _isFireEnded;
    public int SizeX { get; } = sizeX;
    public int SizeY { get; } = sizeY;

    public bool CheckEndFire()
    {
        return _isFireEnded;
    }

    public Cell[,] GetField()
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле не создано");
        }
        
        return _field;
    }

    public void Initialize()
    {
        _field = new Cell[SizeX, SizeY];
        _tempField = new Cell[SizeX, SizeY];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var randomState = (CellState)new Random().Next(0, 3);
                _field[x,y] = new Cell(randomState);
                _tempField[x, y] = new Cell(randomState);
            }
        }
    }

    public void SetStartBurningCell(int startX, int startY)
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }
        if (startX  < 0 || startX  >= SizeX || startY  < 0 || startY >= SizeY)
        {
            throw new ArgumentException("Некорректные стартовые значения");
        }

        _field[startX, startY].State = CellState.Burning;
        _tempField![startX, startY].State = CellState.Burning;
    }

    public void UpdateFieldAfterFire()
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }

        _isFireEnded = true;
        Parallel.For(0, SizeX, x =>
        {
            Parallel.For(0, SizeY, y =>
            {
                UpdateCellState(x, y);
            });
        });

        
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                if (_tempField![x, y].State == CellState.Burning)
                {
                    _isFireEnded = false;
                }
                _field[x, y].State = _tempField![x,y].State;
                _field[x, y].PreviousState = _tempField![x, y].PreviousState;
            }
        }
    }
    public void UpdateFieldWithoutParallel()
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }

        _isFireEnded = true;
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                UpdateCellState(x,y);
            }
        }

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                if (_tempField![x, y].State == CellState.Burning)
                {
                    _isFireEnded = false;
                }
                _field[x, y].State = _tempField![x, y].State;
                _field[x, y].PreviousState = _tempField![x, y].PreviousState;
            }
        }
    }
    private void UpdateCellState(int x, int y)
    {
        var cell = _field![x, y];
        var tempCell = _tempField![x, y];
        if (cell.State == CellState.Empty)
        {
            return;
        }
        int countDiagonal = 0;
        bool isNeighbourFire = false;
        bool flag = false;
        if (cell.State == CellState.Bush || cell.State == CellState.Tree)
        {
            for (int i = Math.Max(x - 1, 0); i < Math.Min(x + 1, SizeX - 1)+1; i++)
            {
                for (int j = Math.Max(y - 1, 0); j < Math.Min(y + 1, SizeY - 1)+1; j++)
                {
                    if (i == x && j == y) continue;
                    if (Math.Abs(x - i) == 1 && Math.Abs(y - j) == 1 && _field[i, j].State == CellState.Burning)
                    {
                        countDiagonal++;
                    }
                    else if (_field[i, j].State == CellState.Burning)
                    {
                        isNeighbourFire = true;
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }
            if (isNeighbourFire || countDiagonal >= 3)
            {
                if (cell.State == CellState.Tree)
                {
                    tempCell.State = CellState.Burning;
                    tempCell.PreviousState = CellState.Tree;
                }
                else if (cell.State == CellState.Bush)
                {
                    tempCell.State = CellState.Burning;
                    tempCell.PreviousState = CellState.Bush;
                }
            }
        }
        else if (cell.State == CellState.Burning)
        {
            if (cell.PreviousState == CellState.Tree)
            {
                tempCell.PreviousState = CellState.Burning;
            }
            else
            {
                tempCell.State = CellState.Empty;
            }
        }
    }
}