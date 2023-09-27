using Chess.Game;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Pieces
{
    //public enum MoveType
    //{
    //    Normal,
    //    Capture,
    //    CaptureEnPassant,
    //    ShortCastle,
    //    LongCastle,
    //    PromoteToQueen,
    //    PromoteToRook,
    //    PromoteToKnight,
    //    PromoteToBishop
    //}

    public abstract class Piece 
    {
        public int Y;
        public int X;
        public int Value;
        public bool Color;
        public bool IsAlive;
        public bool IsChecking;
        public LongshotPiece? PinnerPiece;

        // get the positions in which piece could move
        public List<Position> AvailablePositions = new();

        // get the positions controlled by piece
        public List<Position> ControlledPositions = new();
        
        protected Piece(int y, int x, bool color)
        {
            Y = y;
            X = x;
            Color = color;
            IsAlive = true;
        }

        public virtual void Move(int x, int y)
        {
            X = x;
            Y = y;

            foreach (var piece in ChessBoard.GetLongshotPieces(!Color).Where(p => p.KingXRayAttack.Contains(ChessBoard.GetPosition(Y, X))))
                piece.SetXRayAttacks();            
        }

        public virtual void SetIsAlive(bool value)
        {
            IsAlive = value;
        }

        protected abstract void _GetControlledPositions();

        /// <summary> Get selected piece's legal moves, filtered by checks, pinnings or forbidden squares (for kings) </summary>
        /// <param name="status">Define current check status, in case of checks or double checks, piece will filter its legal moves by parry or escape check</param>
        public virtual void GetLegalMoves(MoveStatus status)
        {
            _GetControlledPositions();
            _CheckPinnings();

            // remove from available positions square owned by pieces of the same deployment
            AvailablePositions = ControlledPositions.Except(ControlledPositions.Where(p => p.Piece?.Color == Color)).ToList();

            if(status == MoveStatus.Check || status == MoveStatus.DoubleCheck) _FilterMovesByChecks(status);
        }

        /// <summary> Return positions in which opponent pieces could parry the check </summary>
        /// <param name="oppKing">King under check</param> 
        public virtual List<Position> GetCheckTrajectory(Piece oppKing)
        {
            return new List<Position> { ChessBoard.GetPosition(Y, X) };
        }

        protected void _FilterMovesByChecks(MoveStatus status)
        {
            // if previous move is a double check, no piece can parry it
            if (status == MoveStatus.DoubleCheck) AvailablePositions.Clear();

            // if previous move is a simple check, filter available positions by checking piece's check trajectory
            if (status == MoveStatus.Check)
                AvailablePositions = AvailablePositions.Intersect(ChessBoard.GetPieces(!Color).FirstOrDefault(p => p.IsChecking).GetCheckTrajectory(ChessBoard.GetKing(Color))).ToList();
        }

        protected void _CheckPinnings()
        {
            if (PinnerPiece != null) ControlledPositions = ControlledPositions.Intersect(PinnerPiece.KingXRayAttack).ToList();

            foreach (var piece in ChessBoard.GetLongshotPieces(null).Where(p => p.KingXRayAttack.Contains(ChessBoard.GetPosition(Y, X))))
                piece.EvaluatePinnings(piece.KingXRayAttack);            
        }

        protected virtual void _GetSpecialMoves() { }
    }
}
