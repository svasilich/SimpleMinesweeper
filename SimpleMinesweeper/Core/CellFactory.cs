using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
