using Chess.Game;
using Chess.Pieces;

namespace Chess.SpecialMoves
{
    public abstract class SpecialMove
    {
        public Position Position;
        public Piece InvolvedPiece;
        public SpecialMove(Position position, Piece involvedPiece) 
        {
            Position = position;
            InvolvedPiece = involvedPiece;
        }
        public abstract void Execute();
    }
}
