namespace FiveInARow
{
    public class Board
    {
        public BoardStatus[,] Data = new BoardStatus[15, 15];

        private bool HasWinFive(BoardStatus who, int i, int j)
        {
            if (Data[i, j] == who)
            {
                // from left to right
                var count = 1;
                for (int k = i - 1; k >= 0 && Data[k, j] == who; k--)
                {
                    count++;
                }
                for (int k = i + 1; k < 15 && Data[k, j] == who; k++)
                {
                    count++;
                }
                if (count == 5 || (count > 5 && who != BoardStatus.Black))
                {
                    return true;
                }

                // from up to down
                count = 1;
                for (int k = j - 1; k >= 0 && Data[i, k] == who; k--)
                {
                    count++;
                }
                for (int k = j + 1; k < 15 && Data[i, k] == who; k++)
                {
                    count++;
                }
                if (count == 5 || (count > 5 && who != BoardStatus.Black))
                {
                    return true;
                }

                // from left-up to down-right
                count = 1;
                for (int k = 1; i - k >= 0 && j - k >= 0 && Data[i - k, j - k] == who; k++)
                {
                    count++;
                }
                for (int k = 1; i + k < 15 && j + k < 15 && Data[i + k, j + k] == who; k++)
                {
                    count++;
                }
                if (count == 5 || (count > 5 && who != BoardStatus.Black))
                {
                    return true;
                }

                // from left-down to up-right
                count = 1;
                for (int k = 1; i + k < 15 && j - k >= 0 && Data[i + k, j - k] == who; k++)
                {
                    count++;
                }
                for (int k = 1; i - k >= 0 && j + k < 15 && Data[i - k, j + k] == who; k++)
                {
                    count++;
                }
                if (count == 5 || (count > 5 && who != BoardStatus.Black))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsForbiddenMove(BoardStatus who, int i, int j)
        {
            if (who == BoardStatus.White)
            {
                return false;
            }
            
            // check normal 3
            if (NormalCount(who, i, j, 3, 2) + JumpThreeCount(who, i, j) >= 2)
            {
                return true;
            }

            // check 4
            if (NormalCount(who, i, j, 4, 1) + JumpFourCount(who, i, j) >= 2)
            {
                return true;
            }

            return false;
        }

        private int NormalCount(BoardStatus who, int i, int j, int number, int emptyCount)
        {
            int result = 0;
            int empty = 0;
            // from left to right
            var count = 1;
            int k;
            for (k = i - 1; k >= 0 && Data[k, j] == who; k--)
            {
                count++;
            }
            for (empty = 0; k >= 0 && Data[k, j] == BoardStatus.Empty; k--)
            {
                empty++;
                if (empty == emptyCount)
                {
                    break;
                }
            }
            if (empty == emptyCount)
            {
                for (k = i + 1; k < 15 && Data[k, j] == who; k++)
                {
                    count++;
                }
                for (empty = 0; k < 15 && Data[k, j] == BoardStatus.Empty; k++)
                {
                    empty++;
                    if (empty == emptyCount)
                    {
                        break;
                    }
                }
            }
            if (empty == emptyCount && count == number)
            {
                result++;
            }

            // from up to down
            count = 1;
            for (k = j - 1; k >= 0 && Data[i, k] == who; k--)
            {
                count++;
            }
            for (empty = 0; k >= 0 && Data[i, k] == BoardStatus.Empty; k--)
            {
                empty++;
                if (empty == emptyCount)
                {
                    break;
                }
            }
            if (empty == emptyCount)
            {
                for (k = j + 1; k < 15 && Data[i, k] == who; k++)
                {
                    count++;
                }
                for (empty = 0; k < 15 && Data[i, k] == BoardStatus.Empty; k++)
                {
                    empty++;
                    if (empty == emptyCount)
                    {
                        break;
                    }
                }
            }
            if (empty == emptyCount && count == number)
            {
                result++;
            }

            // from left-up to down-right
            count = 1;
            for (k = 1; i - k >= 0 && j - k >= 0 && Data[i - k, j - k] == who; k++)
            {
                count++;
            }
            for (empty = 0; i - k >= 0 && j - k >= 0 && Data[i - k, j - k] == BoardStatus.Empty; k++)
            {
                empty++;
                if (empty == emptyCount)
                {
                    break;
                }
            }
            if (empty == emptyCount)
            {
                for (k = 1; i + k < 15 && j + k < 15 && Data[i + k, j + k] == who; k++)
                {
                    count++;
                }
                for (empty = 0; i + k < 15 && j + k < 15 && Data[i + k, j + k] == BoardStatus.Empty; k++)
                {
                    empty++;
                    if (empty == emptyCount)
                    {
                        break;
                    }
                }
            }
            if (empty == emptyCount && count == number)
            {
                result++;
            }

            // from left-down to up-right
            count = 1;
            for (k = 1; i + k < 15 && j - k >= 0 && Data[i + k, j - k] == who; k++)
            {
                count++;
            }
            for (empty = 0; i + k < 15 && j - k >= 0 && Data[i + k, j - k] == BoardStatus.Empty; k++)
            {
                empty++;
                if (empty == emptyCount)
                {
                    break;
                }
            }
            if (empty == emptyCount)
            {
                for (k = 1; i - k >= 0 && j + k < 15 && Data[i - k, j + k] == who; k++)
                {
                    count++;
                }
                for (empty = 0; i - k >= 0 && j + k < 15 && Data[i - k, j + k] == BoardStatus.Empty; k++)
                {
                    empty++;
                    if (empty == emptyCount)
                    {
                        break;
                    }
                }
            }
            if (empty == emptyCount && count == number)
            {
                result++;
            }
            return result;
        }

        private int JumpThreeCount(BoardStatus who, int i, int j)
        {
            int result = 0;
            return result;
        }

        private int JumpFourCount(BoardStatus who, int i, int j)
        {
            int result = 0;
            return result;
        }
    }

    public enum BoardStatus
    {
        Empty,
        Black,
        White,
    }
}
