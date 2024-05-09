namespace Contract;

/// <summary>
/// Интерфейс пожара одного поколения
/// </summary>
/// <typeparam name="T">клетка поля</typeparam>
public interface IProcessFire
{
    /// <summary>
    /// метод обновляет поле клеток после 1 поколения пожара
    /// </summary>
    public void UpdateFieldAfterFire();

    /// <summary>
    /// метод проверяет закончился ли пожар
    /// </summary>
    /// <returns>true - если закончился, false - не закончился</returns>
    public bool CheckEndFire();
}