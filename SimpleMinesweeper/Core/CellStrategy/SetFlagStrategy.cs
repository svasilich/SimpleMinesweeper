using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class SetFlagStrategy : AbstractCellStrategy
    {
        public SetFlagStrategy(ICell cell) : base(cell)
        {
        }

        public override void Action()
        {
            throw new NotImplementedException();
        }
    }
}
