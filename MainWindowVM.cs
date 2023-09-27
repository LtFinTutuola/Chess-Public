using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game;
using CommunityToolkit.Mvvm.Input;
using Chess.Pieces;
using System.Collections.ObjectModel;

namespace Chess
{
    public class MainWindowVM : ObservableObject
    {
        public Match CurrentMatch { get; set; }
        public List<Position> DisplayBoard { get; set; }
        public Piece? SelectedPiece { get; set; }

        public ObservableCollection<Move> Moves { get; set; }
        //public ObservableCollection<Turn> DisplayTurns { get; set; }

        public MainWindowVM()
        {
            CurrentMatch = new Match();
            //Match.TurnClosed += Match_TurnClosed;
            DisplayBoard = _GetDisplayBoard(ChessBoard.GetPositions());
            //DisplayTurns = new ObservableCollection<Turn>();
            Match.WhitePlayer.Timer.Start();
        }

        // TODO trovare un modo più carino per aggiornare la visualizzazione dei turni
        //private void Match_TurnClosed(object sender, EventArgs e)
        //{
        //    DisplayTurns.Add(Match.CurrentTurn);
        //    NotifyPropertyChanged(this, nameof(DisplayTurns));
        //}

        private List<Position> _GetDisplayBoard(Position[,] rawPositions)
        {
            var displayPositions = new List<Position>();
            foreach (var position in rawPositions)
                displayPositions.Add(position);

            return displayPositions;
        }

        private RelayCommand<object> _selectSquareCommand;
        public RelayCommand<object> SelectSquareCommand => _selectSquareCommand ??= new RelayCommand<object>(_SelectSquareExecution);

        private void _SelectSquareExecution(object sender)
        {
            var position = sender as Position;
            if (position == null) return;

            // if selected square has a piece and piece's color is WHITE set move's starting position
            if (position.Piece != null && Match.Trait == position.Piece?.Color)
            {
                Match.CurrentMove.StartingPosition = position;
                SelectedPiece = Match.CurrentMove.StartingPosition.Piece;
            }

            // if current move has alredy a starting position, set ending position
            else if (Match.CurrentMove.StartingPosition != null && Match.CurrentMove.StartingPosition.Piece.AvailablePositions.Contains(position))
            {
                Match.CurrentMove.EndingPosition = position;
                SelectedPiece = null;
            }
            NotifyPropertyChanged(this, nameof(SelectedPiece));
        }
    }
}
