using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Queen : LongshotPiece
    {
        public Queen(int y, int x, bool color) : base(y, x, color)
        {
            Value = 9;
            _availableDirections = new List<MoveDirection>
            {
                MoveDirection.Left,
                MoveDirection.Right,
                MoveDirection.Up,
                MoveDirection.Down,
                MoveDirection.UpperRight,
                MoveDirection.LowerRight,
                MoveDirection.LowerLeft,
                MoveDirection.UpperLeft
            };
        }
    }
}
