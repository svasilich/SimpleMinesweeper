using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class MinedStrategy : AbstractCellStrategy
    {
        public MinedStrategy(ICell cell) : base(cell)
        {
        }

        public override void Action()
        {
            throw new NotImplementedException();
        }
    }
}
