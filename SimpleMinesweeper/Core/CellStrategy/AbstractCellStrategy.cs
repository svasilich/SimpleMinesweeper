using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    abstract class AbstractCellStrategy : ICellStrategy
    {
        public AbstractCellStrategy(ICell cell)
        {
            Cell = cell;
        }

        public ICell Cell { get; private set; }

        public abstract void Action();
    }
}
