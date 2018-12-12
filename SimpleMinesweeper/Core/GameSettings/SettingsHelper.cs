using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core.GameSettings
{
    public static class SettingsHelper
    {
        public static bool CheckValidity(int height, int width, int mineCount, out string reason)
        {
            if (height == 0)
            {
                reason = "Высота должна быть ненулевой.";
                return false;
            }

            if (width == 0)
            {
                reason = "Ширина должна быть ненулевой.";
                return false;
            }

            if (mineCount == 0)
            {
                reason = "Количество мин должно быть больше нуля.";
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
