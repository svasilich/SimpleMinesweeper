using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public enum FieldState
    {
        NotStarted,
        InGame,
        GameOver
    }

    public interface IMinefield
    {
        FieldState State { get; }

        List<List<ICell>> Cells { get; }

        ICell GetCellByCoords(int x, int y);

        void Fill(int hight, int length, int mineCount);
    }
}
