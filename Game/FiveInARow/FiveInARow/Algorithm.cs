using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInARow
{
    public static class Algorithm
    {
        public static BoardPosition GetBestMove(BoardStatus who, Board board)
        {
            var bestPoint = 0;
            BoardPosition boardPosition = new BoardPosition();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board.Data[i, j] == BoardStatus.Empty)
                    {
                        board.Data[i, j] = who;
                        var point = board.GetCurrentPoint();
                        if ((who == BoardStatus.Black && point > bestPoint) || (who == BoardStatus.White && point < bestPoint))
                        {
                            bestPoint = point;
                            boardPosition.i = i;
                            boardPosition.j = j;
                        }
                        board.Data[i, j] = BoardStatus.Empty;
                    }
                }
            }
            if (boardPosition.i == 0 && boardPosition.j == 0 && bestPoint == 0)
            {
                boardPosition.i = 7;
                boardPosition.j = 7;
                if (who == BoardStatus.Black)
                {
                    return boardPosition;
                }
                else
                {
                    var random = new Random();
                    for (boardPosition.i += random.Next(-1, 1), boardPosition.j += random.Next(-1, 1);
                        boardPosition.i == 7 && boardPosition.j == 7;
                        boardPosition.i += random.Next(-1, 1), boardPosition.j += random.Next(-1, 1))
                    {
                        return boardPosition;
                    }
                }
            }
            return boardPosition;
        }
    }

    public struct BoardPosition
    {
        public int i;
        public int j;
    }
}
