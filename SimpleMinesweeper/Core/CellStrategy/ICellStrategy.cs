using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public interface ICellStrategy
    {
        ICell Cell { get; }

        void Action();
    }
}
