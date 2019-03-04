using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core.GameSettings;

namespace SimpleMinesweeper.Core
{
    public class Minefield : IMinefield
    {
        #region Fields
        private int notMinedCellsCount;
        private int openedCellsCount;
        private FieldState state;
        private int flagsCount;

        private readonly ICellFactory cellFactory;
        private readonly IMinePositionsGenerator minePositionsGenerator;
        #endregion

        #region Properties
        public List<List<ICell>> Cells { get; private set; }

        public virtual FieldState State
        {
            get { return state; }
            private set
            {
                state = value;
                OnStateChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool CellsStateCanBeChanged
        {
            get
            {
                return State == FieldState.NotStarted || State == FieldState.InGame;
            }
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public int MinesCount { get; private set; }
        public int FlagsCount
        {
            get { return flagsCount; }
            private set
            {
                flagsCount = value;
                OnFlagsCountChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Constructor
        public Minefield(ICellFactory cellFactory, IMinePositionsGenerator minePositionsGenerator)
        {
            this.cellFactory = cellFactory;
            this.minePositionsGenerator = minePositionsGenerator;
            State = FieldState.NotStarted;
        }
        #endregion

        #region Events
        public event EventHandler OnStateChanged;
        public event EventHandler OnFilled;
        public event EventHandler OnFlagsCountChanged;
        #endregion

        #region Event handlers

        private void Cell_OnStateChanged(object sender, CellChangeStateEventArgs e)
        {
            if (!CellsStateCanBeChanged)
                return;

            ICell cell = (ICell)sender;
            switch (cell.State)
            {
                case CellState.Opened:
                    if (State == FieldState.NotStarted)
                        State = FieldState.InGame;

                    // Все ли незаминированные клетки открыты?
                    ++openedCellsCount;
                    if (openedCellsCount == notMinedCellsCount)
                        State = FieldState.Win;

                    // Проверить окружение.
                    var nearby = GetCellNearby(cell);
                    if (!IsMineExists(nearby))
                        foreach (var c in nearby)
                            c.Open();

                    break;

                case CellState.Explosion:
                    if (State == FieldState.NotStarted)
                    {
                        MoveMineToRendomPosition(cell);
                        cell.Open();
                    }
                    else
                        GameOver();

                    break;

                case CellState.Flagged:
                    if (State == FieldState.NotStarted)
                        State = FieldState.InGame;

                    if (e.OldState != CellState.Flagged)
                        ++FlagsCount;

                    break;

                case CellState.NotOpened:
                    if (e.OldState == CellState.Flagged)
                        --FlagsCount;
                    break;
            }
        }

        private void GameOver()
        {
            State = FieldState.GameOver;

            foreach (var row in Cells)
                foreach (var cell in row)
                {
                    cell.OnStateChanged -= Cell_OnStateChanged;

                    if (cell.State == CellState.NotOpened)
                    {
                        if (cell.Mined)
                            cell.State = CellState.NoFindedMine;
                        else
                            cell.State = CellState.Opened;
                    }

                    if (cell.State == CellState.Flagged && !cell.Mined)
                        cell.State = CellState.WrongFlag;

                    cell.OnStateChanged += Cell_OnStateChanged;
                }
        }

        #endregion

        #region Settings
        public void SetGameSettings(SettingsItem settings)
        {
            Height = settings.Height;
            Width = settings.Width;
            MinesCount = settings.MineCount;
        }
        #endregion

        #region Filling

        public virtual void Fill()
        {
            if (!SettingsHelper.CheckValidity(Height, Width, MinesCount, out string reason))
                throw new Exception(reason);            

            FlagsCount = 0;
            notMinedCellsCount = Height * Width - MinesCount;
            openedCellsCount = 0;

            Cells = new List<List<ICell>>(Height);
            for (int y = 0; y < Height; ++y)
            {
                List<ICell> row = new List<ICell>(Width);
                for (int x = 0; x < Width; ++x)
                {
                    ICell cell = cellFactory.CreateCell(this, x, y);
                    cell.OnStateChanged += Cell_OnStateChanged;
                    row.Add(cell);
                }
                Cells.Add(row);
            }

            MineAField();

            OnFilled?.Invoke(this, EventArgs.Empty);
            State = FieldState.NotStarted;
        }

        private void MineAField()
        {
            for (int i = 0; i < MinesCount; ++i)
                AddMainToRandomFieldsPosition();
        }

        private void MoveMineToRendomPosition(ICell from)
        {
            from.Mined = false;
            AddMainToRandomFieldsPosition();
            from.State = CellState.NotOpened;
        }

        private void AddMainToRandomFieldsPosition()
        {
            while (true)
            {
                int minePosX = minePositionsGenerator.Next(Width);
                int minePosY = minePositionsGenerator.Next(Height);

                ICell cell = Cells[minePosY][minePosX];
                if (!cell.Mined)
                {
                    cell.Mined = true;
                    break;
                }
            }
        }

        #endregion

        #region Cells and mines
        public ICell GetCellByCoords(int x, int y)
        {
            return Cells[y][x];
        }

        public int GetCellMineNearbyCount(ICell cell)
        {
            IEnumerable<ICell> nearby = GetCellNearby(cell);
            return nearby.Where(c => c.Mined).Count();
        }

        private IEnumerable<ICell> GetCellNearby(ICell cell)
        {
            int topLeftX = cell.CoordX - 1;
            topLeftX = topLeftX < 0 ? 0 : topLeftX;
            int topLeftY = cell.CoordY - 1;
            topLeftY = topLeftY < 0 ? 0 : topLeftY;

            int bottomRihtX = cell.CoordX + 1;
            bottomRihtX = bottomRihtX == Width ? Width - 1 : bottomRihtX;
            int bottomRightY = cell.CoordY + 1;
            bottomRightY = bottomRightY == Height ? Height - 1 : bottomRightY;

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

        private static bool IsMineExists(IEnumerable<ICell> cells)
        {
            return cells.Where(c => c.Mined).Any();
        }
        #endregion
    }
}
