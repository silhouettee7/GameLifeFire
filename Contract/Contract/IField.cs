namespace Contract;

/// <summary>
/// интерфейс поля клеток
/// </summary>
public interface IField<out T> where T : ICell
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

    /// <summary>
    /// метод получения поля клеток
    /// </summary>
    /// <returns>поле клеток</returns>
    public T[,] GetField();

    /// <summary>
    /// задает клетку, где будет начнетя пожар
    /// </summary>
    /// <param name="startX">позиция по строке</param>
    /// <param name="startY">позиция по столбцу</param>
    public void SetStartBurningCell(int startX, int startY);
}