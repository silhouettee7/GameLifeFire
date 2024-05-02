namespace Contract;

/// <summary>
/// интерфейс поля клеток
/// </summary>
public interface IField
{
    /// <summary>
    /// количество столбцов
    /// </summary>
    public int SizeX { get; }
    /// <summary>
    /// количество строк
    /// </summary>
    public int SizeY { get; }
    /// <summary>
    /// метод инициализации поля
    /// </summary>
    public void Initialize();
}