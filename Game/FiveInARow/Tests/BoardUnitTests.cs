using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveInARow;

namespace Tests
{
    [TestClass]
    public class BoardUnitTests
    {
        [TestMethod]
        public void DirectExpand_Four_Tests()
        {
            var board = new Board();

            board.Data[0, 0] = BoardStatus.White;
            board.Data[1, 1] = BoardStatus.Black;
            board.Data[2, 2] = BoardStatus.Black;
            board.Data[3, 3] = BoardStatus.Black;
            board.Data[4, 4] = BoardStatus.Black;

            Assert.AreEqual(1, board.DirectExpand(BoardStatus.Black, 1, 1, 1, 1, 1, 4));

            board.Data[5, 5] = BoardStatus.White;
            Assert.AreEqual(0, board.DirectExpand(BoardStatus.Black, 1, 1, 1, 1, 1, 4));

            board.Data[0, 0] = BoardStatus.Empty;
            Assert.AreEqual(1, board.DirectExpand(BoardStatus.Black, 1, 1, 1, 1, 1, 4));

            board.Data[5, 5] = BoardStatus.Empty;
            Assert.AreEqual(1, board.DirectExpand(BoardStatus.Black, 1, 1, 1, 1, 2, 4));
        }

        [TestMethod]
        public void DirectExpand_Three_Tests()
        {
            var board = new Board();

            board.Data[0, 0] = BoardStatus.Black;
            board.Data[0, 1] = BoardStatus.White;
            board.Data[0, 2] = BoardStatus.White;
            board.Data[0, 3] = BoardStatus.White;

            Assert.AreEqual(1, board.DirectExpand(BoardStatus.White, 0, 2, 0, 1, 1, 3));

            board.Data[0, 0] = BoardStatus.Empty;
            Assert.AreEqual(1, board.DirectExpand(BoardStatus.White, 0, 1, 0, 1, 2, 3));

            board.Data[0, 4] = BoardStatus.Black;
            Assert.AreEqual(1, board.DirectExpand(BoardStatus.White, 0, 3, 0, 1, 1, 3));

            Assert.AreEqual(0, board.DirectExpand(BoardStatus.White, 0, 2, 0, 1, 2, 3));
        }

        [TestMethod]
        public void JumpExpand_Three_Tests()
        {
            var board = new Board();

            board.Data[0, 0] = BoardStatus.Black;
            board.Data[1, 0] = BoardStatus.White;
            board.Data[2, 0] = BoardStatus.White;
            board.Data[4, 0] = BoardStatus.White;

            Assert.AreEqual(0, board.DirectExpand(BoardStatus.White, 2, 0, 1, 0, 1, 3));
            Assert.AreEqual(0, board.DirectExpand(BoardStatus.White, 1, 1, 0, 1, 2, 3));
            Assert.AreEqual(1, board.JumpExpand(BoardStatus.White, 2, 0, 1, 0, 1, 3));
            Assert.AreEqual(0, board.JumpExpand(BoardStatus.White, 1, 1, 0, 1, 2, 3));

            board.Data[0, 0] = BoardStatus.Empty;
            Assert.AreEqual(0, board.JumpExpand(BoardStatus.White, 1, 1, 0, 1, 2, 3));

            board.Data[3, 0] = BoardStatus.Black;
            Assert.AreEqual(0, board.JumpExpand(BoardStatus.White, 2, 0, 0, 1, 1, 3));
        }

        [TestMethod]
        public void Expand_Tests()
        {
            var board = new Board();

            board.Data[7, 7] = BoardStatus.Black;
            board.Data[8, 6] = BoardStatus.White;
            board.Data[9, 5] = BoardStatus.White;
            board.Data[11, 3] = BoardStatus.White;

            var k = 1;
            var result = board.Expand(BoardStatus.White, 9, 5, ref k, 1, -1, 1);
            Assert.AreEqual(0, result);
            Assert.AreEqual(1, k);

            result = board.Expand(BoardStatus.Empty, 9, 5, ref k, 1, -1, 1);
            Assert.AreEqual(1, result);
            Assert.AreEqual(2, k);

            k = 1;
            result = board.Expand(BoardStatus.White, 9, 5, ref k, -1, 1, -1);
            Assert.AreEqual(1, result);
            Assert.AreEqual(2, k);
        }

        [TestMethod]
        public void GetCurrentPoint_Tests()
        {
            var board = new Board();

            board.Data[7, 7] = BoardStatus.Black;
            board.Data[8, 8] = BoardStatus.White;
            board.Data[9, 9] = BoardStatus.White;
            board.Data[10, 10] = BoardStatus.White;
            board.Data[11, 11] = BoardStatus.White;

            Assert.AreEqual(-Board.Four, board.GetCurrentPoint());

            board.Data[7, 7] = BoardStatus.Empty;
            Assert.AreEqual(-Board.GreatFour - Board.Four, board.GetCurrentPoint());
        }
    }
}
