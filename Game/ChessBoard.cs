using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;

namespace Chess.Game
{
    public class ChessBoard
    {
        private static Position[,] _positions;

        public ChessBoard(List<Piece> pieces)
        {
            _SetPositions(pieces);
        }

        private void _SetPositions(List<Piece> pieces)
        {
            _positions = new Position[8, 8];
            var color = true;
            for (int x = 0; x < _positions.GetLength(0); x++)
            {
                color = !color;
                for (int y = 0; y < _positions.GetLength(1); y++)
                {
                    _positions[y, x] = new Position(color, y, x, pieces.FirstOrDefault(p => p.Y == y && p.X == x));
                    color = !color;
                }
            }
            foreach (var piece in pieces) piece.GetLegalMoves(MoveStatus.NoCheck);           
        }

        public static Position[,] GetPositions() { return _positions; }

        public static Position GetPosition(int y, int x) { return _positions[y, x]; }

        public static List<Piece> GetPieces(bool? color, params Piece[] excludedPieces)
        {
            if (color == null) return _positions.Cast<Position>().Where(p => p.Piece != null && p.Piece.IsAlive).Select(p => p.Piece).Except(excludedPieces.ToList()).ToList();
            else return _positions.Cast<Position>().Where(p => p.Piece != null && p.Piece.IsAlive && p.Piece.Color == color).Select(p => p.Piece).Except(excludedPieces.ToList()).ToList();
        }

        public static List<LongshotPiece> GetLongshotPieces(bool? color, params Piece[] excludedPieces)
        {
            return GetPieces(color, excludedPieces).OfType<LongshotPiece>().ToList();
        }

        public static Piece GetKing(bool color)
        {
            return _positions.Cast<Position>().Select(p => p.Piece).FirstOrDefault(p => p is King && p.Color == color);
        }
    }
}
