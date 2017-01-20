using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveInARow;

namespace Tests
{
    [TestClass]
    public class AlgorithmUnitTests
    {
        [TestMethod]
        public void GetBestMove_Black_Tests()
        {
            var board = new Board();

            board.Data[0, 1] = BoardStatus.Black;
            board.Data[0, 2] = BoardStatus.Black;
            board.Data[9, 9] = BoardStatus.White;
            board.Data[10, 10] = BoardStatus.White;
            board.Data[11, 11] = BoardStatus.White;

            var bestMove = Algorithm.GetBestMove(BoardStatus.Black, board);
            Assert.IsTrue((bestMove.i == 8 && bestMove.j == 8) || (bestMove.i == 12 && bestMove.j == 12));

            board.Data[0, 3] = BoardStatus.Black;
            board.Data[8, 8] = BoardStatus.Black;
            board.Data[12, 12] = BoardStatus.White;

            bestMove = Algorithm.GetBestMove(BoardStatus.Black, board);
            Assert.AreEqual(13, bestMove.i);
            Assert.AreEqual(13, bestMove.j);
        }

        [TestMethod]
        public void GetBestMove_White_Tests()
        {
            var board = new Board();

            board.Data[0, 1] = BoardStatus.White;
            board.Data[0, 2] = BoardStatus.White;
            board.Data[9, 9] = BoardStatus.Black;
            board.Data[10, 10] = BoardStatus.Black;
            board.Data[11, 11] = BoardStatus.Black;

            var bestMove = Algorithm.GetBestMove(BoardStatus.White, board);
            Assert.IsTrue((bestMove.i == 8 && bestMove.j == 8) || (bestMove.i == 12 && bestMove.j == 12));

            board.Data[0, 3] = BoardStatus.White;
            board.Data[8, 8] = BoardStatus.White;
            board.Data[12, 12] = BoardStatus.Black;

            int score;
            bestMove = Algorithm.GetBestMove(BoardStatus.White, board, 2, out score);
            Assert.AreEqual(13, bestMove.i);
            Assert.AreEqual(13, bestMove.j);
        }

        [TestMethod]
        public void GetBestMove_White2_Tests()
        {
            var board = new Board();

            board.Data[3, 1] = BoardStatus.White;
            board.Data[3, 2] = BoardStatus.White;
            board.Data[1, 3] = BoardStatus.White;
            board.Data[2, 3] = BoardStatus.White;
            board.Data[9, 9] = BoardStatus.Black;
            board.Data[10, 10] = BoardStatus.Black;
            board.Data[11, 11] = BoardStatus.Black;

            var bestMove = Algorithm.GetBestMove(BoardStatus.White, board);
            Assert.IsTrue((bestMove.i == 8 && bestMove.j == 8) || (bestMove.i == 12 && bestMove.j == 12));

            board.Data[3, 3] = BoardStatus.White;
            board.Data[8, 8] = BoardStatus.White;
            board.Data[12, 12] = BoardStatus.Black;

            int score;
            bestMove = Algorithm.GetBestMove(BoardStatus.White, board, 2, out score);
            Assert.AreEqual(13, bestMove.i);
            Assert.AreEqual(13, bestMove.j);
        }
    }
}
