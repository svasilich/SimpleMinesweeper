using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimpleMinesweeper.Core
{
    public class Minefield : IMinefield
    {
        public List<List<ICell>> Cells { get; private set; }
        private int hight;
        private int length;
        private int mineCount;

        private FieldState state;
        public FieldState State
        {
            get { return state; }
            private set
            {
                state = value;
                OnStateChanged?.Invoke(this, new EventArgs());
            }
        }

        private readonly ICellFactory cellFactory;
        private readonly IMinePositionsGenerator minePositionsGenerator;

        public event EventHandler OnStateChanged;
        public event EventHandler OnFilled;

        public Minefield(ICellFactory cellFactory, IMinePositionsGenerator minePositionsGenerator)
        {
            this.cellFactory = cellFactory;
            this.minePositionsGenerator = minePositionsGenerator;
            State = FieldState.NotStarted;
        }

        private IEnumerable<ICell> GetCellNearby(ICell cell)
        {            
            int topLeftX = cell.CoordX - 1;
            topLeftX = topLeftX < 0 ? 0 : topLeftX;
            int topLeftY = cell.CoordY - 1;
            topLeftY = topLeftY < 0 ? 0 : topLeftY;

            int bottomRihtX = cell.CoordX + 1;
            bottomRihtX = bottomRihtX == length ? length - 1 : bottomRihtX;
            int bottomRightY = cell.CoordY + 1;
            bottomRightY = bottomRightY == hight ? hight - 1 : bottomRightY;

            List<ICell> nearby = new List<ICell>();
            for (int x = topLeftX; x <= bottomRihtX; ++x)
                for (int y = topLeftY; y <= bottomRightY; ++y)
                {
                    if (x == cell.CoordX && y == cell.CoordY)
                        continue;

                    nearby.Add(Cells[y][x]);
                };

            return nearby;
        }

        public int GetCellMineNearbyCount(ICell cell)
        {
            IEnumerable<ICell> nearby = GetCellNearby(cell);
            return nearby.Where(c => c.Mined).Count();
        }

        public void Fill(int fieldHight, int fieldLength, int mineCount)
        {
            CheckFillParameters(fieldHight, fieldLength, mineCount);
            hight = fieldHight;
            length = fieldLength;
            this.mineCount = mineCount;

            Cells = new List<List<ICell>>(hight);
            for (int y = 0; y < hight; ++y)
            {
                List<ICell> row = new List<ICell>(length);
                for (int x = 0; x < length; ++x)
                {
                    ICell cell = cellFactory.CreateCell(this, x, y);
                    cell.OnStateChanged += Cell_OnStateChanged;
                    row.Add(cell);
                }
                Cells.Add(row);
            }

            MineAField();

            OnFilled?.Invoke(this, EventArgs.Empty);
        }

        private void Cell_OnStateChanged(object sender, CellChangeStateEventArgs e)
        {
            ICell cell = (ICell)sender;
            switch (cell.State)
            {
                case CellState.Opened:
                    if (State == FieldState.NotStarted)
                        State = FieldState.InGame;
                    break;

                case CellState.Explosion:
                    if (State == FieldState.NotStarted)
                    {
                        RemoveMineToRendomPosition(cell);
                        cell.Open();
                    }
                    else
                        GameOver();

                    break;
            }
        }

        private void RemoveMineToRendomPosition(ICell from)
        {
            from.Mined = false;
            AddMainToRandomFieldsPosition();
            from.State = CellState.NotOpened;
        }

        protected virtual void Cell_OnSetFlag(object sender, EventArgs e)
        {
            ICell flaggedCell = (ICell)sender;

            switch (flaggedCell.State)
            {
                case CellState.NotOpened:
                    flaggedCell.State = CellState.Flagged;
                    return;

                case CellState.Flagged:
                    flaggedCell.State = CellState.NotOpened;
                    return;
            }
        }

        protected virtual void Cell_OnOpen(object sender, EventArgs e)
        {
            if (State == FieldState.NotStarted)
                State = FieldState.InGame;

            ICell openedCell = (ICell)sender;

            if (openedCell.State != CellState.NotOpened)
                return;

            if (openedCell.Mined)
            {
                if (State == FieldState.GameOver)
                {
                    openedCell.State = CellState.NoFindedMine;
                }
                else
                {
                    openedCell.State = CellState.Explosion;
                    GameOver();
                }
                return;
            }

            openedCell.State = CellState.Opened;
            int minesNearby = openedCell.MinesNearby;
            if (minesNearby == 0)
            {
                IEnumerable<ICell> cellsNearby = GetCellNearby(openedCell);
                foreach (var cell in cellsNearby)
                {
                    if (cell.State == CellState.NotOpened)
                        cell.Open();
                }
                return;
            }

            openedCell.State = CellState.Opened;
        }

        private void GameOver()
        {
            State = FieldState.GameOver;

            foreach (var row in Cells)
                foreach (var cell in row)
                {
                    if (cell.State == CellState.NotOpened)
                    {
                        if (cell.Mined)
                            cell.State = CellState.NoFindedMine;
                        else
                            cell.State = CellState.Opened;
                    }

                    if (cell.State == CellState.Flagged && !cell.Mined)
                        cell.State = CellState.WrongFlag;
                }
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

        private void MineAField()
        {
            for (int i = 0; i < mineCount; ++i)
                AddMainToRandomFieldsPosition();
        }

        private void AddMainToRandomFieldsPosition()
        {
            while (true)
            {
                int minePosX = minePositionsGenerator.Next(length);
                int minePosY = minePositionsGenerator.Next(hight);

                ICell cell = Cells[minePosY][minePosX];
                if (!cell.Mined)
                {
                    cell.Mined = true;
                    break;
                }
            }
        }

        public ICell GetCellByCoords(int x, int y)
        {
            return Cells[y][x];
        } 
    }
}
