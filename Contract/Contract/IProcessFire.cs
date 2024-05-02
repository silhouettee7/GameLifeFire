namespace Contract;

/// <summary>
/// Интерфейс пожара одного поколения
/// </summary>
/// <typeparam name="T">клетка поля</typeparam>
public interface IProcessFire<out T> where T : ICell
{
    /// <summary>
    /// метод обновляет поле клеток после 1 поколения пожара
    /// </summary>
    /// <returns>возвращает получившееся поле</returns>
    public T[] UpdateFieldAfterFire();
}