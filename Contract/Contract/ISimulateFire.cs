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
    public T[,] SimulateFire(T[,] cells);

    /// <summary>
    /// проверяет закончился ли пожар
    /// </summary>
    /// <param name="cells">поле клеток, в котором проверяется наличие пожара</param>
    /// <returns>true - если закончился, false - не закончился</returns>
    public bool CheckEndFire(T[,] cells);
}