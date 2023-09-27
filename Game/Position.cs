using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class Position : ObservableObject
    {
        public bool Color { get; set; }
        public string AlphaCoordinates { get; set; }

        private Piece _piece;
        public Piece? Piece { get => _piece; set { _piece = value; NotifyPropertyChanged(this, nameof(Piece)); } }

        public int Y { get; set; }
        public int X { get; set; }

        public Position(bool color, int y, int x, Piece piece)
        {
            Color = color;
            Y = y;
            X = x;
            _piece = piece;
            AlphaCoordinates = _GetAlphaCoordinates(x, y);
        }
        public Position(int y, int x)
        {
            Y = y;
            X = x;
        }

        private string _GetAlphaCoordinates(int x, int y)
        {
            x++;
            y++;
            switch (x)
            {
                default:
                case 1: return $"a{y}";
                case 2: return $"b{y}";
                case 3: return $"c{y}";
                case 4: return $"d{y}";
                case 5: return $"e{y}";
                case 6: return $"f{y}";
                case 7: return $"g{y}";
                case 8: return $"h{y}";
            }
        }
        //public override bool Equals(object? obj)
        //{
        //    if (obj is Piece piece) return piece.X == X && piece.Y == Y;
        //    else return false;
        //}
    }
}
