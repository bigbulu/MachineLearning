using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInARow
{
    public static class Algorithm
    {
        public static BoardPosition GetBestMove(BoardStatus who, Board board)
        {
            int score = 0;
            var boardPosition = GetBestMove(who, board, 2, out score);
            if (boardPosition.i == 0 && boardPosition.j == 0 && score == 0)
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
                    boardPosition.i += random.Next(-1, 1);
                    boardPosition.j += boardPosition.i == 7 ? random.Next(1, 2) * 2 - 3 : random.Next(-1, 1);
                    return boardPosition;
                }
            }
            return boardPosition;
        }

        public static BoardPosition GetBestMove(BoardStatus who, Board board, int depth, out int score)
        {
            BoardPosition boardPosition = new BoardPosition();
            var defaultPoint = board.GetCurrentPoint();
            if (Math.Abs(defaultPoint) == Board.Five)
            {
                score = defaultPoint;
                return boardPosition;
            }
            if (depth == 4)
            {
                Debug.Print(defaultPoint.ToString());
            }
            List<KeyValuePair<BoardPosition, int>> resultCandidate = new List<KeyValuePair<BoardPosition, int>>();
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board.Data[i, j] == BoardStatus.Empty)
                    {
                        board.Data[i, j] = who;
                        var point = board.GetCurrentPoint();
                        if ((who == BoardStatus.Black && point > defaultPoint) || (who == BoardStatus.White && point < defaultPoint))
                        {
                            boardPosition.i = i;
                            boardPosition.j = j;
                            resultCandidate.Add(new KeyValuePair<BoardPosition, int>(boardPosition, point));
                        }
                        board.Data[i, j] = BoardStatus.Empty;
                    }
                }
            }
            if (resultCandidate.Count == 0)
            {
                score = 0;
                return boardPosition;
            }

            if (who == BoardStatus.Black)
            {
                resultCandidate.Sort((a, b) => { return b.Value.CompareTo(a.Value); });
                if (depth > 1)
                {
                    for (int i = 0; i < resultCandidate.Count; i++)
                    {
                        var current = resultCandidate[i];
                        board.Data[current.Key.i, current.Key.j] = BoardStatus.Black;

                        int subScore;
                        GetBestMove(BoardStatus.White, board, depth - 1, out subScore);
                        resultCandidate[i] = new KeyValuePair<BoardPosition, int>(resultCandidate[i].Key, subScore);

                        board.Data[current.Key.i, current.Key.j] = BoardStatus.Empty;
                    }
                }
            }
            else
            {
                resultCandidate.Sort((a, b) => { return a.Value.CompareTo(b.Value); });
                if (depth > 1)
                {
                    for (int i = 0; i < resultCandidate.Count; i++)
                    {
                        var current = resultCandidate[i];
                        board.Data[current.Key.i, current.Key.j] = BoardStatus.White;

                        int subScore;
                        GetBestMove(BoardStatus.Black, board, depth - 1, out subScore);
                        resultCandidate[i] = new KeyValuePair<BoardPosition, int>(resultCandidate[i].Key, subScore);

                        board.Data[current.Key.i, current.Key.j] = BoardStatus.Empty;
                    }
                }
            }
            
            var retValue = GetBest(resultCandidate, who);
            score = retValue.Value;
            return retValue.Key;
        }

        private static KeyValuePair<BoardPosition, int> GetBest(List<KeyValuePair<BoardPosition, int>> candidate, BoardStatus who)
        {
            int? score = null;
            BoardPosition position = new BoardPosition();
            for (int i = 0; i < candidate.Count; i++)
            {
                if (score == null)
                {
                    score = candidate[i].Value;
                    position = candidate[i].Key;
                }
                else if ((who == BoardStatus.Black && candidate[i].Value > score )
                    || (who == BoardStatus.White && candidate[i].Value < score))
                {
                    score = candidate[i].Value;
                    position = candidate[i].Key;
                }
            }
            return new KeyValuePair<BoardPosition, int>(position, score.Value);
        }
    }

    public struct BoardPosition
    {
        public int i;
        public int j;
    }
}
