using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public class Minefield : IMinefield
    {
        private List<List<ICell>> cells;
        public List<List<ICell>> Cells { get { return cells; } }

        private IMinePositionsGenerator minePositionsGenerator;

        public Minefield(IMinePositionsGenerator minePositionsGenerator)
        {
            this.minePositionsGenerator = minePositionsGenerator;
        }

        public void Fill(int hight, int length, int mineCount)
        {
            CheckFillParameters(hight, length, mineCount);

            cells = new List<List<ICell>>(hight);
            for (int y = 0; y < hight; ++y)
            {
                List<ICell> row = new List<ICell>(length);
                for (int x = 0; x < length; ++x)
                {
                    ICell cell = new Cell(x, y);
                    cell.OnOpen += Cell_OnOpen;
                    cell.OnSetFlag += Cell_OnSetFlag;
                    row.Add(cell);
                }
                cells.Add(row);
            }

            MineAField(hight, length, mineCount);
        }

        protected virtual void Cell_OnSetFlag(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected virtual void Cell_OnOpen(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CheckFillParameters(int hight, int length, int mineCount)
        {
            if (hight == 0)
                throw new ArgumentException("Высота должна быть ненулевой.");

            if (length == 0)
                throw new ArgumentException("Ширина должна быть ненулевой.");

            if (mineCount == 0)
                throw new ArgumentException("Количество мин должно быть больше нуля.");

            if (hight * length <= mineCount)
                throw new ArgumentException("Слишком много мин.");
        }

        private void MineAField(int higth, int length, int mineCount)
        {
            for (int i = 0; i < mineCount; ++i)
            {
                while (true)
                {
                    int minePosX = minePositionsGenerator.Next(length);
                    int minePosY = minePositionsGenerator.Next(higth);

                    ICell cell = cells[minePosY][minePosX];
                    if (!cell.Mined)
                    {
                        cell.Mined = true;
                        break;
                    }
                }
            }
        }

        public ICell GetCellByCoords(int x, int y)
        {
            return cells[y][x];
        }
    }
}
