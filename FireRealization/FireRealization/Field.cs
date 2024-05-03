using Contract;
namespace FireRealization;

public class Field(int sizeX, int sizeY) : IField, IProcessFire<Cell>
{
    private Cell[,]? _field;
    public int SizeX { get; } = sizeX;
    public int SizeY { get; } = sizeY;

    public void Initialize()
    {
        _field = new Cell[SizeX, SizeY];

    }

    public Cell[] UpdateFieldAfterFire()
    {
        if (_field == null)
        {
            throw new NullReferenceException("Поле для пожара не создано");
        }

        return _field;
    }
}