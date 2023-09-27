using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Bishop : LongshotPiece
    {
        public Bishop(int y, int x, bool color) : base(y, x, color)
        {
            Value = 3;
            _availableDirections = new List<MoveDirection>
            {
                MoveDirection.UpperLeft,
                MoveDirection.UpperRight,
                MoveDirection.LowerLeft,
                MoveDirection.LowerRight
            };
        }
    }
}
