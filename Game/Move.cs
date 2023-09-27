using Chess.Pieces;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chess.Game
{
    public enum MoveStatus
    {
        NoCheck,
        Check,
        DoubleCheck,
        CheckMate
    }

    public class Move 
    {
        private Position? _startingPosition;
        public Position? StartingPosition
        {
            get { return _startingPosition; }
            set { _startingPosition = value; }
        }

        private Position? _endingPosition;
        public Position? EndingPosition
        {
            get { return _endingPosition; }
            set { _endingPosition = value; _ExecuteMove(); }
        }

        private bool _trait;
        public bool Trait 
        {
            get { return _trait; }
            set 
            { 
                _trait = value;
                if (_trait) Match.WhitePlayer.Timer.Start();
                else Match.BlackPlayer.Timer.Start();
            }
        }        
        public Piece Piece { get; set; }

        public MoveStatus MoveStatus;
        public Piece SecondCheckingPiece;
        public int MoveNumber;

        public Move(bool trait, int moveNumber)
        {
            Trait = trait;
            MoveNumber = moveNumber;
        }

        private void _ExecuteMove()
        {
            if (_trait) // if trait belongs to WHITE player
            {
                Match.WhitePlayer.Timer.Stop();
                Match.BlackPlayer.Timer.Start();
            }
            else // if trait belongs to BLACK player
            {
                Match.BlackPlayer.Timer.Stop();
                Match.WhitePlayer.Timer.Start();
            }

            // if ending position is alredy occupied by another piece, that piece is killed
            if (_endingPosition.Piece != null)
            {
                _endingPosition.Piece.SetIsAlive(false);
                if (_endingPosition.Piece is LongshotPiece lPiece) lPiece.KingXRayAttack = new();
            }

            // move the piece from the starting position to the ending position
            _endingPosition.Piece = _startingPosition.Piece;
            _endingPosition.Piece.Move(_endingPosition.X, _endingPosition.Y);

            if (_endingPosition.Piece is ISpecialMovesPiece sPiece && sPiece.SpecialMoves.Any(p => p.Position.Equals(_endingPosition)))
                sPiece.SpecialMoves.FirstOrDefault(p => p.Position.Equals(_endingPosition)).Execute();

            _startingPosition.Piece = null;
            Piece = _endingPosition.Piece;

            Piece.GetLegalMoves(MoveStatus);
            MoveStatus = _LookForChecks(Piece, MoveStatus);

            // update positions of the other pieces on the chessboard
            _UpdateCascadePiecesPositions();

            MoveStatus = ChessBoard.GetPieces(!_trait).Any(p => p.AvailablePositions.Any()) ? MoveStatus : MoveStatus.CheckMate;            

            if (MoveStatus == MoveStatus.CheckMate)
            {
                Match.MatchStatus = _trait ? MatchStatus.WhiteWon : MatchStatus.BlackWon;
                MessageBox.Show($"It's checkmate, {Match.MatchStatus}");
            }

            else Match.MatchStatus = MatchStatus.Active;
        }

        private MoveStatus _LookForChecks(Piece piece, MoveStatus currentStatus)
        {
            //if (currentStatus == MoveStatus.CheckMate) return currentStatus;

            var oppKing = ChessBoard.GetKing(!_trait);
            if (piece.ControlledPositions.Contains(ChessBoard.GetPosition(oppKing.Y, oppKing.X)))
            {
                // set IsChecking flag to true, thus calculate checking trajectory
                piece.IsChecking = true;

                // if move status is alredy check, it means that is a double check
                if (currentStatus == MoveStatus.Check)
                {
                    SecondCheckingPiece = ChessBoard.GetPieces(_trait, new Piece[] { piece }).FirstOrDefault(p => p.IsChecking);
                    currentStatus = MoveStatus.DoubleCheck;
                }
                else currentStatus = MoveStatus.Check;
            }
            return currentStatus;
        }

        private void _UpdateCascadePiecesPositions()
        {
            // update positions of moved piece's deployment pieces to find double checks or discovery checks
            foreach (var piece in ChessBoard.GetPieces(Piece.Color, new Piece[] { Piece })
                .Where(p => p.ControlledPositions.Contains(_startingPosition) || p.ControlledPositions.Contains(_endingPosition)))
            {
                piece.GetLegalMoves(MoveStatus.NoCheck);
                if (piece.GetType() != typeof(King)) MoveStatus = _LookForChecks(piece, MoveStatus);
            }

            // update positions of moved piece's opponent deployment
            foreach (var piece in ChessBoard.GetPieces(!Piece.Color)) piece.GetLegalMoves(MoveStatus);
        }
    }
}
