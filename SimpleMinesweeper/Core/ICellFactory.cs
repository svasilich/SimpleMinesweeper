using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public interface ICellFactory
    {
        ICell CreateCell(IMinefield field, int x, int y);
    }
}
