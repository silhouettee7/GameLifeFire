using Contract;
namespace FireRealization;

public class FireField(int sizeX, int sizeY) : IField, IProcessFire<Cell>
{
    private Cell[,]? _field;
    private bool isInitialized = false;
    public int SizeX { get; } = sizeX;
    public int SizeY { get; } = sizeY;

    public void Initialize()
    {
        if (isInitialized)
        {
            throw new FieldInitializedAlreadyException("Поле уже создано");
        }

        _field = new Cell[SizeX, SizeY];
        
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var state = (CellState)new Random().Next(0, 3);
                _field[x,y] = new Cell(state);
            }
        }
        isInitialized = true;
        
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Console.Write($"{(int)_field[i, j].State}  ");
            }
            Console.WriteLine();
        }
    }

    public void SetStartBurningCell(int startX, int startY)
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }
        if (startX - 1 < 0 || startX - 1 >= SizeX || startY - 1 < 0 || startY - 1 >= SizeY)
        {
            throw new ArgumentException("Некорректные стартовые значения");
        }
        
        _field[startX-1, startY-1].State = CellState.Burning;
    }

    public Cell[,] UpdateFieldAfterFire()
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }

        /*Parallel.For(0, SizeX, x =>
        {
            Parallel.For(0, SizeY, y =>
            {
                UpdateCellState(x, y);
            });
        });*/
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                UpdateCellState(x, y);
            }
        }
        
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                _field[x,y].State = _field[x,y].NextState;
            }
        }

        return _field;
    }

    private void UpdateCellState(int x, int y)
    {
        var cell = _field![x, y];
        
        if (cell.State == CellState.Empty)
        {
            return;
        }
        int countDiagonal = 0;
        bool isNeighbourFire = false;
        
        if (cell.State == CellState.Bush || cell.State == CellState.Tree)
        {
            for (int i = Math.Max(x - 1, 0); i < Math.Min(x + 1, SizeX - 1)+1; i++)
            {
                for (int j = Math.Max(y - 1, 0); j < Math.Min(y + 1, SizeY - 1)+1; j++)
                {
                    if (i == j) continue;
                    if (Math.Abs(x - i) == 1 && Math.Abs(y - j) == 1 && _field[i, j].State == CellState.Burning)
                    {
                        countDiagonal++;
                    }
                    else if (_field[i, j].State == CellState.Burning)
                    {
                        isNeighbourFire = true;
                        break;
                    }
                }
            }
            if (isNeighbourFire || countDiagonal >= 3)
            {
                if (cell.State == CellState.Tree)
                {
                    cell.NextState = CellState.Burning;
                }
                else if (cell.State == CellState.Bush)
                {
                    cell.NextState = CellState.Burning;
                }
            }
        }
        else if (cell.State == CellState.Burning)
        {
            cell.NextState = CellState.Empty;
        }
    }
}