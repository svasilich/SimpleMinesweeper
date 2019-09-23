namespace SimpleMinesweeper.Core
{
    public interface ICellFactory
    {
        ICell CreateCell(IMinefield field, int x, int y);
    }
}
