using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public interface IMinefield
    {
        List<List<ICell>> Cells { get; }

        ICell GetCellByCoords(int x, int y);

        void Fill(int hight, int length, int mineCount);
    }
}
