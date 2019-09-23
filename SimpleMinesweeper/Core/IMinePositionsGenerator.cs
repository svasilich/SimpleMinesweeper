namespace SimpleMinesweeper.Core
{
    public interface IMinePositionsGenerator
    {
        int Next(int max);
    }
}