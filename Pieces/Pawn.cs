using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public enum PromotionPiece
    {
        Queen,
        Rook,
        Bishop,
        Knight
    }

    public class Pawn : Piece
    {
        private int _promotionRow;
        private PromotionPiece _promotionPiece;

        public Pawn(int y, int x, bool color) : base(y, x, color)
        { 
            Value = 1;
            _promotionRow = color ? 7 : 0;
            _promotionPiece = PromotionPiece.Queen;
        }

        public override void GetLegalMoves(MoveStatus status)
        {
            _GetAvailablePositions();
            _GetControlledPositions();

            _CheckPinnings();

            if (status == MoveStatus.Check || status == MoveStatus.DoubleCheck)
                _FilterMovesByChecks(status);
        }

        public override void Move(int x, int y)
        {
            base.Move(x, y);
            if(y == _promotionRow)
            {
                // TODO: player could choose promotion piece
                var promotedPiece = _GetPromotedPiece(_promotionPiece);
                var player = Color ? Match.WhitePlayer : Match.BlackPlayer;
                player.Pieces.Remove(this);
                player.Pieces.Add(promotedPiece);
                ChessBoard.GetPosition(y, x).Piece = promotedPiece;
            }
        }

        private Piece _GetPromotedPiece(PromotionPiece piece)
        {
            switch (piece)
            {
                default:
                case PromotionPiece.Queen: return new Queen(Y, X, Color);
                case PromotionPiece.Rook: return new Rook(Y, X, Color);
                case PromotionPiece.Bishop: return new Bishop(Y, X, Color);
                case PromotionPiece.Knight: return new Knight(Y, X, Color);
            }
        }

        private void _GetAvailablePositions()
        {
            // reset available positions
            AvailablePositions.Clear();

            // increase Y if it's white pawn, decrease if it's black pawn
            if (ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X).Piece == null)
            {
                // if the square in front of pawn is free, add to available positions
                AvailablePositions.Add(ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X));

                // if the pawn is on the starting row and the square two steps ahead is free, add to available positions
                if (Color && Y == 1 && ChessBoard.GetPosition(Y + 2, X).Piece == null) // white pawn
                    AvailablePositions.Add(ChessBoard.GetPosition(Y + 2, X));

                if (!Color && Y == 6 && ChessBoard.GetPosition(Y - 2, X).Piece == null) // black pawn
                    AvailablePositions.Add(ChessBoard.GetPosition(Y - 2, X));

            }

            // if the square in front of pawn and --X or ++X has an opponent's piece, add to available positions
            if (X > 0 && ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X - 1).Piece != null && ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X - 1).Piece?.Color != Color)
                AvailablePositions.Add(ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X - 1));

            if (X < 7 && ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X + 1).Piece != null && ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X + 1).Piece?.Color != Color)
                AvailablePositions.Add(ChessBoard.GetPosition(Color ? Y + 1 : Y - 1, X + 1));
        }

        protected override void _GetControlledPositions()
        {
            // reset controlled positions
            ControlledPositions.Clear();

            if (Color) // white pawn
            {
                // if pawn it's not too close to left or top border, add up-left square to controlled positions
                if (X > 0 && Y < 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X - 1));

                // if pawn it's not too close to right or top border, add up-right square to controlled positions
                if (X < 7 && Y < 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X + 1));
            }
            else // black pawn
            {
                // if pawn it's not too close to left or bottom border, add down-left square to controlled positions
                if (X > 0 && Y > 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X - 1));

                // if pawn it's not too close to right or bottom border, add down-right square to controlled positions
                if (X < 7 && Y > 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X + 1));
            }
        }
    }
}
