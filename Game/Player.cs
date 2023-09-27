using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Chess.Game
{
    public class Player : ObservableObject
    {
        public List<Piece> Pieces { get; set; }
        public List<Piece> DeadPieces { get; set; }
        public bool Color { get; set; }
        public Timer Timer { get; set; }
        public int TimeToPlay { get; set; }

        public Player(bool color)
        {
            Color = color;
            DeadPieces = new List<Piece>();
            Pieces = _SetPieces(color);
            TimeToPlay = 7200000;
            Timer = new Timer(1000);
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            TimeToPlay -= 1000;
            NotifyPropertyChanged(this, nameof(TimeToPlay));
        }

        private List<Piece> _SetPieces(bool color)
        {          
            return new List<Piece> 
            {
                // GAME SET
                // ----------------------------------
                //// set pieces
                //new Rook(color ? 0 : 7, 0, color),
                //new Knight(color ? 0 : 7, 1, color),
                //new Bishop(color ? 0 : 7, 2, color),
                //new Queen(color ? 0 : 7, 3, color),
                //new King(color ? 0 : 7, 4, color),
                //new Bishop(color ? 0 : 7, 5, color),
                //new Knight(color ? 0 : 7, 6, color),
                //new Rook(color ? 0 : 7, 7, color),

                //// set pawns
                //new Pawn(color ? 1 : 6, 0, color),
                //new Pawn(color ? 1 : 6, 1, color),
                //new Pawn(color ? 1 : 6, 2, color),
                //new Pawn(color ? 1 : 6, 3, color),
                //new Pawn(color ? 1 : 6, 4, color),
                //new Pawn(color ? 1 : 6, 5, color),
                //new Pawn(color ? 1 : 6, 6, color),
                //new Pawn(color ? 1 : 6, 7, color)


                // DEBUG
                // ----------------------------------
                // set pieces
                new Rook(color ? 0 : 7, 0, color),
                new Knight(color ? 0 : 7, 1, color),
                new Bishop(color ? 0 : 7, 2, color),
                new Queen(color ? 0 : 7, 3, color),
                new King(color ? 0 : 7, 4, color),
                new Bishop(color ? 3 : 4, 2, color),
                new Knight(color ? 2 : 5, 5, color),
                new Rook(color ? 0 : 7, 7, color),

                // set pawns
                new Pawn(color ? 1 : 6, 0, color),
                new Pawn(color ? 1 : 6, 1, color),
                new Pawn(color ? 1 : 6, 2, color),
                new Pawn(color ? 1 : 6, 3, color),
                new Pawn(color ? 3 : 4, 4, color),
                new Pawn(color ? 1 : 6, 5, color),
                new Pawn(color ? 1 : 6, 6, color),
                new Pawn(color ? 1 : 6, 7, color)
            };
        }
    }
}
