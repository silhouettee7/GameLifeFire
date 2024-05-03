namespace Contract;

/// <summary>
/// Интерфейс симулирования пожара
/// </summary>
/// <typeparam name="T">Клетка поля</typeparam>
public interface ISimulateFire<T> where T : ICell
{
    /// <summary>
    /// Процесс пожара
    /// </summary>
    /// <param name="cells">входное поле клеток</param>
    /// <returns>результат поля после пожара</returns>
    public ICell[,] SimulateFire(ICell[,] cells);
}