using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.ViewModel
{
    public interface IDynamicGameFieldSize
    {
        double ContainerHeight { get; }
        double ContainetWidth { get; }
    }
}
