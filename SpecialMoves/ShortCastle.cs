using Chess.Game;
using Chess.Pieces;

namespace Chess.SpecialMoves
{
    public class ShortCastle : SpecialMove
    {
        public ShortCastle(Position position, Piece involvedPiece) : base(position, involvedPiece) { }
        public override void Execute()
        {
            InvolvedPiece.Move(5, InvolvedPiece.Y);
        }
    }
}
