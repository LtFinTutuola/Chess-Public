using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Rook : LongshotPiece
    {
        public Rook(int y, int x, bool color) : base(y, x, color)
        {
            Value = 5;
            _availableDirections = new List<MoveDirection> 
            { 
                MoveDirection.Left,
                MoveDirection.Right,
                MoveDirection.Up,
                MoveDirection.Down
            };
        }
    }
}
