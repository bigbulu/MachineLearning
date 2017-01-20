using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FiveInARow
{
    public class Board
    {
        public BoardStatus[,] Data = new BoardStatus[15, 15];

        public bool? WinOrLost(int i, int j)
        {
            var result = new List<bool?>();
            result.Add(WinOrLost(i, j, 1, 0));
            result.Add(WinOrLost(i, j, 0, 1));
            result.Add(WinOrLost(i, j, 1, 1));
            result.Add(WinOrLost(i, j, 1, -1));

            if (result.Any(a => a == false))
            {
                return false;
            }
            if (result.Any(a => a == true))
            {
                return true;
            }
            if (IsForbiddenMove(i, j))
            {
                return false;
            }
            return null;
        }

        private bool? WinOrLost(int i, int j, int operator1, int operator2)
        {
            var who = Data[i, j];
            int count = 1;
            int k = 1;
            count += Expand(who, i, j, ref k, operator1, operator2, -1);
            k = 1;
            count += Expand(who, i, j, ref k, -operator1, -operator2, -1);
            if (count == 5)
            {
                return true;
            }
            if (count > 5)
            {
                return who != BoardStatus.Black;
            }
            return null;
        }

        private bool IsForbiddenMove(int i, int j)
        {
            var who = Data[i, j];
            if (who == BoardStatus.White)
            {
                return false;
            }

            // check 3
            if (NormalCount(who, i, j, 3, 3) + JumpCount(who, i, j, 3, 2) >= 2)
            {
                return true;
            }

            // check 4
            if (NormalCount(who, i, j, 4, 2) + JumpCount(who, i, j, 4, 1) >= 2)
            {
                return true;
            }

            return false;
        }

        private int NormalCount(BoardStatus who, int i, int j, int number, int emptyCount)
        {
            int result = 0;
            // from left to right
            result += DirectExpand(who, i, j, 1, 0, emptyCount, number);
            // from up to down
            result += DirectExpand(who, i, j, 0, 1, emptyCount, number);
            // from left-up to down-right
            result += DirectExpand(who, i, j, 1, 1, emptyCount, number);
            // from left-down to up-right
            result += DirectExpand(who, i, j, 1, -1, emptyCount, number);
            return result;
        }

        private bool IsPositionValid(int position)
        {
            return position >= 0 && position < 15;
        }

        public int DirectExpand(BoardStatus who, int i, int j, int operator1, int operator2, int emptyCount, int number)
        {
            int empty = 0;
            int count = 1;
            int k;
            k = 1;
            count += Expand(who, i, j, ref k, operator1, operator2, -1);
            empty += Expand(BoardStatus.Empty, i, j, ref k, operator1, operator2, emptyCount - 1);
            if (empty > 0 || emptyCount == 1)
            {
                k = 1;
                count += Expand(who, i, j, ref k, -operator1, -operator2, -1);
                empty += Expand(BoardStatus.Empty, i, j, ref k, -operator1, -operator2, emptyCount - empty);
            }
            return (empty >= emptyCount && count == number) ? 1 : 0;
        }

        private int JumpCount(BoardStatus who, int i, int j, int count, int emptyCount)
        {
            var result = 0;
            result += JumpExpand(who, i, j, 1, 0, emptyCount, count);
            result += JumpExpand(who, i, j, 0, 1, emptyCount, count);
            result += JumpExpand(who, i, j, 1, 1, emptyCount, count);
            result += JumpExpand(who, i, j, 1, -1, emptyCount, count);
            return result;
        }

        public int Expand(BoardStatus who, int i, int j, ref int k, int operator1, int operator2, int number)
        {
            int pos1, pos2;
            int count = 0;
            for (pos1 = i + operator1 * k, pos2 = j + operator2 * k;
                IsPositionValid(pos1) && IsPositionValid(pos2) && Data[pos1, pos2] == who;
                k++, pos1 = i + operator1 * k, pos2 = j + operator2 * k)
            {
                count++;
                if (count >= number && number != -1)
                {
                    k++;
                    break;
                }
            }
            return count;
        }

        public int JumpExpand(BoardStatus who, int i, int j, int operator1, int operator2, int emptyCount, int number)
        {
            int count = 1, result = 0;
            int k, jumpCount1 = 0, jumpCount2 = 0;
            bool jumpCount1HasEmpty = false, jumpCount2HasEmpty = false, hasEmpty1 = false, hasEmpty2 = false;
            k = 1;
            count += Expand(who, i, j, ref k, operator1, operator2, -1);
            hasEmpty1 = Expand(BoardStatus.Empty, i, j, ref k, operator1, operator2, 1) == 1;
            if (hasEmpty1)
            {
                jumpCount1 = Expand(who, i, j, ref k, operator1, operator2, -1);
                jumpCount1HasEmpty = Expand(BoardStatus.Empty, i, j, ref k, operator1, operator2, 1) == 1;
            }

            k = 1;
            count += Expand(who, i, j, ref k, -operator1, -operator2, -1);
            hasEmpty2 = Expand(BoardStatus.Empty, i, j, ref k, -operator1, -operator2, 1) == 1;
            if (hasEmpty2)
            {
                jumpCount2 = Expand(who, i, j, ref k, -operator1, -operator2, -1);
                jumpCount2HasEmpty = Expand(BoardStatus.Empty, i, j, ref k, -operator1, -operator2, 1) == 1;
            }

            if (jumpCount1 != 0 && ((emptyCount == 2 && jumpCount1HasEmpty && hasEmpty2) || (emptyCount == 1 && (jumpCount1HasEmpty || hasEmpty2))))
            {
                if (count + jumpCount1 == number)
                {
                    result++;
                }
            }
            if (count == 3 && result > 0)
            {
                return result;
            }

            if (jumpCount2 != 0 && ((emptyCount == 2 && jumpCount2HasEmpty && hasEmpty1) || (emptyCount == 1 && (jumpCount2HasEmpty || hasEmpty1))))
            {
                if (count + jumpCount2 == number)
                {
                    result++;
                }
            }
            return number == 3 && result > 0 ? 1 : result;
        }

        public static readonly int Five = 100000000;
        public static readonly int GreatFour = 100000;
        public static readonly int Four = 10000;
        public static readonly int JumpFour = 5000;
        public static readonly int Three = 500;
        public static readonly int JumpThree = 250;
        public static readonly int Two = 10;

        public int GetCurrentPoint()
        {
            int result = 0;
            int blackFour = 0, blackThree = 0, whiteFour = 0, whiteThree = 0;
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (Data[i, j] != BoardStatus.Empty)
                    {
                        int pointOperator = Data[i, j] == BoardStatus.Black ? 1 : -1;
                        if (NormalCount(Data[i, j], i, j, 5, 0) > 0)
                        {
                            return Five * pointOperator;
                        }

                        result += NormalCount(Data[i, j], i, j, 4, 2) * pointOperator * GreatFour / 4;
                        if (pointOperator > 0)
                        {
                            blackFour += NormalCount(Data[i, j], i, j, 4, 1) + JumpCount(Data[i, j], i, j, 4, 1);
                            blackThree += NormalCount(Data[i, j], i, j, 3, 2) + JumpCount(Data[i, j], i, j, 3, 2);
                        }
                        else
                        {
                            whiteFour += NormalCount(Data[i, j], i, j, 4, 1) + JumpCount(Data[i, j], i, j, 4, 1);
                            whiteThree += NormalCount(Data[i, j], i, j, 3, 2) + JumpCount(Data[i, j], i, j, 3, 2);
                        }
                        result += NormalCount(Data[i, j], i, j, 2, 3) * pointOperator * Two;
                        result += JumpCount(Data[i, j], i, j, 2, 3) * pointOperator * Two;
                    }
                }
            }
            blackFour /= 4; blackThree /= 3; whiteFour /= 4; whiteThree /= 3;
            if (blackFour + blackThree >= 2)
            {
                result += GreatFour / 2;
            }
            else
            {
                result += blackFour * Four;
                result += blackThree * Three;
            }
            if (whiteFour + whiteThree >= 2)
            {
                result -= GreatFour / 2;
            }
            else
            {
                result -= whiteFour * Four;
                result -= whiteThree * Three;
            }
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
