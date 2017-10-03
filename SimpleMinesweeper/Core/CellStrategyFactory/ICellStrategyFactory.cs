using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core.CellStrategyFactory
{
    interface ICellStrategyFactory
    {
        ICellStrategy GetStrategy();
    }
}
