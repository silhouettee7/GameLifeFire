namespace Contract
{
    /// <summary>
    /// интерфейс клетки поля
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Свойство о состоянии клетки
        /// </summary>
        public CellState State { get; set; }
    }
}
