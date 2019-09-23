using System;
namespace SimpleMinesweeper.Core
{
    public class CellChangeStateEventArgs : EventArgs
    {
        public CellState NewState { get; private set; }
        public CellState OldState { get; private set; }

        public CellChangeStateEventArgs(CellState oldState, CellState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}
