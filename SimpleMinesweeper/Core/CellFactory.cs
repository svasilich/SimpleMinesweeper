namespace SimpleMinesweeper.Core
{
    public class CellFactory : ICellFactory
    {
        public ICell CreateCell(IMinefield field, int x, int y)
        {
            return new Cell(field, x, y);
        }
    }
}
