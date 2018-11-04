﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeperTests.Common;

namespace SimpleMinesweeperTests.ViewModel
{
    class TestMinefieldViewModel : MinefieldViewModel
    {
        public static int DefaultFieldHeightCell { get { return 16; } }
        public static int DefaultFieldWidthCell { get { return 30; } }
        public static int DefaultMineCount { get { return 99; } }


        public static IMinefield DefaultMinefield(int fieldHeightCell, int fieldWidthCell)
        {
            IMinefield field = MinefieldTestHelper.CreateDefaultMinefield();
            field.Fill(fieldHeightCell, fieldWidthCell, DefaultMineCount);
            return field;
        }

        public TestMinefieldViewModel() :
            base(DefaultMinefield(DefaultFieldHeightCell, DefaultFieldWidthCell), MinefieldTestHelper.FakeMainWindow())
        {

        }

        public TestMinefieldViewModel(int height, int width) :
            base(DefaultMinefield(height, width), MinefieldTestHelper.FakeMainWindow())
        {

        }

        public TestMinefieldViewModel(IMinefield minefield) : base(minefield, MinefieldTestHelper.FakeMainWindow())
        {
        }

        public void SetSize(double widthPx, double heightPx)
        {
            ResizeField(widthPx, heightPx);
        }

        public IMinefield Field { get { return field; } }
    }
}