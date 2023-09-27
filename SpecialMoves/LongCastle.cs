using Chess.Game;
using Chess.Pieces;

namespace Chess.SpecialMoves
{
    public class LongCastle : SpecialMove
    {
        public LongCastle(Position position, Piece involvedPiece) : base(position, involvedPiece) { }
        public override void Execute()
        {
            InvolvedPiece.Move(3, InvolvedPiece.Y);
        }
    }
}
