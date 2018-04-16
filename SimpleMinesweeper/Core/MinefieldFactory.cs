using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public class MinefieldFactory : IMinefieldFactory
    {
        private ICellFactory cellFactory;
        private IMinePositionsGenerator minePositionsGenerator;

        public MinefieldFactory()
        {
            cellFactory = new CellFactory();
            minePositionsGenerator = new RandomMinePositionGenerator();
        }

        public IMinefield Create()
        {
            return new Minefield(cellFactory, minePositionsGenerator);
        }
    }
}
