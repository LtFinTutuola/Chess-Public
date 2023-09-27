using Chess.SpecialMoves;
using System.Collections.Generic;

namespace Chess.Pieces
{
    public interface ISpecialMovesPiece
    {
        public List<SpecialMove> SpecialMoves { get; set; }
    }
}
