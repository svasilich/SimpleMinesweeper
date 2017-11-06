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

        public void Fill(int hight, int length, int mineCount)
        {
            CheckFillParameters(hight, length, mineCount);

            cells = new List<List<ICell>>(hight);
            for (int y = 0; y < hight; ++y)
            {
                List<ICell> row = new List<ICell>(length);
                for (int x = 0; x < length; ++x)
                    row.Add(new Cell(x, y));
                cells.Add(row);
            }

            MineAField(hight, length, mineCount);
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
            Random random = new Random((int) DateTime.Now.Ticks);
            for (int i = 0; i < mineCount; ++i)
            {
                while (true)
                {
                    int minePosX = random.Next(length);
                    int minePosY = random.Next(higth);

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
            throw new NotImplementedException();
        }
    }
}
