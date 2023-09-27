using Chess.Game;
using Chess.SpecialMoves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Chess.Pieces
{
    public class King : Piece, ISpecialMovesPiece
    {
        public List<SpecialMove> SpecialMoves { get; set; } = new();

        public King(int y, int x, bool color) : base(y, x, color) { }

        public override void Move(int x, int y)
        {
            foreach (var piece in ChessBoard.GetLongshotPieces(!Color))
            {
                if (piece.KingXRayAttack.Contains(ChessBoard.GetPosition(Y, X)) && !piece.KingXRayAttack.Contains(ChessBoard.GetPosition(y, x))) piece.KingXRayAttack = new();
                else piece.SetXRayAttacks();
            }            
            base.Move(x, y);
        }

        protected override void _GetControlledPositions()
        {
            // reset available positions
            ControlledPositions.Clear();

            // move right => increase X
            if (X + 1 <= 7) ControlledPositions.Add(ChessBoard.GetPosition(Y, X + 1));
            // move left => decrease X
            if (X - 1 >= 0) ControlledPositions.Add(ChessBoard.GetPosition(Y, X - 1));
            // move up => increase Y
            if (Y + 1 <= 7) ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X));
            // move down => decrease Y
            if (Y - 1 >= 0) ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X));
            // move upper right => increase X, increase Y
            if (X + 1 <= 7 && Y + 1 <= 7) ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X + 1));
            // move lower right => increase X, decrease Y
            if (X + 1 <= 7 && Y - 1 >= 0) ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X + 1));
            // move upper left => decrease X, increase Y
            if (X - 1 >= 0 && Y + 1 <= 7) ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X - 1));
            // move lower left => decrease X, decrease Y
            if (X - 1 >= 0 && Y - 1 >= 0) ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X - 1));
        }        
        public override void GetLegalMoves(MoveStatus status)
        {
            //MoveStatus is always NoCheck because king don't require available positions filtering as other pieces
            base.GetLegalMoves(MoveStatus.NoCheck);
            _GetSpecialMoves();

            // exclude from king's available positions all available position of opponent's piece
            // this is needed to exclude forbidden squares in case of check and to avoid that king could check itself
            foreach (var piece in ChessBoard.GetPieces(!Color)) 
                AvailablePositions = AvailablePositions.Except(piece.ControlledPositions).ToList();
            
            // if king is under check and checking piece is longshot piece, remove its trajectory from king's available moves
            if(status == MoveStatus.Check || status == MoveStatus.DoubleCheck)
            {
                foreach(LongshotPiece piece in ChessBoard.GetLongshotPieces(!Color).Where(p => p.IsChecking))
                    AvailablePositions = AvailablePositions.Except(piece.GetLongshotTrajectory(piece.GetTrajectoryDirection(this), TrajectoryType.AllPositions)).ToList();                
            }
        }

        protected override void _GetSpecialMoves()
        {
            // if king was alredy moved, no castles are available
            if(Match.GameMoves.Any(m => m.Piece == this)) return;
            
            // check short castle
            if(ChessBoard.GetPieces(Color).FirstOrDefault(p => p is Rook rook && rook.X > X && rook.Y == Y) is Rook sCastleRook 
                && _CheckCastle(sCastleRook))
            {
                var sCastlePosition = ChessBoard.GetPosition(Y, X + 2);
                AvailablePositions.Add(sCastlePosition);
                if(!SpecialMoves.Any(s => s is ShortCastle)) SpecialMoves.Add(new ShortCastle(sCastlePosition, sCastleRook));
            }
            else SpecialMoves.Remove(SpecialMoves.FirstOrDefault(s => s is ShortCastle));

            // check long castle
            if (ChessBoard.GetPieces(Color).FirstOrDefault(p => p is Rook rook && rook.X < X && rook.Y == Y) is Rook lCastleRook
                && _CheckCastle(lCastleRook))
            {
                var lCastlePosition = ChessBoard.GetPosition(Y, X - 2);
                AvailablePositions.Add(lCastlePosition);
                if (SpecialMoves.Any(s => s is LongCastle)) SpecialMoves.Add(new LongCastle(lCastlePosition, lCastleRook));
            }
            else SpecialMoves.Remove(SpecialMoves.FirstOrDefault(s => s is LongCastle));
        }

        private bool _CheckCastle(Rook rook)
        {
            return rook != null && !Match.GameMoves.Any(m => m.Piece == rook) && _CheckCastlingPositions(rook);
        }

        private bool _CheckCastlingPositions(Rook rook)
        {
            for(int i = Math.Min(X + 1, rook.X + 1); i < Math.Max(X, rook.X); i++)
            {
                var position = ChessBoard.GetPosition(Y, i);
                if (ChessBoard.GetPieces(!Color).Any(p => p.ControlledPositions.Contains(position))
                || position.Piece != null)
                    return false;
            }
            return true;
        }
    }
}
