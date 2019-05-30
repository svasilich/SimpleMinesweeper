using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core.GameSettings
{
    public static class SettingsHelper
    {
        public const int MinHeight = 5;
        public const int MinWidth = 5;
        public const int MinMineCount = 1;

        public static bool CheckValidity(int height, int width, int mineCount, out string reason)
        {
            if (height < MinHeight)
            {
                reason = $"Высота не может быть меньше {MinHeight}.";
                return false;
            }

            if (width <= 4)
            {
                reason = $"Ширина не может быть меньше {MinWidth}.";
                return false;
            }

            if (mineCount <= 0)
            {
                reason = $"Мин на поле не может быть меньше {MinMineCount}.";
                return false;
            }

            if (height * width <= mineCount)
            {
                reason = "Слишком много мин.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }
}
