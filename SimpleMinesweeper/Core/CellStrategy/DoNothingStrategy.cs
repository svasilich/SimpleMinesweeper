using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    class DoNothingStrategy : AbstractCellStrategy
    {
        public DoNothingStrategy(ICell cell) : base(cell)
        {
        }

        public override void Action()
        {
            // Здесь ничего не делаем (Default object).
        }
    }
}
