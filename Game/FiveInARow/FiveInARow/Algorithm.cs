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
        private static int total = 0;
        private static int calculate = 0;

        public static BoardPosition GetBestMove(BoardStatus who, Board board)
        {
            int score = 0;
            total = 0; calculate = 0;
            var cache = new Dictionary<int, int>();
            var boardPosition = GetBestMove(who, board, 4, cache, out score);
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
            Debug.Print(string.Format("Total: {0} Skip:{1} Rate:{2} Score:{3}",
                total, (total - calculate), (total - calculate) * 1.0 / total, score));
            return boardPosition;
        }

        public static BoardPosition GetBestMove(BoardStatus who, Board board, int depth, Dictionary<int, int> cache, out int score)
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

            total += 15 * 15;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board.Data[i, j] == BoardStatus.Empty && board.shouldCheck[i, j] > 0)
                    {
                        board.Set(i, j, who);
                        var point = board.GetCurrentPoint();
                        if ((who == BoardStatus.Black && point > defaultPoint) || (who == BoardStatus.White && point < defaultPoint))
                        {
                            boardPosition.i = i;
                            boardPosition.j = j;
                            resultCandidate.Add(new KeyValuePair<BoardPosition, int>(boardPosition, point));
                        }
                        board.Set(i, j, BoardStatus.Empty);
                        calculate++;

                        if (depth == 1)
                        {
                            if (SkipRemainingItems(cache, who == BoardStatus.White, depth, point))
                            {
                                score = point;
                                return boardPosition;
                            }
                        }
                    }
                }
            }
            if (resultCandidate.Count == 0)
            {
                score = 0;
                return boardPosition;
            }

            int k = 0;
            total += resultCandidate.Count;
            if (who == BoardStatus.Black)
            {
                resultCandidate.Sort((a, b) => { return b.Value.CompareTo(a.Value); });
                if (depth > 1)
                {
                    for (k = 0; k < resultCandidate.Count; k++)
                    {
                        var current = resultCandidate[k];
                        board.Set(current.Key.i, current.Key.j, BoardStatus.Black);

                        int subScore;
                        GetBestMove(BoardStatus.White, board, depth - 1, cache, out subScore);
                        resultCandidate[k] = new KeyValuePair<BoardPosition, int>(resultCandidate[k].Key, subScore);
                        calculate++;
                        board.Set(current.Key.i, current.Key.j, BoardStatus.Empty);

                        if (SkipRemainingItems(cache, true, depth, subScore))
                        {
                            k++;
                            break;
                        }
                    }
                }
            }
            else
            {
                resultCandidate.Sort((a, b) => { return a.Value.CompareTo(b.Value); });
                if (depth > 1)
                {
                    for (k = 0; k < resultCandidate.Count; k++)
                    {
                        var current = resultCandidate[k];
                        board.Set(current.Key.i, current.Key.j, BoardStatus.White);

                        int subScore;
                        GetBestMove(BoardStatus.Black, board, depth - 1, cache, out subScore);
                        resultCandidate[k] = new KeyValuePair<BoardPosition, int>(resultCandidate[k].Key, subScore);
                        calculate++;
                        board.Set(current.Key.i, current.Key.j, BoardStatus.Empty);

                        if (SkipRemainingItems(cache, true, depth, subScore))
                        {
                            k++;
                            break;
                        }
                    }
                }
            }

            var retValue = GetBest(resultCandidate, who, k == 0 ? resultCandidate.Count : k);
            score = retValue.Value;
            cache.Remove(depth);
            UpdateParentValue(cache, who == BoardStatus.White, depth, score);
            return retValue.Key;
        }

        private static void UpdateParentValue(Dictionary<int, int> cache, bool isMinThisTurn, int depth, int score)
        {
            var parentDepth = depth + 1;
            if (!cache.ContainsKey(parentDepth))
            {
                cache[parentDepth] = score;
            }
            else if (isMinThisTurn ? score > cache[parentDepth] : score < cache[parentDepth])
            {
                cache[parentDepth] = score;
            }
        }

        private static bool SkipRemainingItems(Dictionary<int, int> cache, bool isMinThisTurn, int depth, int score)
        {
            if (!cache.ContainsKey(depth))
            {
                cache[depth] = score;
            }
            else
            {
                if (cache.ContainsKey(depth + 1) &&
                    (isMinThisTurn ? score <= cache[depth + 1] : score >= cache[depth + 1]))
                {
                    return true;
                }
            }
            return false;
        }

        private static KeyValuePair<BoardPosition, int> GetBest(List<KeyValuePair<BoardPosition, int>> candidate, BoardStatus who, int k)
        {
            int? score = null;
            BoardPosition position = new BoardPosition();
            for (int i = 0; i < k; i++)
            {
                if (score == null)
                {
                    score = candidate[i].Value;
                    position = candidate[i].Key;
                }
                else if ((who == BoardStatus.Black && candidate[i].Value > score)
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
