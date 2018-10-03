using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core.GameSettings
{
    public enum GameType
    {
        Newbie,
        Advanced,
        Professional,
        Custom
    }
    
    public class SettingsItem
    {
        public GameType Type { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int MineCount { get; set; }
    }
}
