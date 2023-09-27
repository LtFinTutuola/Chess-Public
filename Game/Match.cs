using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    public enum MatchStatus
    {
        Active,
        WhiteWon,
        BlackWon,
        StaleMate,
        WhiteResignation,
        BlackResignation
    }

    public class Match : ObservableObject
    {
        public static Player? WhitePlayer;
        public static Player? BlackPlayer;
        public ChessBoard Chessboard;
        public static List<Move>? GameMoves = new();
        public static Move? CurrentMove;

        private static int _turnCounter;
        public static bool Trait;

        private static MatchStatus _matchStatus;
        public static MatchStatus MatchStatus
        {
            get { return _matchStatus; }
            set
            {
                _matchStatus = value;
                _CheckMatchStatus();
            }
        }

        private bool _chessBoardView;
        public bool ChessBoardView { set { _chessBoardView = value; NotifyPropertyChanged(this, nameof(ChessBoardView)); } }

        public Match()
        {
            WhitePlayer = new Player(true);
            BlackPlayer = new Player(false);

            Chessboard = new(WhitePlayer.Pieces.Concat(BlackPlayer.Pieces).ToList());
            CurrentMove = new Move(Trait = true, ++_turnCounter);
        }

        private static void _CheckMatchStatus()
        {
            switch (_matchStatus)
            {
                default:
                case MatchStatus.Active:
                    _ResetChecks(GameMoves.LastOrDefault()); // reset previous move's checks
                    GameMoves.Add(CurrentMove);
                    Trait = !Trait;
                    CurrentMove = new Move(Trait, ++_turnCounter);
                    break;
                case MatchStatus.WhiteWon:
                    break;
                case MatchStatus.BlackWon:
                    break;
                case MatchStatus.StaleMate:
                    break;
                case MatchStatus.WhiteResignation:
                    break;
                case MatchStatus.BlackResignation:
                    break;
            }
        }

        private static void _ResetChecks(Move currMove)
        {
            // return if method is called on first move
            if (currMove == null) return;

            //foreach(var piece in GetDeploymentPieces(currMove.Trait, false).Where(p => p.IsChecking))
            foreach(var piece in ChessBoard.GetPieces(currMove.Trait).Where(p => p.IsChecking))
                piece.IsChecking = false;
        }
    }
}
